using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{

    [Header("Boss HP")]
    public float maxHp = 500f;
    public float currentHp;

    [Header("Death")]
    public float winDelay = 2f;

    [Header("UI Image")]
    public Image Hpbar;

    private bool isDead = false;

    private DamageFlash damageFlash;


    void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
    }

    void Start()
    {
        currentHp = maxHp;
    }
    void Update()
    {
        Hpbar.fillAmount = currentHp / maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHp -= damage;

        if (damageFlash != null)
        {
            damageFlash.Flash();
        }

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        //Debug.Log("BOSS DEAD!");
        
        BossAttackManager.Instance.StopBoss();
        

        // บันทึกว่า Boss ถูกฆ่าแล้ว
        PlayerPrefs.SetInt("QuestBossKilled", 1);

        // Save เกมทั้งหมด
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SaveGame();
        }

        // =========================
        // ANIMATION ตอนตาย
        // =========================

        // ตอนมี Animation ค่อยเปิด
        //
        StartCoroutine(GoToWin());
    }

    IEnumerator GoToWin()
    {
        yield return new WaitForSeconds(winDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene("Win");
    }
}