using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotateSpeed = 5f;
    [Tooltip("Stop the enemy within this distance, should be bigger than NavMeshAgent stop distance")]
    [SerializeField] private float stopDistance = 2.7f;

    private Enemy enemy;
    private float distanceToTarget;
    private bool isProvoke;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        isProvoke = false;
    }

    void Update()
    {
        ProvokeCheck();
    }

    private float DistanceToTarget()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    private void ProvokeCheck()
    {
        if (enemy == null) return;

        distanceToTarget = DistanceToTarget();
        if (isProvoke)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= enemy.chaseRange)
        {
            isProvoke = true; // temporarily
        }
    }

    private void EngageTarget()
    {
        if (navMeshAgent == null) return;

        if (distanceToTarget > stopDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        FaceToTarget();
        AttackState(true);
        Debug.Log("Got Attack");
    }

    private void ChaseTarget()
    {
        MoveState();
        AttackState(false);
        navMeshAgent.SetDestination(target.position);
    }

    private void FaceToTarget()
    {
        //Calculate direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        //Create rotation then smoothly rotate the enemy
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }

    public void IdleStae()
    {
        animator.SetTrigger("idle");
    }

    public void MoveState()
    {
        animator.SetTrigger("move");
    }

    public void AttackState(bool state)
    {
        animator.SetBool("attack", state);
    }
}
