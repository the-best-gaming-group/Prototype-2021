using DigitalRuby.LightningBolt;
using Platformer.Mechanics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Rune;

[Serializable]
public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public enum CombatOptions
{
    Slam = 14,
    Firebolt = 22,
    Electrocute = 20,
    Knife = 11,
    Stun = 8,
    Heal = 10
}

public class TurnActions {
    public CombatOptions action;
    public float waitTime;
    public Func<bool, GameObject> actionFunc;
    public TurnActions(CombatOptions co, float f, Func<bool, GameObject> a)
    {
        action = co;
        waitTime = f;
        actionFunc = a;
    }
}

public class BattleSystem : MonoBehaviour
{
    public float dialogWaitTime = 2f;

    GameObject player;
    PlayerHealthBar playerHP;
    RigidbodyConstraints playerRBConstraints;
    GameObject enemy;
    PlayerHealthBar enemyHP;
    [SerializeField] AudioSource winSound;
    [SerializeField] AudioSource loseSound;
    public GameObject fireboltAsset;
    public GameObject lightningAsset;
    public GameObject knifeAsset;

  
    public GameObject enemyStunAsset;
    public GameObject stunObj;
    public GameObject healAsset;
    public GameObject healObj;
    public int remaningStunTurns = 0;


    TextMeshProUGUI battleDialog;

    public BattleState state;
    public bool isPlayerFirstTurn;
    private readonly List<Func<string>> battleStartListeners = new();
    private readonly List<Func<string>> playerTurnBeginListeners = new();
    private readonly List<Func<string>> playerTurnEndListeners = new();

    bool playerDodged = false;

    Animator animator;

    Animator enemyAnimator;
    WaitForSeconds jumpWait = new WaitForSeconds(5.6f); // use to wait for anim to finish
    WaitForSeconds wait3sec = new WaitForSeconds(3f);
    WaitForSeconds wait2sec = new WaitForSeconds(2f);
    WaitForSeconds wait1sec = new WaitForSeconds(1f);

    readonly List<TurnActions> turnActions = new ();
    public ResourceHandler resourceHandler = new();
    public GameManager.Spell[] spells = new GameManager.Spell[4];
    void Start()
    {
        animator = GetComponent<Animator>();
        state = BattleState.START;
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        playerHP = player.GetComponent<PlayerHealthBar>();
// playerHP.TakeDamage(-100);//todo: for testing, comment or rm        
        enemyHP = enemy.GetComponentInChildren<PlayerHealthBar>();
        battleDialog = GameObject.FindWithTag("BattleDialog").GetComponent<TextMeshProUGUI>();
        
        //freeze rotation/position
        playerRBConstraints = player.GetComponentInChildren<Rigidbody>().constraints;
        player.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;//freeze position xz, rotation
        enemy.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;

        enemyAnimator = enemy.GetComponentInChildren<Animator>(); // enemy animation controller

    }
    
    public void Resume()
    {
        GetComponentInChildren<TurnbasedDialogHandler>().Disable();
        StartCoroutine(SetupBattle());
    }

    /** 
     *  Going to need to move all of the spell creation to the GameManager,
     *  or at least some of it. We need to know the spells the player has
     *  when we replace them.
     */
    public void SetupSpells(string[] selectedSpells)
    {
        for (int i = 0; i < 4; i++)
        {
            Action eventFunc = selectedSpells[i] switch
            {
                "Dodge"       => OnDodgeButton,
                "Electrocute" => OnElectrocuteButton,
                "Fireball"    => OnFireButton,
                "Heal"        => OnHealButton,
                "Slam"        => OnSlamButton,
                "Stun"        => OnStunButton,
                "Knife Throw" => OnKnifeButton,
                _             => null
            };
            
            if (eventFunc != null)
            {
                GameManager.Instance.RegisterSpellEventFunc(selectedSpells[i], eventFunc);
            }
            else {
                Debug.Log("Error: Could not find spell named " + selectedSpells[i]);
            }
            spells[i] = GameManager.Instance.spells.Find(s => s.name.Equals(selectedSpells[i]));
        }
    }

    IEnumerator SetupBattle()
    {
        battleDialog.text = "Fighting the enemy!";
        
        foreach (var fun in battleStartListeners)
        {
            fun();
        }

        yield return new WaitForSeconds(dialogWaitTime);

        if (isPlayerFirstTurn)
            PlayerTurn();
        else
            EnemyTurn();
    }

    void PlayerTurn()
    {
        battleDialog.text = "What will you do?";
        state = BattleState.PLAYER_TURN;
        foreach (var fun in playerTurnBeginListeners)
        {
            fun();
        }
    }

    IEnumerator ProcessTurn()
    {
        foreach (var fun in playerTurnEndListeners)
        {
            fun();
        }

        foreach (var action in turnActions)
        {
            if (action.action == CombatOptions.Heal)
            {
                selfHeal();
            }
            else
            {
                battleDialog.text = "The enemy takes " + action.action.ToString();
                var gameObj = action.actionFunc(true);
                yield return new WaitForSeconds(action.waitTime);
                int enemyNewHP = enemyHP.TakeDamage(2*(int)action.action, false);
                GameObject.Destroy(gameObj);
            }

            yield return new WaitForSeconds(dialogWaitTime);
            if (enemyHP.currentHealth <= 0)
            {
                state = BattleState.WON;
                break;
            }

        }
        turnActions.Clear();

        if (state == BattleState.WON)
        {
            StartCoroutine(EndBattle());
        } else if (remaningStunTurns < 1)
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        } else
        {
            if (--remaningStunTurns == 0)
                Destroy(stunObj);
            PlayerTurn();
        }
    }

    public int getRandomAbilityBasedOnEnemyType()
    {
        var lowerCaseEnemyName = PlayerPrefs.GetString("ObjectToSpawn").ToLower();
        if (lowerCaseEnemyName.Contains("skeleton"))
        {
            return 1;
        }

        return Time.renderedFrameCount % 50 + (lowerCaseEnemyName.Contains("chess") || lowerCaseEnemyName.Contains("horse") ? 50 : 0);
    }

    IEnumerator EnemyTurn()
    {
        int randomInt = getRandomAbilityBasedOnEnemyType();
        CombatOptions enemyAction = CombatOptions.Knife;
        string dialogText = "The enemy <harm> you";

        if (playerDodged)
            animator.Play("PlayerDodge");
         
     
        switch (randomInt)
        {
            case < 25:
                sendKnife(false);
                var lowerCaseEnemyName = PlayerPrefs.GetString("ObjectToSpawn").ToLower();
                if (lowerCaseEnemyName.Contains("skeleton"))
                {
                    battleDialog.text = dialogText.Replace("<harm>", "threw a swinging sword at");
                }
                else
                {
                    battleDialog.text = dialogText.Replace("<harm>", "threw a knife at");
                }
                yield return new WaitForSeconds(1f);
                break;
            case < 50:
                enemyAction = CombatOptions.Slam;
                battleDialog.text = playerDodged ? "You dodged enemy's slam!" : dialogText.Replace("<harm>", "slammed");
                if (!playerDodged) sendSlam(false);
                yield return wait3sec; // important for animation to finish
                break;
            case < 75:
                enemyAction = CombatOptions.Firebolt;
                battleDialog.text = dialogText.Replace("<harm>", "threw a firebolt at");
                sendFirebolt(false);
                yield return wait1sec; //new WaitForSeconds(1f);
                break;
            default:
                enemyAction = CombatOptions.Electrocute;
                battleDialog.text = dialogText.Replace("<harm>", "electrocutes");
                var lightning = sendLightning();
                yield return wait1sec; //new WaitForSeconds(1f);
                Destroy(lightning);
                break;
        }
        if (!playerDodged || enemyAction is CombatOptions.Electrocute)
            playerHP.TakeDamage((int)enemyAction / (enemyAction is CombatOptions.Electrocute && playerDodged ? 2 : 1), true);

        playerDodged = false;

        yield return new WaitForSeconds(dialogWaitTime);

        if (playerHP.currentHealth <= 0)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYER_TURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        player.GetComponentInChildren<Rigidbody>().constraints = playerRBConstraints;//restore ability to move/rotate
        if (state == BattleState.WON)
        {
              winSound.Play();
            battleDialog.text = "You have prevailed!";
            // This can be replaced with a confirmation UI when we're ready
            yield return new WaitForSecondsRealtime(2f);
            var sceneChanger = GetComponent<SceneChangeInvokable>();
            sceneChanger.sceneName = GameManager.Instance.PrepareForReturnFromCombat();
            sceneChanger.Invoke();
        }
        else
        {
             loseSound.Play();
            battleDialog.text = "You were vanquished!";
            //move back to checkpoint
            yield return new WaitForSecondsRealtime(3f);
            // Debug.Log("AFter being defeated " + GameManager.Instance.GetPlayerHealth());
            GameManager.Instance.LoadCheckpoint();
        }
    }

    GameObject sendFirebolt(bool isFromPlayer = true)//todo: change for enemy
    {
        if (isFromPlayer)
        {
            GameObject fire = GameObject.Instantiate(fireboltAsset);
            fire.transform.position = player.transform.position + new Vector3(1, .5f, 1);
            fire.transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);
        }
        else
        {
            GameObject fire = GameObject.Instantiate(fireboltAsset);
            fire.transform.position = enemy.transform.position + new Vector3(-1, .5f, -1);
            fire.transform.rotation = new Quaternion(0, 0.70711f, 0, -0.70711f);
        }
        //var currentPrefabObject = GameObject.Instantiate(fireboltAsset);
        //int fireSrcOffset = isFromPlayer ? 1 : -1;
        //currentPrefabObject.transform.position = (isFromPlayer ? player : enemy).transform.position + new Vector3(fireSrcOffset, .5f, 0);
        //currentPrefabObject.transform.rotation = new Quaternion(0, 0.70711f * fireSrcOffset, 0, 0.70711f);

        return null;
    }
    
    private IEnumerator animateAndWaitThenDeactivate(string anim)
    {
        enemyAnimator.SetBool(anim, true);
        yield return jumpWait; //new WaitForSeconds(5.6f);
        enemyAnimator.SetBool(anim, false);
    }

    GameObject sendSlam(bool isFromPlayer = true)
    {
        if(isFromPlayer)
        {
            animator.Play("EnemySlammed");
        }
        else
        {
            StartCoroutine(animateAndWaitThenDeactivate("isCombat"));
        }
        //animator.Play((isFromPlayer ? "Enemy" : "Player") + "Slammed");

        return null;
    }
    GameObject sendLightning(bool isFromPlayer = true)
    {
        var lightningObj = GameObject.Instantiate(lightningAsset);
        var lightningComp = lightningObj.GetComponent<LightningBoltScript>();
        lightningComp.StartObject = player;
        lightningComp.EndObject = enemy;
        lightningComp.Generations = 3;

        return lightningObj;
    }

    private IEnumerator animateThrow(string anim)
    {
        enemyAnimator.SetBool(anim, true);
        yield return wait2sec;
        enemyAnimator.SetBool(anim, false);
    }

    GameObject sendKnife(bool isFromPlayer = true)
    {
        if (isFromPlayer)
        {
            animator.Play("PlayerThrowKnife");
        }
        else
        {
            StartCoroutine(animateThrow("isThrow"));
        }
        //animator.Play((isFromPlayer ? "Player" : "Enemy") + "ThrowKnife");
        return null;
    }

    GameObject selfHeal(bool isFromPlayer = true)
    {
        try
        {
            GameObject.Instantiate(healAsset, player.transform);
        } catch (Exception) { }
        playerHP.TakeDamage(-(int)CombatOptions.Heal);
        battleDialog.text = $"You gained {(int)CombatOptions.Heal}HP";

        return null;
    }
    GameObject sendStun(bool isFromPlayer = true)
    {
        Destroy(stunObj);//remove prev stun effect if any

        animator.Play("PlayerStun");
        try
        {
            stunObj = Instantiate(enemyStunAsset, enemy.transform);
        }
        catch (Exception) { }

        remaningStunTurns += 2;

        return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            battleDialog.text = PlayerPrefs.GetString("ObjectToSpawn").ToLower();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Destroy(healObj);
            Destroy(healObj);
        }
    }


    /// <summary>
    /// combat buttons handlers
    /// </summary>
    public void OnSlamButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new (CombatOptions.Slam, 1.5f, sendSlam)) ;
        }
    }

    public void OnFireButton()//todo: remove listener on mouse click
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new (CombatOptions.Firebolt, 1f, sendFirebolt));
        }
    }

    public void OnDodgeButton()//todo: remove listener on mouse click, need this func?
    {
        if (state == BattleState.PLAYER_TURN)
        {
            playerDodged = true;
        }
    }

    public void OnElectrocuteButton() //for quick debug, not really needed
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new(CombatOptions.Electrocute, 2f, sendLightning));
        }
    }

    public void OnKnifeButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new(CombatOptions.Knife, 1f, sendKnife));
        }
    }
    public void OnStunButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new(CombatOptions.Stun, 1f, sendStun));
        }
    }
    public void OnHealButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new(CombatOptions.Heal, 0, selfHeal));
        }
    }


    public void OnEndTurnButton()//todo: remove listener on mouse click
    {
        if (state == BattleState.PLAYER_TURN)
        {
            StartCoroutine(ProcessTurn());
        }
    }
    
    public void RegisterPlayerTurnBeginListener(Func<string> fun)
    {
        playerTurnBeginListeners.Add(fun);
    }

    public void RegisterPlayerTurnEndListener(Func<string> fun)
    {
        playerTurnEndListeners.Add(fun);
    }

    public void RegisterStartBattleListener(Func<string> fun)
    {
        battleStartListeners.Add(fun);
    }
}
