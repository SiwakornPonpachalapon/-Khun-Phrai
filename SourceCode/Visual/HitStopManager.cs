using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    private Coroutine hitStopRoutine;

    void Awake()
    {
        Instance = this;
    }

    public void Play(float duration, float slowScale)
    {
        if (hitStopRoutine != null)
            StopCoroutine(hitStopRoutine);

        hitStopRoutine = StartCoroutine(HitStop(duration, slowScale));
    }

    IEnumerator HitStop(float duration, float slowScale)
    {
        Time.timeScale = slowScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;

        hitStopRoutine = null;
    }
}