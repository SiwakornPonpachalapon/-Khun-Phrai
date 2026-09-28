using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMoveMent : MonoBehaviour
{
    [Header("MoveMent")]

    // ความเร็วเดิน
    public float WalkSpeed = 5f;

    // ความเร็ววิ่ง
    public float RunSpeed = 8f;

    // ความเร็วปัจจุบัน
    private float currentSpeed;

    // ความเร็วในการหมุน
    public float rotateSpeed = 5f;
    // =========================
    // Dash
    // =========================

    // ระยะ Dash (เมตร)
    public float DashDistance = 4f;
    // โบนัสระยะ Dash ตอนวิ่งเต็มสปีด
    public float RunDashBonus = 1f;

    // ระยะเวลาที่ Dash
    public float DashDuration = 0.2f;

    // เวลาคูลดาวน์
    public float DashCooldown = 1f;

    // กำลัง Dash อยู่หรือไม่
    private bool isDashing = false;

    // Dash ได้หรือยัง
    private bool canDash = true;
    // เวลาที่ Dash ไปแล้ว
    private float dashTimer = 0f;
    [Header("Camera")] 
    // กล้องหลัก
    public Transform cameraTransform;

    // Rigidbody ของ Player
    private Rigidbody rb;

    // ค่าที่รับจาก WASD
    private Vector2 moveInput;
    // ทิศที่ใช้ Dash
    private Vector3 dashDirection;
    [Header("Animator")]
    // Animator ของโมเดล
    [SerializeField]
    private Animator animator;
    private PlayerCombat combat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<PlayerCombat>();
        
    }
    void Start()
    {
        currentSpeed = WalkSpeed;
    }
    void wasdMovement()
    {
            // อ่าน Input ตลอด
        float x = (Keyboard.current.aKey.isPressed ? -1 : 0)
                + (Keyboard.current.dKey.isPressed ? 1 : 0);

        float y = (Keyboard.current.sKey.isPressed ? -1 : 0)
                + (Keyboard.current.wKey.isPressed ? 1 : 0);

        moveInput = new Vector2(x, y);

        // Run
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = RunSpeed;
        }
        else
        {
            currentSpeed = WalkSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        wasdMovement();
        // =========================
        // Dash Input
        // =========================

        if (Keyboard.current.qKey.wasPressedThisFrame && canDash)
        {
            // ถ้ามีการกด WASD
            if (moveInput.magnitude > 0.01f)
            {
                Vector3 camForward = cameraTransform.forward;
                camForward.y = 0;
                camForward.Normalize();

                Vector3 camRight = cameraTransform.right;
                camRight.y = 0;
                camRight.Normalize();

                dashDirection =
                    (camForward * moveInput.y + camRight * moveInput.x).normalized;
            }
            else
            {
                // ถ้ายืนเฉย ๆ Dash ไปด้านหน้าตัวละคร
                dashDirection = transform.forward;
            }

            if (combat != null)
            {
                combat.EndCombo();
            }
            
            StartCoroutine(Dash());
        }
    }
    void FixedUpdate()
    {
        // ทิศทางกล้อง (ตัดแกน Y)
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // แปลง input → ทิศทางโลก ตามกล้อง
        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        // ความเร็วที่ส่งให้ Blend Tree
        float animationSpeed = 0;

        // =========================
        // ถ้ากำลัง Dash จะไม่ใช้ระบบเดิน
        // =========================

        if (isDashing)
        {
            // ความเร็วปัจจุบันคิดเป็นกี่ % ของความเร็ววิ่ง
            float speedPercent = currentSpeed / RunSpeed;

            float finalDashDistance =
                DashDistance + (RunDashBonus * speedPercent);

            float progress = dashTimer / DashDuration;

            float speedMultiplier = Mathf.Lerp(1f, 0.2f, progress);

            float dashSpeed =
                (finalDashDistance / DashDuration) * speedMultiplier;

            rb.MovePosition(
                rb.position +
                dashDirection * dashSpeed * Time.fixedDeltaTime
            );

            return;
        }

        // =========================
        // ถ้าสถานะปัจจุบัน เดินไม่ได้
        // =========================

        if (PlayerManager.Instance.CurrentState == PlayerStats.Move || PlayerManager.Instance.CurrentState == PlayerStats.Idle)
        {
            if (PlayerManager.Instance.IsAiming)
            {
                Vector3 lookDir = cameraTransform.forward;
                lookDir.y = 0f;

                Quaternion targetRot = Quaternion.LookRotation(lookDir);

                rb.rotation = Quaternion.Slerp(
                    rb.rotation,
                    targetRot,
                    rotateSpeed * Time.fixedDeltaTime
                );
            }
            else if (moveDir.magnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);

                rb.rotation = Quaternion.Slerp(
                    rb.rotation,
                    targetRot,
                    rotateSpeed * Time.fixedDeltaTime
                );
            }
            float moveSpeed = currentSpeed;

            if (PlayerManager.Instance.IsAiming)
            {
                moveSpeed *= 0.7f;
            }
            if (moveDir.magnitude > 0.01f)
            {
                rb.MovePosition(
                    rb.position +
                    moveDir * moveSpeed * Time.fixedDeltaTime
                );
            }
        }
        

        // เดิน
        if (moveDir.magnitude > 0.01f &&
            (
            PlayerManager.Instance.CurrentState == PlayerStats.Move ||
            PlayerManager.Instance.CurrentState == PlayerStats.Idle))
        {
            PlayerManager.Instance.ChangeState(PlayerStats.Move);
            animationSpeed = 1;
        }

        // วิ่ง
        if (moveDir.magnitude > 0.01f &&
            currentSpeed == RunSpeed)
        {
            animationSpeed = 2;
        }
        // ส่งค่าให้ Blend Tree
        animator.SetFloat("Speed", animationSpeed, 0.15f, Time.deltaTime);
        if(moveDir.magnitude <= 0.01f &&
            (PlayerManager.Instance.CurrentState == PlayerStats.Move ||
            PlayerManager.Instance.CurrentState == PlayerStats.Idle))
        {
            PlayerManager.Instance.ChangeState(PlayerStats.Idle);
        }
    }
    // =========================
    // Dash
    // =========================

    IEnumerator Dash()
    {
        //เสียงDash
        AudioManager.Instance.PlayDash();
        // กำลัง Dash
        isDashing = true;

        PlayerManager.Instance.ChangeState(PlayerStats.Dash);

        // ยัง Dash ซ้ำไม่ได้
        canDash = false;

        // เล่น Animation Dash
        animator.SetTrigger("Dash");

        // รีเซ็ตเวลา
        dashTimer = 0;

        while (dashTimer < DashDuration)
        {
            dashTimer += Time.deltaTime;

            yield return null;
        }

        // Dash จบ
        isDashing = false;

        PlayerManager.Instance.ChangeState(PlayerStats.Idle);

        // รอ Cooldown
        yield return new WaitForSeconds(DashCooldown);

        // Dash ได้อีกครั้ง
        canDash = true;
    }
    
}
