using System.Collections;
using UnityEngine;

public class GroundWaveEffect : MonoBehaviour
{
    [Header("Effect")]
    public float riseHeight = 2f;

    public float riseTime = 0.15f;
    public float fallTime = 0.35f;

    private Vector3 startPosition;
    private Vector3 topPosition;


    void Start()
    {
        startPosition = transform.position;

        // เริ่มอยู่ใต้พื้นนิดหนึ่ง
        startPosition.y -= riseHeight;

        topPosition = transform.position;

        transform.position = startPosition;

        StartCoroutine(EffectRoutine());
    }


    IEnumerator EffectRoutine()
    {
        // =========================
        // พุ่งขึ้น
        // =========================

        float timer = 0f;

        while (timer < riseTime)
        {
            timer += Time.deltaTime;

            float t = timer / riseTime;

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    topPosition,
                    t
                );

            yield return null;
        }


        // =========================
        // ลดกลับลง
        // =========================

        timer = 0f;

        while (timer < fallTime)
        {
            timer += Time.deltaTime;

            float t = timer / fallTime;

            transform.position =
                Vector3.Lerp(
                    topPosition,
                    startPosition,
                    t
                );

            yield return null;
        }


        // =========================
        // หาย
        // =========================

        Destroy(gameObject);
    }
}