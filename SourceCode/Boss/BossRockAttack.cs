using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossRockAttack : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Rock")]
    public GameObject rockPrefab;
    public Transform rockSpawnPoint;

    [Header("Warning")]
    public GameObject warningPrefab;
    public float warningTime = 1.5f;

    [Header("Attack")]
    //public float attackCooldown = 4f;

    private bool isAttacking;

    void Update()
    {
        // =========================
        // TEST
        // =========================
        // กด Y เพื่อทดสอบโยนหิน
        /*if (Keyboard.current.yKey.wasPressedThisFrame && !isAttacking)
        {
            StartCoroutine(RockAttack());
        }*/
    }

    IEnumerator RockAttack()
    {
        isAttacking = true;


        // =========================
        // 1. ล็อกตำแหน่ง Player
        // =========================

        Vector3 targetPosition = player.position;


        // =========================
        // 2. สร้าง Warning
        // =========================

        GameObject warning = Instantiate(
            warningPrefab,
            targetPosition + Vector3.up * 0.02f,
            Quaternion.identity
        );


        // =========================
        // ANIMATION
        // =========================

        // ตอนมี Animation ค่อยใช้
        //
        // animator.SetTrigger("RockAttack");
        //
        // เช่น:
        // Boss ยกหินขึ้น
        // ↓
        // Warning ขึ้น
        // ↓
        // Animation Event
        // ↓
        // ThrowRock();


        // =========================
        // 3. ให้เวลาผู้เล่นหลบ
        // =========================

        yield return new WaitForSeconds(warningTime);


        // =========================
        // 4. โยนหิน
        // =========================

        SpawnRock(targetPosition);


        // Warning จะอยู่จนหินตก
        // Projectile จะเป็นคนลบทีหลัง


        // =========================
        // 5. Cooldown
        // =========================

        //yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


    void SpawnRock(Vector3 targetPosition)
    {
        GameObject rock = Instantiate(
            rockPrefab,
            rockSpawnPoint.position,
            Quaternion.identity
        );

        BossRockProjectile projectile =
            rock.GetComponent<BossRockProjectile>();

        if (projectile != null)
        {
            projectile.Launch(targetPosition);
        }
    }
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public void StartRockAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(RockAttack());
        }
    }
}