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
    Stun = 6,
    Heal = 8,
    Knife = 9,
    Slam = 11,
    Electrocute = 14,
    Firebolt = 16
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
    [SerializeField] AudioSource slamSound;
    [SerializeField] AudioSource knifeSound;
    [SerializeField] AudioSource electrocuteSound;
    [SerializeField] AudioSource healSound;
    [SerializeField] AudioSource stunSound;
  

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
    Animator ghostAnimator;
    WaitForSeconds jumpWait = new WaitForSeconds(5.6f); // use to wait for anim to finish
    WaitForSeconds wait3sec = new WaitForSeconds(3f);
    WaitForSeconds wait2sec = new WaitForSeconds(2f);
    WaitForSeconds wait1sec = new WaitForSeconds(1f);
    GameObject ghostBasic;
    GameObject enemyReference;

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
        enemyHP = enemy.GetComponentInChildren<PlayerHealthBar>();
        battleDialog = GameObject.FindWithTag("BattleDialog").GetComponent<TextMeshProUGUI>();
        
        //freeze rotation/position
        playerRBConstraints = player.GetComponentInChildren<Rigidbody>().constraints;
        player.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;//freeze position xz, rotation

        enemyReference = GameObject.FindWithTag("enemyReference");

        enemyReference.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)122;

        enemyAnimator = enemyReference.GetComponent<Animator>(); // enemy animation controller
        ghostBasic = GameObject.Find("ghost basic");
        ghostAnimator = ghostBasic.GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        battleDialog = GameObject.FindWithTag("BattleDialog").GetComponent<TextMeshProUGUI>();
        enemyReference = GameObject.FindWithTag("enemyReference");
        //Debug.Log(enemyReference.name);

        player.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;//freeze position xz, rotation
        enemyReference.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)122;
        enemyAnimator = enemyReference.GetComponent<Animator>();
	    ghostAnimator = ghostBasic.GetComponent<Animator>();
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
                int enemyNewHP = enemyHP.TakeDamage((int)action.action, false);
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

        return lowerCaseEnemyName switch
        {
            string enemyName when enemyName.Contains("skeleton") => Time.renderedFrameCount % 50, //skeleton lower 2 abilities: knife or slam
            string enemyName when enemyName.Contains("monster") => Time.renderedFrameCount % 50 + 25, //monster middle 2: slam or fire
            string enemyName when enemyName.Contains("chess") || enemyName.Contains("horse") => Time.renderedFrameCount % 100, //final boss can do all 4 
            _ => 100  //default: electrocute
        };
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
                if (enemyReference.name.ToLower().Contains("skel"))
                {
                    battleDialog.text = dialogText.Replace("<harm>", "threw a swinging sword at");
                }
                else
                {
                    battleDialog.text = dialogText.Replace("<harm>", "threw a knife at");
                }
                yield return wait1sec;
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
        {   
            var enemyName = PlayerPrefs.GetString("ObjectToSpawn").ToLower();
            var damage = (int)enemyAction * (enemyName.Contains("chess") || enemyName.Contains("horse") ? 2 : 1);//if final boss x2 damage
            damage = damage / (enemyAction is CombatOptions.Electrocute && playerDodged ? 2 : 1);//id dodging electrocute, only 1/2 damage 
            playerHP.TakeDamage(damage, true);
        }
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
            var lowerCaseEnemyName = PlayerPrefs.GetString("ObjectToSpawn").ToLower();
            // This can be replaced with a confirmation UI when we're ready
            yield return new WaitForSecondsRealtime(2f);
            var sceneChanger = GetComponent<SceneChangeInvokable>();
            if (lowerCaseEnemyName.Contains("horse"))
            {
                // if defeat the boss, go to ending scene
                sceneChanger.sceneName = "EndingStory";
            }
            else
            {
                sceneChanger.sceneName = GameManager.Instance.PrepareForReturnFromCombat();
                sceneChanger.Invoke();
            }
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
            fire.transform.position = GameObject.FindWithTag("enemyReference").transform.position + new Vector3(-2, .5f, -1);
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
        yield return wait2sec;
        animator.Play("PlayerSlammed");
        slamSound.Play();
        yield return new WaitForSeconds(3.6f);
        enemyAnimator.SetBool(anim, false);
    }

    private IEnumerator ghostSlam(string anim)
    {
        ghostAnimator.SetBool(anim, true);
        yield return wait2sec;
        ghostAnimator.SetBool(anim, false);
    }

    GameObject sendSlam(bool isFromPlayer = true)
    {
        if(isFromPlayer)
        {
            StartCoroutine(ghostSlam("isCombat"));
            slamSound.Play();
        }
        else
        {
            if (enemyReference.name.ToLower().Contains("skel") || enemyReference.name.ToLower().Contains("eye") || enemyReference.name.ToLower().Contains("horse"))
            {
                StartCoroutine(animateAndWaitThenDeactivate("isCombat"));
            }
        }
        //animator.Play((isFromPlayer ? "Enemy" : "Player") + "Slammed");

        return null;
    }
    GameObject sendLightning(bool isFromPlayer = true)
    {
        var lightningObj = GameObject.Instantiate(lightningAsset);
        electrocuteSound.Play();
        var lightningComp = lightningObj.GetComponent<LightningBoltScript>();
        lightningComp.StartObject = GameObject.Find("ghost basic");
        lightningComp.EndObject = GameObject.FindWithTag("enemyReference");
        lightningComp.Generations = 3;

        return lightningObj;
    }

    private IEnumerator animateThrow(string anim)
    {
        enemyAnimator.SetBool(anim, true);
        yield return wait2sec;
        enemyAnimator.SetBool(anim, false);
    }

    private IEnumerator bossThrow(string anim)
    {
        enemyAnimator.SetBool(anim, true);
        yield return new WaitForSeconds(4f);
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
            if (enemyReference.name.ToLower().Contains("skel")) // sword throw skeleton
            {
                StartCoroutine(animateThrow("isThrow"));
            }
            else if (enemyReference.name.ToLower().Contains("horse"))
            {
                StartCoroutine(bossThrow("isThrow"));
            }
            else
            {
                animator.Play("EnemyThrowKnife");
            }
        }
        //animator.Play((isFromPlayer ? "Player" : "Enemy") + "ThrowKnife");
        knifeSound.Play();
        return null;
    }

    GameObject selfHeal(bool isFromPlayer = true)
    {
        try
        {
            GameObject.Instantiate(healAsset, GameObject.Find("ghost basic").transform);
        } catch (Exception) { }
        playerHP.TakeDamage(-(int)CombatOptions.Heal);
        battleDialog.text = $"You gained {(int)CombatOptions.Heal}HP";
        healSound.Play();

        return null;
    }
    GameObject sendStun(bool isFromPlayer = true)
    {
        Destroy(stunObj);//remove prev stun effect if any

        animator.Play("PlayerStun");
        stunSound.Play();
        try
        {
            stunObj = Instantiate(enemyStunAsset, gameObject.transform);
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
