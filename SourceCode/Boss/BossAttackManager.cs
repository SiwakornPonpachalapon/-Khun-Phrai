using System.Collections;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Skills")]
    public BossStompAttack stompAttack;
    public BossRockAttack rockAttack;

    [Header("Attack Settings")]
    public float minAttackDelay = 2f;
    public float maxAttackDelay = 4f;

    private bool bossDead;
    private bool isAttacking;
    public static BossAttackManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(AttackLoop());
    }


    IEnumerator AttackLoop()
    {
        while (!bossDead)
        {
            // =========================
            // รอก่อนโจมตี
            // =========================

            float delay = Random.Range(
                minAttackDelay,
                maxAttackDelay
            );

            yield return new WaitForSeconds(delay);

            if (bossDead)
                yield break;


            // =========================
            // สุ่ม Animation
            // =========================

            int randomAttack = Random.Range(0, 2);

            isAttacking = true;

            if (randomAttack == 0)
            {
                // กระทืบพื้น
                animator.SetTrigger("Earthquake");
            }
            else
            {
                // โยนหิน
                animator.SetTrigger("RockDrop");
            }


            // รอ Animation Event บอกว่าโจมตีจบ
            yield return new WaitUntil(
                () => !isAttacking
            );
        }
    }


    // =====================================================
    // ANIMATION EVENTS
    // =====================================================

    // ใส่ Event ตอนเท้ากระแทกพื้น
    public void StompEvent()
    {
        if (bossDead)
            return;
        BossVoiceManager.Instance.PlayAttack1();
        stompAttack.StartStomp();
    }


    // ใส่ Event ตอนจังหวะปล่อย/โยนหิน
    public void RockEvent()
    {
        if (bossDead)
            return;
        BossVoiceManager.Instance.PlayAttack1();
        rockAttack.StartRockAttack();
    }


    // ใส่ Event ใกล้ท้าย Animation โจมตีทั้งสองท่า
    public void AttackEnd()
    {
        isAttacking = false;
    }


    // =====================================================
    // DEAD
    // =====================================================

    public void StopBoss()
    {
        if (bossDead)
            return;

        bossDead = true;
        isAttacking = false;

        StopAllCoroutines();
        BossVoiceManager.Instance.PlayDead();
        animator.SetTrigger("Dead");
    }
}