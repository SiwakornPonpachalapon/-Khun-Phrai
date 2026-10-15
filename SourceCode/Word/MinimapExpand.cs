using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapExpand : MonoBehaviour
{
    [Header("Map UI")]
    public RectTransform mapRect;

    [Header("Mini Map")]
    public Vector2 miniSize = new Vector2(300f, 300f);
    public Vector2 miniPosition = new Vector2(-10f, -10f);

    [Header("Full Map")]
    public Vector2 fullSize = new Vector2(1000f, 800f);
    public Vector2 fullPosition = Vector2.zero;

    [Header("Map Camera")]
    public Camera minimapCamera;

    public float miniMapSize = 10f;
    public float fullMapSize = 30f;

    private bool isFullMap = false;

    void Start()
    {
        SetMiniMap();
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            isFullMap = !isFullMap;

            if (isFullMap)
                SetFullMap();
            else
                SetMiniMap();
        }
    }

    void SetMiniMap()
    {
        // ขวาบน
        mapRect.anchorMin = new Vector2(1f, 1f);
        mapRect.anchorMax = new Vector2(1f, 1f);
        mapRect.pivot = new Vector2(1f, 1f);

        // 300 x 300
        mapRect.sizeDelta = miniSize;

        // -10, -10
        mapRect.anchoredPosition = miniPosition;

        // Camera Zoom
        minimapCamera.orthographicSize = miniMapSize;

    }

    void SetFullMap()
    {
        // กลางจอ
        mapRect.anchorMin = new Vector2(0.5f, 0.5f);
        mapRect.anchorMax = new Vector2(0.5f, 0.5f);
        mapRect.pivot = new Vector2(0.5f, 0.5f);

        // Full Map Size
        mapRect.sizeDelta = fullSize;

        // กลาง
        mapRect.anchoredPosition = fullPosition;

        // Camera Zoom
        minimapCamera.orthographicSize = fullMapSize;
    }
}