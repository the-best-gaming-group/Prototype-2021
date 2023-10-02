using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public enum CombatOptions//rename
{
    Slam = 2,
    Firebolt = 3,
    Electrocute = 4,
    Icefreeze= 5
}

public class BattleSystem : MonoBehaviour
{
    public float dialogWaitTime = 2f;

    GameObject player;
    PlayerHealthBar playerHP;
    RigidbodyConstraints playerRBConstraints;
    GameObject enemy;
    PlayerHealthBar enemyHP;

    public GameObject firebolt;

    Text battleDialog;

    BattleState state;
    public bool isPlayerFirstTurn;

    bool playerDodged = false;

    Animator animator;
    List<(CombatOptions action, float waitTime, Action actionFunc)> turnActions = new List<(CombatOptions, float, Action)>();
    void Start()
    {
        animator = GetComponent<Animator>();
        state = BattleState.START;
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        playerHP = player.GetComponent<PlayerHealthBar>();
        enemyHP = enemy.GetComponent<PlayerHealthBar>();
        battleDialog = GameObject.FindWithTag("BattleDialog").GetComponent<Text>();

        //freeze rotation/position
        playerRBConstraints = player.GetComponentInChildren<Rigidbody>().constraints;
        player.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;//freeze position xz, rotation
        enemy.GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)122;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        battleDialog.text = "Fighting <enemy>";

        yield return new WaitForSeconds(dialogWaitTime);

        if (isPlayerFirstTurn)
            PlayerTurn();
        else
            EnemyTurn();
    }

    void PlayerTurn()
    {
        battleDialog.text = "What you gonna do?";
        state = BattleState.PLAYER_TURN;
        //todo: set limit to what actions can be done based on energy and exp
    }

    IEnumerator ProcessTurn()
    {
        foreach (var action in turnActions)
        {
            action.actionFunc();
            yield return new WaitForSeconds(action.waitTime);
            int enemyNewHP = enemyHP.TakeDamage(((int)action.action));
            battleDialog.text = "<enemyName> takes " + action.action.ToString();
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
            EndBattle();
    }

    IEnumerator EnemyTurn()
    {
        battleDialog.text = "<enemyName> attacks!";

        const int enemySlamDamage = 5; //todo: add other attack abilities
        if (!playerDodged)
        {
            int playerNewHP = playerHP.TakeDamage(enemySlamDamage);
            battleDialog.text = "<enemy> slammed <player>";
            animator.SetTrigger("PlayerSlam");
        }
        else
        {
            battleDialog.text = "<player> dodged, <enemy> slam attack failed";
        }

        playerDodged = false;

        yield return new WaitForSeconds(dialogWaitTime);

        if (playerHP.currentHealth <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYER_TURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            battleDialog.text = "You won against <enemy>";
            //move on w/ quest
        }
        else
        {
            battleDialog.text = "You lost against <enemy>";
            //move back to checkpoint
        }
        player.GetComponentInChildren<Rigidbody>().constraints = playerRBConstraints;//restore ability to move/rotate
    }

    void sendFirebolt()//use for enemy as well?
    {
        var currentPrefabObject = GameObject.Instantiate(firebolt);
        currentPrefabObject.transform.position = player.transform.position + new Vector3(1, 0, 0);//from player, need change for from enemy
        currentPrefabObject.transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);//from player to enemy, might need change for backward
    }
    void sendSlam()//use for enemy as well?
    {
        animator.SetTrigger("EnemySlam");
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
            turnActions.Add(new ValueTuple<CombatOptions, float, Action>(CombatOptions.Slam, 1, sendSlam)) ;
        }
    }

    public void OnFireButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(new ValueTuple<CombatOptions, float, Action>(CombatOptions.Firebolt, .1f, sendFirebolt));
        }
    }

    public void OnDodgeButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            playerDodged = !playerDodged;
            //for debugging
            if (playerDodged)
                battleDialog.text += "dodging";
            else battleDialog.text = battleDialog.text.Replace("dodging", "");
        }
    }
    public void OnEndTurnButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            StartCoroutine(ProcessTurn());
        }
    }
}
