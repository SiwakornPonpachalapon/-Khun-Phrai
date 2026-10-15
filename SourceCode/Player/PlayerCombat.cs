using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    private bool isFirstShoot = true;
    private bool isWaitingVoice = false;
    public static PlayerCombat Instance;
    [Header("Damage")]
    public int attackDamage = 20;
    // =========================
    // Hitbox
    // =========================

    [Header("Hitbox")]

    // จุดกึ่งกลาง Hitbox
    public Transform attackPoint;

    // ขนาด Hitbox
    public Vector3 attackSize = new Vector3(1f, 1f, 1.5f);

    // Layer ของศัตรู
    public LayerMask enemyLayer;
    // =========================
    // กล้อง
    // =========================

    public Camera playerCamera;

    // UI เป้าเล็ง
    public GameObject crosshairUI;

    // =========================
    // ระยะยิง
    // =========================

    public float shootRange = 100f;

    // =========================
    // Combo
    // =========================

    public int comboIndex = 0;

    public float comboResetTime = 1f;


    // =========================
    // Aim Mode
    // =========================

    // =========================
    // Cooldown หลัง Combo จบ
    // =========================

    private bool isRecovery;

    public float recoveryTime = 0.5f;
    // กำลังโจมตีอยู่ไหม
    private bool isAttacking;

    // ผู้เล่นกดต่อ Combo แล้วหรือยัง
    private bool comboQueued;

    [SerializeField]
    private Animator animator;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Instance = this;
    }

    void Update()
    {
        HandleAim();

        HandleMeleeAttack();

        HandleShoot();

    }

    // ==================================
    // คลิกขวา = เล็ง
    // ==================================
    void HandleAim()
    {

        if (!WeaponManager.Instance.HasSword)
        {
            PlayerManager.Instance.SetAim(false);
            crosshairUI.SetActive(false);
            animator.SetBool("Aim", false);
            return;
        }

        bool isAiming = Mouse.current.rightButton.isPressed;

        PlayerManager.Instance.SetAim(isAiming);

        crosshairUI.SetActive(isAiming);

        animator.SetBool("Aim", isAiming);

    }

    // ==================================
    // คลิกซ้าย = ตี
    // ==================================

    void HandleMeleeAttack()
    {
        // ยังไม่มีดาบ = ยังต่อสู้ไม่ได้
        if (!WeaponManager.Instance.HasSword)
        {
            return;
        }
        if (PlayerManager.Instance.IsAiming)
            return;

        if (isRecovery)
            return;

        if(PlayerManager.Instance.CurrentState == PlayerStats.Dash)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isAttacking)
        {
            StartAttack();
        }
        else
        {
            comboQueued = true;
        }
        }
    }

    // ==================================
    // คลิกซ้ายตอนเล็ง = ยิง
    // ==================================

    void HandleShoot()
    {
        if (!PlayerManager.Instance.IsAiming)
            return;

        // กำลังพูดอยู่ = ห้ามกดยิงซ้ำ
        if (isWaitingVoice)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (PlayerManager.Instance.Ammo <= 0)
            {
                //Debug.Log("กระสุนหมด!");
                return;
            }

            // ยิงครั้งแรก → พูดก่อน
            if (isFirstShoot)
            {
                StartCoroutine(FirstShoot());
                return;
            }

            // ครั้งต่อไปยิงปกติ
            Shoot();
        }
    }

    // ==================================
    // พักหลัง Combo 4
    // ==================================

    IEnumerator Recovery()
    {
        isRecovery = true;

        yield return new WaitForSeconds(recoveryTime);

        comboIndex = 0;

        isRecovery = false;
    }
    void StartAttack()
    {
        isAttacking = true;

        PlayerManager.Instance.ChangeState(PlayerStats.Attack);

        comboIndex = 1;

        animator.SetInteger("Combo", 1);
        //เสียงฟัน
        AudioManager.Instance.PlaySwing();

        // animator.SetInteger("Combo",1);
    }
    public void NextCombo()
    {
        if(comboQueued)
        {
            comboQueued = false;

            comboIndex++;

            animator.SetInteger("Combo", comboIndex);
            //เสียงฟัน
            AudioManager.Instance.PlaySwing();
        }
        else
        {
            EndCombo();
        }
    }
    public void EndCombo()
    {
        comboIndex = 0;

        animator.SetInteger("Combo", 0);

        isAttacking = false;

        comboQueued = false;

        PlayerManager.Instance.ChangeState(PlayerStats.Idle);

        
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.matrix =
            Matrix4x4.TRS(
                attackPoint.position,
                attackPoint.rotation,
                Vector3.one);

        Gizmos.DrawWireCube(
            Vector3.zero,
            attackSize);
    }
    public void AttackHit()
    {
        Collider[] hits = Physics.OverlapBox(
            attackPoint.position,
            attackSize * 0.5f,
            attackPoint.rotation,
            enemyLayer);
        HashSet<EnemyHealth> damagedEnemies =
        new HashSet<EnemyHealth>();

        //Debug.Log($"Hit {hits.Length} Enemy");

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemy == null)
                continue;

            if(damagedEnemies.Contains(enemy))
            continue;

            damagedEnemies.Add(enemy);

            enemy.TakeDamage(attackDamage);
            CombatEffect.Instance.PlayHit(enemy, attackPoint);
            //เสียงตีโดน
            AudioManager.Instance.PlayHit();
        }
    }
    public void FireGun()
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            //Debug.Log($"ยิงโดน {hit.collider.name}");

            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                 if (hit.collider.CompareTag("Head"))
                {
                    enemy.TakeDamage(80);
                }
                else
                {
                    enemy.TakeDamage(20);
                }
            }

            BossHealth boss = hit.collider.GetComponentInParent<BossHealth>();

            if (boss != null)
            {
                if (hit.collider.CompareTag("Head"))
                {
                    boss.TakeDamage(80);
                }
                else
                {
                    boss.TakeDamage(20);
                }

                return;
            }
        }
    }
    //Animation
    public void PlayAnimation(string AnimationName)
    {
        animator.SetTrigger(AnimationName);
    }
    public void EndWoodCut()
    {
        PlayerManager.Instance.ChangeState(PlayerStats.Idle);
    }
    IEnumerator FirstShoot()
    {
        isWaitingVoice = true;

        // เล่นบทพูด และรอจนพูดจบ
        yield return StartCoroutine(
            AudioManager.Instance.PlayFirstShootVoice()
        );

        //isFirstShoot = false;
        isWaitingVoice = false;

        // พูดจบ → ยิงทันที
        Shoot();
    }
    void Shoot()
    {
        if (PlayerManager.Instance.Ammo <= 0)
            return;

        // ใช้กระสุน
        PlayerManager.Instance.AddAmmo(-1);

        PlayerManager.Instance.UpdateInventory();

        // เล่น Animation ยิง
        animator.SetTrigger("Shoot");
    }
}