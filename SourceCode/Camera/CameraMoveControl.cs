using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveControl : MonoBehaviour
{
    [Header("Target")]
    public Transform target;      // Player

    [Header("Camera Settings")]
    public float mouseSensitivity = 3f;
    public float distance = 4f;
    public float height = 1.5f;

    [Header("Camera Rotation")]
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch;

    void Start()
    {
        // ล็อกเมาส์ไว้กลางจอตอนเล่นเกม
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ให้กล้องเริ่มจาก Rotation ปัจจุบัน
        Vector3 startRotation = transform.eulerAngles;

        yaw = startRotation.y;
        pitch = startRotation.x;

        // แก้กรณีมุม X เป็น 0 - 360
        if (pitch > 180f)
        {
            pitch -= 360f;
        }
    }

    void LateUpdate()
    {
        // =========================
        // PAUSE
        // =========================

        // ถ้าเกมหยุดอยู่
        // ไม่รับ Mouse และไม่ขยับกล้อง
        if (Time.timeScale == 0f)
            return;


        // =========================
        // MOUSE INPUT
        // =========================

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;


        // =========================
        // LIMIT CAMERA
        // =========================

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );


        // =========================
        // ROTATION
        // =========================

        transform.rotation =
            Quaternion.Euler(pitch, yaw, 0f);


        // =========================
        // FOLLOW PLAYER
        // =========================

        Vector3 camPos =
            target.position
            - transform.forward * distance
            + Vector3.up * height;

        transform.position = camPos;
    }
}