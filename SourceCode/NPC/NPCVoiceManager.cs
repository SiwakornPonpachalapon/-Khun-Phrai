using UnityEngine;

public class NPCVoiceManager : MonoBehaviour
{
    public static NPCVoiceManager Instance;
    [Header("Audio")]
    [SerializeField] private AudioSource voiceSource;

    [Header("Quest Voice")]
    [Tooltip("0 = Quest 1, 1 = Quest 2, 2 = Quest 3 ...")]
    [SerializeField] private AudioClip[] questVoices;

    void Awake()
    {
        Instance = this;
    }
    // เล่นเสียงตาม Quest ปัจจุบัน
    public void PlayCurrentQuestVoice()
    {
        if (QuestManager.Instance == null)
            return;

        int quest = QuestManager.Instance.currentQuest;

        PlayQuestVoice(quest);
    }

    // เล่นเสียง Quest ที่กำหนด
    public void PlayQuestVoice(int questNumber)
    {
        // Quest 1 = Array ช่อง 0
        int index = questNumber - 1;

        if (index < 0 || index >= questVoices.Length)
            return;

        if (questVoices[index] == null)
            return;

        // ถ้ามีเสียงเก่าอยู่ ให้หยุดก่อน
        voiceSource.Stop();

        voiceSource.clip = questVoices[index];
        voiceSource.Play();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
    }
}