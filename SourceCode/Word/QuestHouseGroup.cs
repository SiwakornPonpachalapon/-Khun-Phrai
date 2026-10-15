using Unity.VisualScripting;
using UnityEngine;

public class QuestHouseGroup : MonoBehaviour
{
    [Header("บ้านก่อนอัปเกรด")]
    public GameObject[] houseStage1;

    [Header("บ้านหลังอัปเกรด")]
    public GameObject[] houseStage2;
    public static QuestHouseGroup Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UpdateHouses();
    }

    public void UpdateHouses()
    {
        if (QuestManager.Instance == null)
            return;

        int quest = QuestManager.Instance.currentQuest;
        bool accepted = QuestManager.Instance.questAccepted;
        bool upgraded = QuestManager.Instance.houseUpgraded;

        // เริ่มต้นซ่อนทั้งหมด
        SetGroup(houseStage1, false);
        SetGroup(houseStage2, false);

        // -------------------------
        // ยังมาไม่ถึง Quest 3
        // -------------------------
        if (quest < 3)
            return;

        // -------------------------
        // ถึง Quest 3 แต่ยังไม่รับ
        // -------------------------
        if (quest == 3 && !accepted)
            return;

        // -------------------------
        // รับ Quest 3 แล้ว
        // แต่ยังไม่ Upgrade
        // -------------------------
        if (quest == 3 && accepted && !upgraded)
        {
            SetGroup(houseStage1, true);
            return;
        }

        // -------------------------
        // Upgrade แล้ว
        // -------------------------
        if (upgraded || quest > 3)
        {
            SetGroup(houseStage2, true);
        }
    }

    void SetGroup(GameObject[] group, bool active)
    {
        foreach (GameObject obj in group)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}