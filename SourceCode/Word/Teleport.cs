using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    private bool playerStay;
    [Header("Scene")]
    public string Scene;

    [Header("Quest Requirement")]
    public bool requireQuest = false;

    [Tooltip("ต้องผ่านถึง Quest ไหนจึงจะใช้วาปนี้ได้")]
    public int requiredQuest = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(playerStay == true && Keyboard.current.fKey.wasPressedThisFrame)
        {
            //Debug.Log($"กดวาปจาก GameObject: {gameObject.name}");

            TryTeleport();
        }
    }  
    void TryTeleport()
    {
        // =========================
        // CHECK QUEST
        // =========================

        if (requireQuest)
        {
            int currentQuest =
                QuestManager.Instance.currentQuest;

            bool accepted =
                QuestManager.Instance.questAccepted;

            /*Debug.Log(
                $"Teleport Check | " +
                $"Current Quest = {currentQuest} | " +
                $"Required Quest = {requiredQuest} | " +
                $"Accepted = {accepted}"
            );*/

            // ยังไม่ถึง Quest ที่กำหนด
            if (currentQuest < requiredQuest)
            {
                /*Debug.Log(
                    $"ยังไปไม่ได้ ต้องถึง Quest {requiredQuest} ก่อน"
                );*/

                return;
            }

            // ถึง Quest แล้ว แต่ยังไม่ได้รับ Quest
            if (!accepted)
            {
                /*Debug.Log(
                    $"ยังไปไม่ได้ ต้องรับ Quest {requiredQuest} ก่อน"
                );*/

                return;
            }
        }

        // =========================
        // SAVE
        // =========================

        PlayerManager.Instance.SaveGame();

        // =========================
        // TELEPORT
        // =========================

        SceneManager.LoadScene(Scene);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerStay = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            playerStay = false;
        } 
    }
}
