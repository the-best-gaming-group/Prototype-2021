using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public string uID;
    GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    //patro
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    // state range
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		//agent.updateRotation = false;
        agent.speed = 2;
		player = GameObject.Find("GhostPC");
        animator = GetComponent<Animator>();
	}

	void Update()
    {
		Vector3 newPosition = transform.position;
		newPosition.z = player.transform.position.z;
		transform.position = newPosition;

		if (SceneManager.GetActiveScene().name != "Combat Arena")
		{
			playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
			playerInSight = Physics.CheckSphere(transform.position, attackRange, playerLayer);
			if (!playerInSight && !playerInAttackRange) Patrol();
			if (playerInSight && !playerInAttackRange) Chase();
            else
            {
                animator.SetBool("isChasing", false);
            }
            if (playerInSight && playerInAttackRange) Attack();
            

		}
    }

    void Chase()
    {
		if (SceneManager.GetActiveScene().name != "Combat Arena")
        {

            animator.SetBool("isChasing", true);
            Debug.Log("Chase");
			agent.SetDestination(player.transform.position);
			agent.speed = 3.5f;
		}
    }

    // function does not seem to be executing ? 
    void Attack()
    {
		Debug.Log("Attack");
        transform.LookAt(player.transform);
   
	}


    void Patrol()
    {
        if (!walkpointSet) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);
		// If the distance between the enemy's current position and the destPoint is less than 10 units
        // Sets walkpointSet to false, indicating that a new destination needs to be found
		if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        // float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
        // Debug.Log("DesPoint" + destPoint);

		//Performs a raycast from destPoint downwards to check if it hits an object on the groundLayer
        //If it does, walkpointSet is set to true, indicating that this point is a valid destination for the enemy to walk towards
		if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
