using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    [Header("Attack Hitbox")]
    public Transform attackPoint;
    public Vector3 attackSize = new Vector3(1f, 1f, 1f);
    public LayerMask playerLayer;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyManager enemyManager;

    private bool isAttacking = false;
    private bool canAttack = true;
    private bool hasHitPlayer = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        // เว้นระยะจากเป้าหมายประมาณ 3 เมตร
        agent.stoppingDistance = 3f;
    }

    private void Update()
    {
        if (player == null) return;

        // ระยะห่างระหว่าง Enemy กับ Player
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // ถ้ากำลังโจมตี ให้หยุดเดินและไม่เริ่มโจมตีซ้ำ
        if (isAttacking)
        {
            
            agent.isStopped = true;
            return;
        }

        // อยู่ในระยะโจมตี
        if (distance <= attackRange)
        {
            Debug.Log("เข้า Attack Range");
            // หยุด NavMeshAgent
            agent.isStopped = true;

            // หันหน้าเข้าหาผู้เล่น
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDir),
                    8f * Time.deltaTime
                );
            }

            // เริ่มโจมตีเมื่อพร้อม
            if (canAttack && !isAttacking)
            {
                Debug.Log("เริ่ม AttackRoutine");
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            // ออกนอกระยะโจมตี ให้ Movement ทำงานต่อ
            agent.isStopped = false;
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        canAttack = false;
        hasHitPlayer = false;

        // เปลี่ยนสถานะเป็นโจมตี
        enemyManager.ChangeState(EnemyState.Attack);

        // เล่น Animation โจมตี
        animator.SetTrigger("Attack");

        // รอจน Animation Event AttackEnd() ถูกเรียก
        // ไม่ใช้ WaitForSeconds เพราะให้ Animation เป็นตัวกำหนดจังหวะจบ
        yield return new WaitUntil(() => !isAttacking);

        // เริ่ม Cooldown หลังจบ Animation
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    // เรียกจาก Animation Event ตอนอาวุธฟาดโดนจังหวะโจมตี
    public void AttackHit()
    {
        if (hasHitPlayer) return;

        Collider[] hits = Physics.OverlapBox(
            attackPoint.position,
            attackSize * 0.5f,
            attackPoint.rotation,
            playerLayer
        );

        foreach (Collider hit in hits)
        {
            // ตรวจว่าคอลลाइडเดอร์นี้เป็นส่วนหนึ่งของ Player
            PlayerManager playerManager =
                hit.GetComponentInParent<PlayerManager>();

            if (playerManager == null) continue;

            // สร้างดาเมจให้ Player หนึ่งครั้งต่อการโจมตี
            hasHitPlayer = true;
            PlayerManager.Instance.SetHp(-1);
            break;
        }
    }

    // เรียกจาก Animation Event ตอน Animation โจมตีจบ
    public void AttackEnd()
    {
        isAttacking = false;

        // กลับไปทำงานต่อใน EnemyMovement
        if (enemyManager.CurrentState != EnemyState.Dead)
        {
            enemyManager.ChangeState(EnemyState.Chase);
        }
    }

    // แสดงกล่องโจมตีใน Scene View
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(
            attackPoint.position,
            attackPoint.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, attackSize);
    }
}