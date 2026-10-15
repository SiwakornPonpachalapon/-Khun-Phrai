using UnityEngine;

public class QuestEnemyGroup : MonoBehaviour
{
    [Header("Enemy Group")]
    public GameObject enemyGroup;

    void Start()
    {
        UpdateEnemyGroup();
    }

    public void UpdateEnemyGroup()
    {
        if (QuestManager.Instance == null)
        {
            enemyGroup.SetActive(false);
            return;
        }

        // ต้องอยู่ Quest 4 + รับ Quest แล้ว + ยังฆ่าไม่ครบ
        bool shouldShow =
            QuestManager.Instance.currentQuest == 4 &&
            QuestManager.Instance.questAccepted &&
            !QuestManager.Instance.IsQuest4Complete();

        enemyGroup.SetActive(shouldShow);
    }
}