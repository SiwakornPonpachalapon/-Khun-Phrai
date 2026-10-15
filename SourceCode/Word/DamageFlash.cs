using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Flash")]
    public float flashTime = 0.1f;

    private Renderer[] renderers;
    private Color[] originalColors;

    private Coroutine flashCoroutine;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] =
                renderers[i].material.color;
        }
    }

    public void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine =
            StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // ขาว
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color =
                Color.white;
        }

        yield return new WaitForSeconds(flashTime);

        // กลับสีเดิม
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color =
                originalColors[i];
        }

        flashCoroutine = null;
    }
}