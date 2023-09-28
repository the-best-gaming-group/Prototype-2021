using DigitalRuby.PyroParticles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
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

    public GameObject player;
    PlayerHealthBar playerHP;
    public GameObject enemy;
    PlayerHealthBar enemyHP;

    public GameObject firebolt;

    public Text dialogText;//rename

    public BattleState state;
    public bool isPlayerFirstTurn;

    bool playerDodged = false;
    List<CombatOptions> turnActions = new List<CombatOptions>();

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        state = BattleState.START;
        playerHP = player.GetComponent<PlayerHealthBar>();
        enemyHP = enemy.GetComponent<PlayerHealthBar>();
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        dialogText.text = "Fighting <enemy>";// + enemyUnit.unitName;

        yield return new WaitForSeconds(dialogWaitTime);

        if (isPlayerFirstTurn)
            PlayerTurn();
        else
            EnemyTurn();
    }

    void PlayerTurn()
    {
        dialogText.text = "What you gonna do?";
        state = BattleState.PLAYER_TURN;
        //todo: set limit to what actions can be done based on energy and exp
    }

    IEnumerator ProcessTurn()
    {
        foreach (var action in turnActions)
        {
            animator.SetTrigger("Enemy" + action.ToString());

            if (action == CombatOptions.Firebolt)
            {
                var currentPrefabObject = GameObject.Instantiate(firebolt);
                currentPrefabObject.transform.position = player.transform.position + new Vector3(1, 0, 0);
                currentPrefabObject.transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);
                yield return new WaitForSeconds(.1f);
            }
            int enemyNewHP = enemyHP.TakeDamage(((int)action));
            dialogText.text = "<enemyName> takes " + action.ToString();
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
        dialogText.text = "<enemyName> attacks!";

        const int enemySlamDamage = 5; //todo: add other attack abilities
        if (!playerDodged)
        {
            int playerNewHP = playerHP.TakeDamage(enemySlamDamage);
            dialogText.text = "<enemy> slammed <player>";
            animator.SetTrigger("PlayerSlam");
        }
        else
        {
            dialogText.text = "<player> dodged, <enemy> slam attack failed";
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
            dialogText.text = "You won against <enemy>";
            //move on w/ quest
        }
        else
        {
            dialogText.text = "You lost against <enemy>";
            //move back to checkpoint
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    var currentPrefabObject = GameObject.Instantiate(firebolt);
        //    currentPrefabObject.transform.position = player.transform.position + new Vector3(1, 0, 0);
        //    currentPrefabObject.transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);
        //}
    }


    /// <summary>
    /// combat buttons handlers
    /// </summary>
    public void OnSlamButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(CombatOptions.Slam);
        }
    }

    public void OnFireButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(CombatOptions.Firebolt);
        }
    }

    public void OnDodgeButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            playerDodged = !playerDodged;
            //for debugging
            if (playerDodged)
                dialogText.text += "dodging";
            else dialogText.text = dialogText.text.Replace("dodging", "");
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
