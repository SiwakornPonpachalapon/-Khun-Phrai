using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Stats")]
    public float Hp = 100f;
    public float MaxHp = 100f;
    public static PlayerManager Instance;
    [Header("Game State")]
    public PlayerStats CurrentState { get; private set; }
    public event Action<PlayerStats> OnStateChanged;

    [Header("Ammo")]
    public int Ammo { get; private set; }

    [Header("Inventory")]
    public int Wood { get; private set; }
    public int Stone { get; private set; }
    public int Iron { get; private set; }
    [Header("UI References")]
    public TMP_Text LogWoodText;
    public TMP_Text StoneText;
    public TMP_Text IronText;
    public TMP_Text AmmoText;
    public TMP_Text ItemNotitext;
    private Coroutine itemNotifyCoroutine;
    public bool IsAiming { get; private set; }  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        Instance = this;
    }
    void Start()
    {
        ChangeState(PlayerStats.Idle);
        Hp = PlayerPrefs.GetFloat("PlayerHp", 100f);
        Wood = PlayerPrefs.GetInt("Wood",0);
        Stone = PlayerPrefs.GetInt("Stone",0);
        Iron = PlayerPrefs.GetInt("Iron",0);
        Ammo = PlayerPrefs.GetInt("Ammo", 0);
        UpdateInventory();
    }
    

    public void ChangeState(PlayerStats newState)
    {
        
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
    public bool CanAttack()
    {
        return CurrentState != PlayerStats.Dash;
    }

    public bool CanDash()
    {
        return true;
    }
    public void AddLogWood(int value)
    {
        ShowItemNotify($"Wood +{value}");
        Wood += value;
        UpdateInventory();
        AudioManager.Instance.PlayCutTree();
    }
    public void AddStone(int value)
    {
        ShowItemNotify($"Stone +{value}");
        Stone += value;
        UpdateInventory();
        AudioManager.Instance.PlayMining();
    }
    public void AddIron(int value)
    {
        ShowItemNotify($"Iron +{value}");;
        Iron += value;
        UpdateInventory();
        AudioManager.Instance.PlayMining();
    }
    public void SetHp(float value)
    {
        Hp += value;
        if (Hp <= 0)
        {
            Wood = 0;
            Iron = 0;
            Stone = 0;
            Ammo = 0;
            SaveGame();
            SceneManager.LoadScene("Loss");
        }
    }
    public void UpdateInventory()
    {
        LogWoodText.text = $"Wood {Wood}";
        StoneText.text = $"Stone {Stone}";
        IronText.text = $"Iron {Iron}";
        AmmoText.text = $"Ammo {Ammo}";
    }
    void SavePlayer()
    {
        PlayerPrefs.SetFloat("PlayerHp", Hp);
        PlayerPrefs.SetInt("Wood",Wood);
        PlayerPrefs.SetInt("Stone",Stone);
        PlayerPrefs.SetInt("Iron",Iron);
        PlayerPrefs.SetInt("Ammo", Ammo);
    }
    public void SaveGame()
    {
        // =========================
        // PLAYER
        // =========================
        SavePlayer();


        // =========================
        // QUEST
        // =========================
        if (QuestManager.Instance != null)
            QuestManager.Instance.SaveQuest();


        // =========================
        // HOUSE
        // =========================
        if (HouseUpgrade.Instantiateb != null)
            HouseUpgrade.Instantiateb.SaveHouse();


        // =========================
        // DAY / NIGHT
        // =========================
        if (DayNight.Instantiatea != null)
            DayNight.Instantiatea.Savetime();


        // =========================
        // STORAGE
        // =========================
        if (BoxManager.Instancec != null)
            BoxManager.Instancec.SaveBox();


        // =========================
        // FOOD
        // =========================
        if (FoodSystem.InstancefoodSystem != null)
            FoodSystem.InstancefoodSystem.savefood();


        // =========================
        // WEAPON
        // =========================
        if (WeaponManager.Instance != null)
            WeaponManager.Instance.SaveWeapon();

        // เขียน PlayerPrefs ลง Disk
        PlayerPrefs.Save();

        //Debug.Log("Save Game Complete!");
    }
    public void AddAmmo(int value)
    {
        Ammo += value;

        // ไม่ให้ติดลบ
        Ammo = Mathf.Max(Ammo, 0);

        //Debug.Log($"Ammo : {Ammo}");
    }
    void ShowItemNotify(string text)
    {
        if (itemNotifyCoroutine != null)
        {
            StopCoroutine(itemNotifyCoroutine);
        }

        itemNotifyCoroutine = StartCoroutine(Itemnotitime(text));
    }
    IEnumerator Itemnotitime(string notitextw)
    {
        ItemNotitext.text = notitextw;
        yield return new WaitForSeconds(3f);
        ItemNotitext.text = "";
        itemNotifyCoroutine = null;
        
    }
    public void SetAim(bool value)
    {
        IsAiming = value;
    }
}
