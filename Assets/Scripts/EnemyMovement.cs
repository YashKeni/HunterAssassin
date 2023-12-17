using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMovement : MonoBehaviour
{
    [Header("Player to Target")]
    [SerializeField] public Transform player;

    [Header("Distances")]
    [SerializeField] float chaseRange = 8;
    [SerializeField] float randomPointRadius = 20;
    [SerializeField] float notificationRange = 12f;

    [Header("Speeds")]
    [SerializeField] float engageSpeed = 5;
    [SerializeField] float randomSpeed = 4;

    [Header("Relation with Player")]
    [SerializeField] bool canSeePlayer;
    [SerializeField] bool hasSeen;
    [SerializeField] bool hasDied;

    [Header("Misc")]
    [SerializeField] bool goingRandom;
    [SerializeField] bool takeNewPath;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] DifficultyGenerator difficultyGenerator;

    float distanceToTarget = Mathf.Infinity;

    NavMeshAgent navMeshAgent;
    Animator animator;
    PlayerHealth playerHealth;
    Vector3 newPos;

    string currentSceneName;
    string difficultyLevel;
    float difficultyFOV;
    float difficultyViewDistance;

    static List<EnemyMovement> allEnemies = new List<EnemyMovement>();
    bool notifiedNearbyEnemies = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        canSeePlayer = false;
        currentSceneName = SceneManager.GetActiveScene().name;

        newPos = transform.position;

        foreach (DifficultyGenerator.Levels levels in difficultyGenerator.myLevels)
        {
            if (currentSceneName == levels.levelName)
            {
                difficultyLevel = levels.levelName;
                difficultyFOV = levels.enemyFOVAngle;
                difficultyViewDistance = levels.enemyViewDistance;
            }
        }

        allEnemies.Add(this);
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(player.transform.position, transform.position);

        if (canSeePlayer && !hasDied) { hasSeen = true; }

        if (distanceToTarget > chaseRange) { hasSeen = false; }

        if (hasSeen)
        {
            EngageOnPlayer();

            if (!notifiedNearbyEnemies && distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                NotifyNearbyEnemies();
                notifiedNearbyEnemies = true;
            }
        }
        else
        {
            // Reset notification flag when player is not seen
            notifiedNearbyEnemies = false;
        }

        if (!hasSeen)
        {
            if (goingRandom)
            {
                TakeRandomPath();
                if (takeNewPath == true)
                {
                    newPos = transform.position;
                    takeNewPath = false;
                }
            }
            else
                StayIdle();
        }

        if (hasDied) { navMeshAgent.SetDestination(transform.position); }
    }

    private void FixedUpdate()
    {
        CreateFOV();
    }

    void NotifyNearbyEnemies()
    {
        List<EnemyMovement> enemiesToNotify = new List<EnemyMovement>(allEnemies);

        // Find nearby enemies to notify
        foreach (EnemyMovement enemy in enemiesToNotify)
        {
            if (enemy == null || enemy == this || enemy.notifiedNearbyEnemies)
                continue;

            float distanceToOtherEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToOtherEnemy <= chaseRange)
            {
                enemy.StartChasing();
            }
        }

        // Mark this enemy as notified
        notifiedNearbyEnemies = true;
    }

    void StartChasing()
    {
        hasSeen = true;
    }

    private void StayIdle()
    {
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("EnemyRun", false);
    }

    private void TakeRandomPath()
    {
        animator.SetBool("EnemyRun", true);
        animator.SetBool("Attack", false);
        if (Vector3.Distance(newPos, transform.position) <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.speed = randomSpeed;
            newPos = EnemyRandomGen.RandomPath(transform.position, randomPointRadius);
            navMeshAgent.SetDestination(newPos);
        }
    }

    private void EngageOnPlayer()
    {
        if (distanceToTarget > navMeshAgent.stoppingDistance)
        {
            ChasePlayer();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackPlayer();
        }
        takeNewPath = true;
    }

    public void ChasePlayer()
    {
        navMeshAgent.speed = engageSpeed;
        navMeshAgent.SetDestination(player.transform.position);
        animator.SetBool("EnemyRun", true);
        animator.SetBool("Attack", false);
        hasSeen = true;
    }

    private void AttackPlayer()
    {
        FaceTarget();
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("EnemyRun", false);
        if (playerHealth.hitPoints == 0)
        {
            animator.SetBool("Attack", false);
        }
        else if (playerHealth.hitPoints != 0)
        {
            animator.SetBool("Attack", true);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);
    }

    IEnumerator FOV()
    {
        float delay = 0.2f;
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CreateFOV();
        }
    }
    private void CreateFOV()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, difficultyViewDistance, targetMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 direction = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direction) < difficultyFOV / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, direction, distance, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Max View Distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, difficultyViewDistance);
        Gizmos.DrawLine(transform.position, newPos);

        // Max Chase Distance
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Can See Player
        if (canSeePlayer)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}