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

    public enum AIState
    {
        isIdle,
        isPatrolling,
        isChasing
    }

    public AIState currentstate;

    public float waitAtPoint = 2f;
    [SerializeField] float waitCounter;
    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
    }

    // Update is called once per frame
    void Update()
    {

        switch(currentstate)
        {
            case AIState.isIdle:
                animator.SetBool("IsMoving", false);
                if(waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    currentstate = AIState.isPatrolling;
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);

                }
                break;

            case AIState.isPatrolling:

                if (agent.remainingDistance <= .2f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }

                    //agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                    currentstate = AIState.isIdle;
                    waitCounter = waitAtPoint;
                }

                animator.SetBool("IsMoving", true);
                break;

        }
        
    }
}
