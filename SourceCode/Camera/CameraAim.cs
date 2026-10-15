using UnityEngine;

public class CameraAim : MonoBehaviour
{
    public Vector3 normalPosition;

    public Vector3 aimPosition;

    public float moveSpeed = 8f;

    public float normalFOV = 60f;

    public float aimFOV = 45f;

    public float zoomSpeed = 8f;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void Start()
    {
        normalPosition = transform.localPosition;
    }
    void Update()
    {
        if(PlayerManager.Instance.IsAiming)
        {
            transform.localPosition =
                Vector3.Lerp(
                    transform.localPosition,
                    aimPosition,
                    moveSpeed * Time.deltaTime);

            cam.fieldOfView =
                Mathf.Lerp(
                    cam.fieldOfView,
                    aimFOV,
                    zoomSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition =
                Vector3.Lerp(
                    transform.localPosition,
                    normalPosition,
                    moveSpeed * Time.deltaTime);

            cam.fieldOfView =
                Mathf.Lerp(
                    cam.fieldOfView,
                    normalFOV,
                    zoomSpeed * Time.deltaTime);
        }
    }
}