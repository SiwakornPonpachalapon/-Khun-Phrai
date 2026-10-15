using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossStompAttack : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Wave")]
    public GameObject wavePrefab;
    public Transform waveSpawnPoint;

    [Header("Warning")]
    public GameObject warningLine;
    public float warningTime = 1.5f;
    public float warningLength = 20f;
    public float warningWidth = 2f;

    [Header("Attack")]
    //public float attackCooldown = 4f;

    private bool isAttacking;

    void Start()
    {
        if (warningLine != null)
        {
            warningLine.SetActive(false);
        }
    }

    void Update()
    {
        // TEST ชั่วคราว
        // กด T ให้ Boss ใช้ Skill
        /*if (Keyboard.current.tKey.wasPressedThisFrame && !isAttacking)
        {
            StartCoroutine(StompAttack());
        }*/
    }

    IEnumerator StompAttack()
    {
        isAttacking = true;

        // =========================
        // 1. หาทิศ Player
        // =========================

        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;
        direction.Normalize();


        // =========================
        // 2. ล็อกทิศ
        // =========================

        Vector3 lockedDirection = direction;

        // Boss หันหา Player
        if (lockedDirection != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(lockedDirection);
        }


        // =========================
        // 3. Warning
        // =========================

        ShowWarning(lockedDirection);

        yield return new WaitForSeconds(warningTime);

        HideWarning();


        // =========================
        // ANIMATION
        // =========================

        // ตอนมี Animation ค่อยเปิด
        //
        // animator.SetTrigger("Stomp");
        //
        // แล้วใน Animation Event
        // ค่อยเรียก SpawnWave()
        //
        // ตอนนี้ยังไม่มี Animation
        // เลย Spawn ทันที


        // =========================
        // 4. สร้าง Wave
        // =========================

        SpawnWave(lockedDirection);


        // =========================
        // 5. Cooldown
        // =========================

        //yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


    void ShowWarning(Vector3 direction)
    {
        if (warningLine == null)
            return;

        warningLine.SetActive(true);

        // อยู่กลางเส้น
        Vector3 center =
            waveSpawnPoint.position
            + direction * (warningLength * 0.5f);

        warningLine.transform.position = center;

        // หันตามทิศยิง
        warningLine.transform.rotation =
            Quaternion.LookRotation(direction);

        // ยืด Cube
        warningLine.transform.localScale =
            new Vector3(
                warningWidth,
                0.02f,
                warningLength
            );
    }


    void HideWarning()
    {
        if (warningLine != null)
        {
            warningLine.SetActive(false);
        }
    }


    void SpawnWave(Vector3 direction)
    {
        GameObject wave = Instantiate(
            wavePrefab,
            waveSpawnPoint.position,
            Quaternion.LookRotation(direction)
        );

        GroundWave groundWave =
            wave.GetComponent<GroundWave>();

        if (groundWave != null)
        {
            groundWave.SetDirection(direction);
        }
    }
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public void StartStomp()
    {
        if (!isAttacking)
        {
            StartCoroutine(StompAttack());
        }
    }
}