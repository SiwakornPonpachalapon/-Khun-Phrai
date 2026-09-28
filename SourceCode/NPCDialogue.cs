using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;
    public GameObject dialoguePanel;

    public TMP_Text npcNameText;
    public TMP_Text dialogueText;

    [Header("Dialogue")]
    public string npcName = "ชาวบ้าน";

    [TextArea(3, 5)]
    public string dialogue =
        "ข้ามีเรื่องอยากให้เจ้าช่วยหน่อย...";

    private bool playerInRange = false;
    private bool isTalking = false;

    private void Start()
    {
        // เริ่มเกมให้ UI ปิดก่อน
        interactText.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // ยังไม่อยู่ใกล้ NPC ไม่ต้องทำอะไร
        if (!playerInRange)
            return;

        // กด F
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (!isTalking)
            {
                OpenDialogue();
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    // =========================
    // เปิดบทสนทนา
    // =========================

    private void OpenDialogue()
    {
        isTalking = true;

        interactText.SetActive(false);
        dialoguePanel.SetActive(true);

        npcNameText.text = npcName;

        // =========================
        // QUEST 1
        // =========================
        if (QuestManager.Instance.currentQuest == 1)
        {
            // ยังไม่ได้รับ Quest 1
            if (!QuestManager.Instance.questAccepted)
            {
                dialogueText.text =
                    "ข้ามีเรื่องอยากให้เจ้าช่วยหน่อย\n" +
                    "ช่วยหาไม้ 10 ชิ้น หิน 10 ชิ้น " +
                    "และเหล็ก 10 ชิ้นมาให้ข้า";

                QuestManager.Instance.AcceptQuest();
            }

            // ของครบแล้ว
            else if (QuestManager.Instance.IsQuest1Complete())
            {
                dialogueText.text =
                    "ดีมาก! เจ้านำของมาครบแล้ว";

                QuestManager.Instance.CompleteQuest1();
            }

            // ของยังไม่ครบ
            else
            {
                int wood = PlayerManager.Instance.Wood;
                int stone = PlayerManager.Instance.Stone;
                int iron = PlayerManager.Instance.Iron;

                dialogueText.text =
                    "ของที่ข้าต้องการยังไม่ครบนะ\n\n" +
                    "ไม้ " + wood + "/10\n" +
                    "หิน " + stone + "/10\n" +
                    "เหล็ก " + iron + "/10";
            }
        }

        // =========================
        // QUEST 2
        // =========================
        else if (QuestManager.Instance.currentQuest == 2)
        {
            // ยังไม่ได้รับ Quest 2
            if (!QuestManager.Instance.questAccepted)
            {
                dialogueText.text =
                    "ทีนี้นำไม้ หิน และเหล็กไปเก็บไว้ในโกดัง";

                QuestManager.Instance.AcceptQuest();
            }

            // เอาของเข้าโกดังแล้ว
            else if (QuestManager.Instance.IsQuest2Complete())
            {
                dialogueText.text =
                    "ดีมาก ตอนนี้เรามีทรัพยากรสำรองแล้ว";

                QuestManager.Instance.CompleteQuest2();
            }

            // รับ Quest แล้ว แต่ยังไม่ได้เก็บของ
            else
            {
                dialogueText.text =
                    "นำทรัพยากรที่หามาไปเก็บไว้ในโกดังก่อน";
            }
        }
        // =========================
        // QUEST 3
        // =========================
        else if (QuestManager.Instance.currentQuest == 3)
        {
            // ยังไม่ได้รับ Quest 3
            if (!QuestManager.Instance.questAccepted)
            {
                dialogueText.text =
                    "เอาของในโกดังไปใช้ซ่อมแซมบ้านเสีย";

                QuestManager.Instance.AcceptQuest();
            }

            // อัปเกรดบ้านเสร็จแล้ว
            else if (QuestManager.Instance.IsQuest3Complete())
            {
                dialogueText.text =
                    "ขอบใจมากไอ้หนู\n" +
                    "แล้วนี่...อาจเป็นสิ่งที่เอ็งต้องการ";

                // ได้รับดาบ
                WeaponManager.Instance.GiveSword();

                // จบ Quest 3 → ไป Quest 4
                QuestManager.Instance.CompleteQuest3();
            }

            // รับ Quest แล้ว แต่บ้านยังไม่เสร็จ
            else
            {
                dialogueText.text =
                    "ไปซ่อมแซมบ้านให้เรียบร้อยก่อนเถอะ";
            }
        }
        // =========================
        // QUEST 4
        // =========================
        else if (QuestManager.Instance.currentQuest == 4)
        {
            // ยังไม่ได้รับ Quest
            if (!QuestManager.Instance.questAccepted)
            {
                dialogueText.text =
                    "ดาบนั่นไม่ได้มีไว้ให้ถือเล่นหรอกนะไอ้หนู\n" +
                    "ช่วงนี้มีพวกอันธพาลเพ่นพ่านอยู่แถวนี้\n" +
                    "จัดการพวกมันสัก 10 คนแล้วกลับมาหาข้า";

                QuestManager.Instance.AcceptQuest();
            }

            // ฆ่าครบ 10 แล้ว
            else if (QuestManager.Instance.IsQuest4Complete())
            {
                dialogueText.text =
                    "ดูเหมือนเอ็งจะใช้ดาบเป็นแล้วนี่\n" +
                    "กลับมาได้ครบสามสิบสองก็ดีแล้ว";

                QuestManager.Instance.CompleteQuest4();
            }

            // ยังฆ่าไม่ครบ
            else
            {
                dialogueText.text =
                    "ยังเหลือพวกมันอีก\n\n" +
                    "กำจัดศัตรู " +
                    QuestManager.Instance.enemyKilled +
                    "/" +
                    QuestManager.Instance.enemyTarget;
            }
        }

    }
        

    // =========================
    // ปิดบทสนทนา
    // =========================

    private void CloseDialogue()
    {
        isTalking = false;

        dialoguePanel.SetActive(false);

        // ถ้ายังยืนอยู่ใกล้ NPC
        // ให้ข้อความกด F กลับมา
        if (playerInRange)
        {
            interactText.SetActive(true);
        }

        Debug.Log("จบบทสนทนา");
    }

    // =========================
    // Player เข้าใกล้
    // =========================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isTalking)
            {
                interactText.SetActive(true);
            }
        }
    }

    // =========================
    // Player เดินออก
    // =========================

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            interactText.SetActive(false);

            // ถ้าเดินหนีตอนกำลังคุย
            // ปิด Dialogue ให้อัตโนมัติ
            if (isTalking)
            {
                CloseDialogue();
            }
        }
    }
}
