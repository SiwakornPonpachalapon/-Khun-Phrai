using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest")]
    public int currentQuest = 1;

    [Header("Quest State")]
    public bool questAccepted = false;

    [Header("Quest 2")]
    public bool storageCompleted = false;

    [Header("Quest 3")]
    public bool houseUpgraded = false;

    [Header("Quest 4")]
    public int enemyKilled = 0;
    public int enemyTarget = 10;

    private void Awake()
    {
        Instance = this;

        LoadQuest();
    }

    // =========================
    // รับ Quest
    // =========================

    public void AcceptQuest()
    {
        if (questAccepted)
            return;

        questAccepted = true;

        Debug.Log("รับ Quest " + currentQuest);

        SaveQuest();
    }

    // =========================
    // ตรวจ Quest 1
    // =========================

    public bool IsQuest1Complete()
    {
        if (currentQuest != 1 || !questAccepted)
            return false;

        int wood = PlayerManager.Instance.Wood;
        int stone = PlayerManager.Instance.Stone;
        int iron = PlayerManager.Instance.Iron;

        return wood >= 10 &&
               stone >= 10 &&
               iron >= 10;
    }

    // =========================
    // ส่ง Quest 1
    // =========================

    public void CompleteQuest1()
    {
        if (!IsQuest1Complete())
            return;

        Debug.Log("Quest 1 Complete!");

        currentQuest = 2;
        questAccepted = false;

        SaveQuest();
    }
    public void StorageCompleted()
    {
        // ต้องอยู่ Quest 2 และรับ Quest แล้วเท่านั้น
        if (currentQuest != 2 || !questAccepted)
            return;

        storageCompleted = true;

        Debug.Log("Quest 2 : เก็บของเข้าโกดังเรียบร้อย");

        SaveQuest();
    }
    public bool IsQuest2Complete()
    {
        return currentQuest == 2 &&
            questAccepted &&
            storageCompleted;
    }
    public void CompleteQuest2()
    {
        if (!IsQuest2Complete())
            return;

        Debug.Log("Quest 2 Complete!");

        currentQuest = 3;
        questAccepted = false;

        SaveQuest();
    }
    public void HouseUpgraded()
    {
        // ต้องอยู่ Quest 3 และรับ Quest แล้ว
        if (currentQuest != 3 || !questAccepted)
            return;

        houseUpgraded = true;

        Debug.Log("Quest 3 : อัปเกรดบ้านเรียบร้อย");

        SaveQuest();
    }
    public bool IsQuest3Complete()
    {
        return currentQuest == 3 &&
            questAccepted &&
            houseUpgraded;
    }
    public void CompleteQuest3()
    {
        if (!IsQuest3Complete())
            return;

        Debug.Log("Quest 3 Complete! ได้รับดาบ");

        currentQuest = 4;
        questAccepted = false;

        SaveQuest();
    }
    public void EnemyKilled()
    {
        // ต้องอยู่ Quest 4 และรับ Quest แล้ว
        if (currentQuest != 4 || !questAccepted)
            return;

        // ครบแล้วไม่ต้องนับเพิ่ม
        if (enemyKilled >= enemyTarget)
            return;

        enemyKilled++;

        Debug.Log(
            $"Quest 4 : กำจัดศัตรู {enemyKilled}/{enemyTarget}"
        );

        SaveQuest();
    }
    public bool IsQuest4Complete()
    {
        return currentQuest == 4 &&
            questAccepted &&
            enemyKilled >= enemyTarget;
    }
    public void CompleteQuest4()
    {
        if (!IsQuest4Complete())
            return;

        Debug.Log("Quest 4 Complete!");

        currentQuest = 5;
        questAccepted = false;

        SaveQuest();
    }

    // =========================
    // Save
    // =========================

    public void SaveQuest()
    {
        PlayerPrefs.SetInt("QuestLevel", currentQuest);

        PlayerPrefs.SetInt("QuestAccepted",questAccepted ? 1 : 0);

        PlayerPrefs.SetInt("QuestStorageCompleted",storageCompleted ? 1 : 0);

        PlayerPrefs.SetInt("QuestHouseUpgraded",houseUpgraded ? 1 : 0);

        PlayerPrefs.SetInt("QuestEnemyKilled", enemyKilled);

        PlayerPrefs.Save();
    }

    // =========================
    // Load
    // =========================

    public void LoadQuest()
    {
        currentQuest =PlayerPrefs.GetInt("QuestLevel", 1);

        questAccepted =PlayerPrefs.GetInt("QuestAccepted", 0) == 1;

        storageCompleted =PlayerPrefs.GetInt("QuestStorageCompleted", 0) == 1;

        houseUpgraded =PlayerPrefs.GetInt("QuestHouseUpgraded", 0) == 1;

        enemyKilled =PlayerPrefs.GetInt("QuestEnemyKilled", 0);
    }
}
