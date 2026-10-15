using System.Collections;
using UnityEngine;

public class RockImpactEffect : MonoBehaviour
{
    [Header("Scale")]
    public float startScale = 0.2f;
    public float maxScale = 4f;
    public float expandTime = 0.25f;

    [Header("Fade")]
    public float fadeTime = 0.5f;

    private Renderer rend;
    private Material material;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            // สร้าง Material Instance ของ Effect ตัวนี้
            material = rend.material;
        }

        transform.localScale = Vector3.one * startScale;

        StartCoroutine(ImpactRoutine());
    }

    IEnumerator ImpactRoutine()
    {
        // =========================
        // ขยาย
        // =========================

        float timer = 0f;

        while (timer < expandTime)
        {
            timer += Time.deltaTime;

            float t = timer / expandTime;

            float scale = Mathf.Lerp(
                startScale,
                maxScale,
                t
            );

            transform.localScale =
                Vector3.one * scale;

            yield return null;
        }


        // =========================
        // จาง
        // =========================

        timer = 0f;

        if (material != null)
        {
            Color startColor = material.color;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;

                float t = timer / fadeTime;

                Color color = startColor;

                color.a = Mathf.Lerp(
                    startColor.a,
                    0f,
                    t
                );

                material.color = color;

                yield return null;
            }
        }
        else
        {
            // ถ้าไม่มี Renderer ก็รอตามเวลา
            yield return new WaitForSeconds(fadeTime);
        }


        // =========================
        // หาย
        // =========================

        Destroy(gameObject);
    }
}