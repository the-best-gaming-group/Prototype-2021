using DigitalRuby.LightningBolt;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Rune;

[Serializable]
public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public enum CombatOptions//rename
{
    Slam = 20,
    Firebolt = 25,
    Electrocute = 40,
    ThrowKnife = 5
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

    public GameObject fireboltAsset;
    public GameObject lightningAsset;
    public GameObject knifeAsset;

    TextMeshProUGUI battleDialog;

    public BattleState state;
    public bool isPlayerFirstTurn;
    private readonly List<Func<string>> playerTurnBeginListeners = new();
    private readonly List<Func<string>> playerTurnEndListeners = new();

    bool playerDodged = false;

    Animator animator;
    readonly List<TurnActions> turnActions = new ();
    public ResourceHandler resourceHandler = new();
    public Spell[] spells = new Spell[5];
    void Start()
    {
        animator = GetComponent<Animator>();
        state = BattleState.START;
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        playerHP = player.GetComponent<PlayerHealthBar>();
        playerHP.TakeDamage(100 - GameManager.Instance.GetPlayerHealth());
        enemyHP = enemy.GetComponent<PlayerHealthBar>();
        battleDialog = GameObject.FindWithTag("BattleDialog").GetComponent<TextMeshProUGUI>();
        
        //freeze rotation/position
        playerRBConstraints = player.GetComponentInChildren<Rigidbody>().constraints;
        player.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;//freeze position xz, rotation
        enemy.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;

        SetupSpells();

        StartCoroutine(SetupBattle());
    }
    
    /** 
     *  Going to need to move all of the spell creation to the GameManager,
     *  or at least some of it. We need to know the spells the player has
     *  when we replace them.
     */
    public void SetupSpells()
    {
        var slamCost = new int[4];
        slamCost[(int)FIRE] = 1;
        slamCost[(int)EARTH] = 1;
        spells[0] = new Spell {
            name = "Slam",
            effect = () =>
            {
                OnSlamButton();
                return "Pressed spell 1!";
            },
            cost = slamCost
        };
        
        var fireBallCost = new int[4];
        fireBallCost[(int)FIRE] = 2;
        fireBallCost[(int)AIR] = 1;
        spells[1] = new Spell {
            name = "Fire Ball",
            effect = () =>
            {
                OnFireButton();
                return "Pressed spell 2!";
            },
            cost = fireBallCost
        };

        var dodgeCost = new int[4];
        dodgeCost[(int)WATER] = 2;
        dodgeCost[(int)AIR] = 1;
        spells[2] = new Spell {
            name = "Dodge",
            effect = () =>
            {
                OnDodgeButton();
                return "Pressed spell 3!";
            },
            cost = dodgeCost
        };
        
        var lightningCost = new int[4];
        lightningCost[(int)FIRE] = 3;
        spells[3] = new Spell {
            name = "Electrocute",
            effect = () =>
            {
                OnElectrocuteButton();
                return "Pressed spell 4!";
            },
            cost = lightningCost
        };
        //following is extra
        var healCost = new int[4];
        healCost[(int)WATER] = 3;
        spells[4] = new Spell {
            name = "Heal",
            effect = () =>
            {
                return "Pressed spell 5!";
            },
            cost = healCost
        };
    }

    IEnumerator SetupBattle()
    {
        battleDialog.text = "Fighting the enemy!";

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
            //todo: maybe add code for enemy to randomly be able to dodge?
            battleDialog.text = "The enemy takes " + action.action.ToString();
            var gameObj = action.actionFunc(true);
            yield return new WaitForSeconds(action.waitTime);
            int enemyNewHP = enemyHP.TakeDamage((int)action.action);
            GameObject.Destroy(gameObj);
            yield return new WaitForSeconds(dialogWaitTime);
            if (enemyNewHP == 0)
            {
                state = BattleState.WON;
                break;
            }

        }
        turnActions.Clear();

        if (enemyHP.currentHealth > 0)
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
        else
            StartCoroutine(EndBattle());
    }

    IEnumerator EnemyTurn()
    {
        int randomInt = Time.renderedFrameCount % 100;
        CombatOptions enemyAction;
        String dialogText = "The enemy <harm> you";

        if (playerDodged) animator.Play("PlayerDodge");
        switch (randomInt)
        {
            case < 25:
                enemyAction = CombatOptions.Slam;
                battleDialog.text = playerDodged ? "You dodged enemy's slam!" : dialogText.Replace("<harm>", "slammed");
                if (!playerDodged) sendSlam(false);
                yield return new WaitForSeconds(1f);
                break;
            case < 50:
                enemyAction = CombatOptions.Firebolt;
                battleDialog.text = dialogText.Replace("<harm>", "threw a firebolt at");
                sendFirebolt(false);
                yield return new WaitForSeconds(.1f);
                break;
            case < 75:
                enemyAction = CombatOptions.Electrocute;
                battleDialog.text = dialogText.Replace("<harm>", "electrocutes");
                var lightning = sendLightning();
                yield return new WaitForSeconds(1f);
                Destroy(lightning);
                break;
            default:
                sendKnife(false);
                enemyAction = CombatOptions.ThrowKnife;
                battleDialog.text = dialogText.Replace("<harm>", "threw a knife at");
                yield return new WaitForSeconds(1f);
                break;
        }
        if (!playerDodged || enemyAction is CombatOptions.Electrocute)
            playerHP.TakeDamage((int)enemyAction);//todo: use other damage values?

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
            battleDialog.text = "You have prevailed!";
            // This can be replaced with a confirmation UI when we're ready
            yield return new WaitForSecondsRealtime(2f);
            var sceneChanger = GetComponent<SceneChangeInvokable>();
            sceneChanger.sceneName = GameManager.Instance.PrepareForReturnFromCombat();
            sceneChanger.Invoke();
        }
        else
        {
            battleDialog.text = "You were vanquished!";
            //move back to checkpoint
        }
    }

    GameObject sendFirebolt(bool isFromPlayer = true)//todo: change for enemy
    {
        var currentPrefabObject = GameObject.Instantiate(fireboltAsset);
        currentPrefabObject.transform.position = player.transform.position + new Vector3(1, 1, 0);
        currentPrefabObject.transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);//from player to enemy, might need change for backward

        return null;
    }
    GameObject sendSlam(bool isFromPlayer = true)
    {
        animator.Play((isFromPlayer ? "Enemy" : "Player") + "Slammed");

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

    GameObject sendKnife(bool isFromPlayer = true)
    { 
        animator.Play((isFromPlayer ? "Player" : "Enemy") + "ThrowKnife");

        return null;
    }

    void Update()
    {
    }


    /// <summary>
    /// combat buttons handlers
    /// </summary>
    public void OnSlamButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new (CombatOptions.Slam, 1, sendSlam)) ;
        }
    }

    public void OnFireButton()//todo: remove listener on mouse click
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new (CombatOptions.Firebolt, .1f, sendFirebolt));
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
            turnActions.Add(new(CombatOptions.Electrocute, 1f, sendLightning));
        }
    }

    public void OnKnifeButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new(CombatOptions.ThrowKnife, 1f, sendKnife));
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
}
