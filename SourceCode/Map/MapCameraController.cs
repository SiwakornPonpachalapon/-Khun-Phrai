using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraController : MonoBehaviour
{
    public Camera mapCamera;

    public float moveSpeed = 0.1f;
    public float zoomSpeed = 5f;

    public float minZoom = 10f;
    public float maxZoom = 50f;

    private Vector2 lastMousePosition;
    private bool isDragging;

    void Update()
    {
        if (Mouse.current == null) return;

        // เริ่มลาก
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        // หยุดลาก
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        // ระหว่างลาก
        if (isDragging)
        {
            Vector2 currentMousePos = Mouse.current.position.ReadValue();
            Vector2 delta = currentMousePos - lastMousePosition;

            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * moveSpeed;
            mapCamera.transform.position += move;

            lastMousePosition = currentMousePos;
        }

        // Scroll Zoom
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll != 0)
        {
            float newSize = mapCamera.orthographicSize - scroll * zoomSpeed * Time.deltaTime;
            mapCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
}