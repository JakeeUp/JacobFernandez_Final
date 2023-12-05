using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int currentPatrolPoint;

    public NavMeshAgent agent;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);

        if(agent.remainingDistance <= .2f)
        {
            currentPatrolPoint++;
            if(currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }

            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }

        animator.SetBool("IsMoving", true);
    }
}
