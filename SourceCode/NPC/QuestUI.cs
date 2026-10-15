using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    [Header("UI")]
    public TMP_Text questText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        if (QuestManager.Instance == null)
            return;

        int quest = QuestManager.Instance.currentQuest;
        bool accepted = QuestManager.Instance.questAccepted;


        // =================================================
        // QUEST 1 - เก็บทรัพยากร
        // =================================================

        if (quest == 1)
        {
            if (!accepted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ไปคุยกับ NPC";
            }
            else if (!QuestManager.Instance.IsQuest1Complete())
            {
                questText.text =
                    "รวบรวมทรัพยากร\n" +
                    "ไม้ " +
                    Mathf.Min(PlayerManager.Instance.Wood, 10) +
                    " / 10\n" +

                    "หิน " +
                    Mathf.Min(PlayerManager.Instance.Stone, 10) +
                    " / 10\n" +

                    "เหล็ก " +
                    Mathf.Min(PlayerManager.Instance.Iron, 10) +
                    " / 10";
            }
            else
            {
                questText.text =
                    "ภารกิจ\n" +
                    "กลับไปส่งภารกิจกับ NPC";
            }

            return;
        }


        // =================================================
        // QUEST 2 - STORAGE
        // =================================================

        if (quest == 2)
        {
            if (!accepted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ไปคุยกับ NPC";
            }
            else if (!QuestManager.Instance.storageCompleted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "นำทรัพยากรไปเก็บไว้ในโกดัง";
            }
            else
            {
                questText.text =
                    "ภารกิจ\n" +
                    "กลับไปส่งภารกิจกับ NPC";
            }

            return;
        }


        // =================================================
        // QUEST 3 - UPGRADE HOUSE
        // =================================================

        if (quest == 3)
        {
            if (!accepted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ไปคุยกับ NPC";
            }
            else if (!QuestManager.Instance.houseUpgraded)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ซ่อมแซมเรือนที่พังทลาย";
            }
            else
            {
                questText.text =
                    "ภารกิจ\n" +
                    "กลับไปส่งภารกิจกับ NPC";
            }

            return;
        }


        // =================================================
        // QUEST 4 - ฆ่าศัตรู 10 ตัว
        // =================================================

        if (quest == 4)
        {
            if (!accepted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ไปคุยกับ NPC";
            }
            else if (!QuestManager.Instance.IsQuest4Complete())
            {
                questText.text =
                    "กำจัดพวกโจรที่ลาดตระเวนทั้ง\n" +
                    Mathf.Min(
                        QuestManager.Instance.enemyKilled,
                        10
                    ) +
                    " / 10 นาย";
            }
            else
            {
                questText.text =
                    "ภารกิจ\n" +
                    "กลับไปส่งภารกิจกับ NPC";
            }

            return;
        }


        // =================================================
        // QUEST 5 - BOSS
        // =================================================

        if (quest == 5)
        {
            if (!accepted)
            {
                questText.text =
                    "ภารกิจ\n" +
                    "ไปคุยกับ NPC";
            }
            else
            {
                questText.text =
                    "ภารกิจสุดท้าย\n" +
                    "กำจัดแม่ทัพและช่วยเหลือเชลย";
            }

            return;
        }


        // =================================================
        // ไม่มี Quest
        // =================================================

        questText.text = "";
    }
}