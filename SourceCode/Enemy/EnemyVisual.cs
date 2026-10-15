using UnityEngine;
using System.Collections;

public class EnemyVisual : MonoBehaviour
{
    [Header("Flash")]
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float flashTime = 0.08f;

    private Renderer[] renderers;
    private Color[] originalColors;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void PlayHitFlash()
    {
        StopAllCoroutines();
        StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        // เปลี่ยนเป็นสีขาว
        foreach (Renderer r in renderers)
        {
            r.material.color = hitColor;
        }

        yield return new WaitForSeconds(flashTime);

        // กลับเป็นสีเดิม
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}