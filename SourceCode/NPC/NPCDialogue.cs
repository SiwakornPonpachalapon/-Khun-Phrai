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
                NPCVoiceManager.Instance.PlayQuestVoice(1);
                dialogueText.text =
                    "“อนิจจา... หมู่บ้านของเราถูกพวกทหารโจรเผาทำลายจนแทบไม่เหลือชิ้นดี ชาวบ้านเดือดร้อนกันทั่วหน้า พ่อพราย ท่านผู้มีฝีมือ ได้โปรดช่วยเหลือพวกเราด้วยเถิด!\n" +
                    "ยามนี้เราต้องรีบฟื้นฟูที่พักอาศัยก่อน ข้าอยากรบกวนท่านช่วยไปหา ไม้ 10 ท่อน หิน 10 ก้อน และเหล็ก 10 ก้อน จากชายป่านอกหมู่บ้าน มาให้ข้าทีเถิด";

                QuestManager.Instance.AcceptQuest();
            }

            // ของครบแล้ว
            else if (QuestManager.Instance.IsQuest1Complete())
            {
                NPCVoiceManager.Instance.PlayQuestVoice(2);
                dialogueText.text =
                    "ได้วัตถุดิบมาครบแล้วรึ? ยอดเยี่ยมมาก!";

                QuestManager.Instance.CompleteQuest1();
            }

            // ของยังไม่ครบ
            else
            {
                int wood = PlayerManager.Instance.Wood;
                int stone = PlayerManager.Instance.Stone;
                int iron = PlayerManager.Instance.Iron;
                NPCVoiceManager.Instance.PlayQuestVoice(3);
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
                NPCVoiceManager.Instance.PlayQuestVoice(4);
                dialogueText.text =
                    "ทีนี้นำไม้ หิน และเหล็กไปเก็บไว้ในโกดัง";

                QuestManager.Instance.AcceptQuest();
            }

            // เอาของเข้าโกดังแล้ว
            else if (QuestManager.Instance.IsQuest2Complete())
            {
                NPCVoiceManager.Instance.PlayQuestVoice(5);
                dialogueText.text =
                    "ดีมาก ตอนนี้เรามีทรัพยากรพร้อมแล้ว";

                QuestManager.Instance.CompleteQuest2();
            }

            // รับ Quest แล้ว แต่ยังไม่ได้เก็บของ
            else
            {
                NPCVoiceManager.Instance.PlayQuestVoice(6);
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
                NPCVoiceManager.Instance.PlayQuestVoice(7);
                dialogueText.text =
                    "รบกวนท่านช่วยนำไม้ หินและเหล็กไป ซ่อมแซมเรือนที่พังทลายให้ข้าที";

                QuestManager.Instance.AcceptQuest();
            }

            // อัปเกรดบ้านเสร็จแล้ว
            else if (QuestManager.Instance.IsQuest3Complete())
            {
                NPCVoiceManager.Instance.PlayQuestVoice(8);
                dialogueText.text = "ช้าก่อนท่าน! อย่าเพิ่งเพลินกับการซ่อมบ้าน พวกทหารศัตรูมันยังซุ่มรอกระหน่ำซ้ำเติมเราอยู่นอกกำแพงหมู่บ้าน";

                // ได้รับดาบ
                WeaponManager.Instance.GiveSword();

                // จบ Quest 3 → ไป Quest 4
                QuestManager.Instance.CompleteQuest3();
            }

            // รับ Quest แล้ว แต่บ้านยังไม่เสร็จ
            else
            {
                NPCVoiceManager.Instance.PlayQuestVoice(9);
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
                NPCVoiceManager.Instance.PlayQuestVoice(10);
                dialogueText.text = "สายของเรารายงานว่ามีพวกมันราวๆ 10 นาย คอยดักซุ่มลาดตระเวนอยู่บริเวณชายป่า หากไม่รีบกำจัด พวกมันต้องย้อนกลับมาเผาหมู่บ้านเราซ้ำแน่";

                QuestManager.Instance.AcceptQuest();
            }

            // ฆ่าครบ 10 แล้ว
            else if (QuestManager.Instance.IsQuest4Complete())
            {
                NPCVoiceManager.Instance.PlayQuestVoice(11);
                dialogueText.text = "พ่อพราย ช่วยหลานข้าด้วย! พวกมันจับตัวชาวบ้านกับพวกเด็กๆ ไปกักขังไว้ที่ค่ายใหญ่ของพวกมันในหุบเขา";

                QuestManager.Instance.CompleteQuest4();
            }

            // ยังฆ่าไม่ครบ
            else
            {
                NPCVoiceManager.Instance.PlayQuestVoice(12);
                dialogueText.text =
                    "ยังเหลือพวกมันอีก\n\n" +
                    "กำจัดศัตรู " +
                    QuestManager.Instance.enemyKilled +
                    "/" +
                    QuestManager.Instance.enemyTarget;
            }
        }
        // =========================
        // QUEST 5
        // =========================
        else if (QuestManager.Instance.currentQuest == 5)
        {
            // ยังไม่ได้รับ Quest 5
            if (!QuestManager.Instance.questAccepted)
            {
                NPCVoiceManager.Instance.PlayQuestVoice(13);
                dialogueText.text =
                    "แม่ทัพของพวกมันมีวิชาคมหนังเหนียว ยามนี้มันจับเชลยไว้ในกรงขังท้ายค่าย หากท่านลุยเข้าไปตรงๆ ต้องปะทะกับมันแน่นอน ท่านโปรดระมัดระวังด้วยเถิด";

                QuestManager.Instance.AcceptQuest();
            }

            // รับ Quest 5 แล้ว
            else
            {
                NPCVoiceManager.Instance.PlayQuestVoice(14);
                dialogueText.text =
                    "ไปเถอะท่านพราย\n" +
                    "จัดการมันให้จบ แล้วกลับมาอย่างปลอดภัย";
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

        //Debug.Log("จบบทสนทนา");
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