using UnityEngine;

public class BossVoiceManager : MonoBehaviour
{
    public static BossVoiceManager Instance;
    [Header("Audio")]
    [SerializeField] private AudioSource voiceSource;

    [Header("Attack Voice")]
    [SerializeField] private AudioClip[] attack1;
    [SerializeField] private AudioClip[] attack2;

    [Header("Hit Voice")]
    [SerializeField] private AudioClip[] hitVoices;

    [Header("Dead Voice")]
    [SerializeField] private AudioClip deadVoice;

    void Awake()
    {
        Instance = this;
    }
    public void PlayAttack1()
    {
        if (attack1.Length == 0)
            return;

        AudioClip clip =
            attack1[Random.Range(0, attack1.Length)];

        PlayVoice(clip);
    }

    public void PlayAttack2()
    {
        if (attack2.Length == 0)
            return;

        AudioClip clip =
            attack2[Random.Range(0, attack2.Length)];

        PlayVoice(clip);
    }

    public void PlayHit()
    {
        if (hitVoices.Length == 0)
            return;

        AudioClip clip =
            hitVoices[Random.Range(0, hitVoices.Length)];

        PlayVoice(clip);
    }

    public void PlayDead()
    {
        PlayVoice(deadVoice);
    }


    private void PlayVoice(AudioClip clip)
    {
        if (clip == null)
            return;

        voiceSource.Stop();
        voiceSource.clip = clip;
        voiceSource.Play();
    }
}