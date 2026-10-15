using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource voiceSource;

    [Header("Swing")]
    [SerializeField] private AudioClip[] swingSounds;

    [Header("Hit")]
    [SerializeField] private AudioClip[] hitSounds;

    [Header("Aim")]
    [SerializeField] private AudioClip[] aimSounds;

    [Header("Shoot")]
    [SerializeField] private AudioClip firstShootVoice;

    [Header("Dash")]
    [SerializeField] private AudioClip dashSound;

    [Header("Minning")]
    [SerializeField] private AudioClip[] minningSounds;

    [Header("Dead")]
    [SerializeField] private AudioClip[] deadSounds;

    [Header("CutTree")]
    [SerializeField] private AudioClip[] CutSounds;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicSounds;

    
    


    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        PlaySceneMusic();
    }


    // =====================================================
    // MUSIC
    // =====================================================

    void PlaySceneMusic()
    {
        string scene = SceneManager.GetActiveScene().name;
        //Debug.Log(scene);

        if (scene == "Scene1")
        {
            PlayMusic(0);
        }
        else if (scene == "Scene2")
        {
            PlayMusic(1);
        }
        else if (scene == "Scene3")
        {
            PlayMusic(2);
        }
    }


    void PlayMusic(int index)
    {
        if (index < 0 || index >= musicSounds.Length)
            return;

        musicSource.clip = musicSounds[index];

        // ทำให้เพลงวน
        musicSource.loop = true;

        musicSource.Play();
    }


    // =====================================================
    // SWING
    // =====================================================

    public void PlaySwing()
    {
        if (swingSounds.Length == 0)
            return;

        AudioClip clip =
            swingSounds[Random.Range(0, swingSounds.Length)];

        sfxSource.PlayOneShot(clip);
    }


    // =====================================================
    // HIT
    // =====================================================

    public void PlayHit()
    {
        if (hitSounds.Length == 0)
            return;

        AudioClip clip =
            hitSounds[Random.Range(0, hitSounds.Length)];

        sfxSource.PlayOneShot(clip);
    }


    // =====================================================
    // AIM
    // =====================================================

    public void PlayAim()
    {
        if (aimSounds.Length == 0)
            return;

        AudioClip clip =
            aimSounds[Random.Range(0, aimSounds.Length)];

        sfxSource.PlayOneShot(clip);
    }


    // =====================================================
    // DASH
    // =====================================================

    public void PlayDash()
    {
        if (dashSound == null)
            return;

        sfxSource.PlayOneShot(dashSound);
    }
    public void PlayCutTree()
    {
        if (CutSounds.Length == 0)
            return;

        AudioClip clip =
            CutSounds[Random.Range(0, CutSounds.Length)];

        sfxSource.PlayOneShot(clip);
    }
    public void PlayMining()
    {
        if (minningSounds.Length == 0)
            return;

        AudioClip clip =
            minningSounds[Random.Range(0, minningSounds.Length)];

        sfxSource.PlayOneShot(clip);
    }
    public IEnumerator PlayFirstShootVoice()
    {
        if (firstShootVoice == null)
            yield break;

        voiceSource.clip = firstShootVoice;
        voiceSource.Play();

        // รอจนเสียงพูดจบ
        yield return new WaitWhile(() => voiceSource.isPlaying);
    }
}