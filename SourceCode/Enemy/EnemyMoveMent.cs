
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement Settings")]
    public float returnDistance = 0.2f;

    [Header("Speed")]
    public float runSpeed = 5f;
    public float walkSpeed = 2f;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 startPosition;

    // เช็กว่า Enemy เคยเริ่มไล่ผู้เล่นแล้วหรือยัง
    private bool hasChased = false;
    private bool playerInChaseRange;
    private EnemyManager enemyManager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();

        // จำตำแหน่งเริ่มต้น
        startPosition = transform.position;

        // เริ่มเกมให้ Enemy ยืนนิ่ง
        agent.isStopped = true;
        agent.ResetPath();

        animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        if (player == null) return;

        // หยุด Movement ระหว่างโจมตี โดนตี หรือตาย
        if (enemyManager.CurrentState == EnemyState.Attack ||
            enemyManager.CurrentState == EnemyState.Hit ||
            enemyManager.CurrentState == EnemyState.Dead)
        {
            agent.isStopped = true;
            return;
        }
        // =====================================
        // 1. ผู้เล่นเข้าระยะ → วิ่งไล่
        // =====================================
        if (playerInChaseRange)
        {
            hasChased = true;

            enemyManager.ChangeState(EnemyState.Chase);

            agent.isStopped = false;
            agent.speed = runSpeed;
            agent.SetDestination(player.position);

            animator.SetFloat("Speed", 2f);
        }
        // =====================================
        // 2. ยังไม่เคยไล่ → ยืนนิ่ง
        // =====================================
        else if (!hasChased)
        {
            agent.isStopped = true;
            agent.ResetPath();

            animator.SetFloat("Speed", 0f);

            enemyManager.ChangeState(EnemyState.Idle);
        }
        // =====================================
        // 3. เคยไล่แล้ว แต่ผู้เล่นออกนอกระยะ
        // → เดินกลับจุดเริ่มต้น
        // =====================================
        else
        {
            agent.isStopped = false;
            agent.speed = walkSpeed;
            agent.SetDestination(startPosition);

            animator.SetFloat("Speed", 1f);

            // เช็กว่ากลับถึงจุดเดิมแล้วหรือยัง
            if (!agent.pathPending &&
                agent.remainingDistance <=
                agent.stoppingDistance + returnDistance)
            {
                agent.isStopped = true;
                agent.ResetPath();

                animator.SetFloat("Speed", 0f);

                enemyManager.ChangeState(EnemyState.Idle);

                // กลับถึงจุดเดิมแล้ว รอผู้เล่นเข้าระยะใหม่
                hasChased = false;
            }
        }
    }
    public void SetPlayerInChaseRange(bool value)
    {
        playerInChaseRange = value;
    }
}