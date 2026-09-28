using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("Weapon")]
    public GameObject sword;

    public bool HasSword { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // โหลดว่าเคยได้ดาบหรือยัง
        HasSword = PlayerPrefs.GetInt("HasSword", 0) == 1;

        UpdateWeapon();
    }

    // =========================
    // ได้รับดาบ
    // =========================
    public void GiveSword()
    {
        // มีอยู่แล้ว ไม่ต้องให้อีก
        if (HasSword)
            return;

        HasSword = true;

        UpdateWeapon();
        SaveWeapon();

        Debug.Log("ได้รับดาบ!");
    }

    // =========================
    // เปิด / ปิดดาบ
    // =========================
    private void UpdateWeapon()
    {
        sword.SetActive(HasSword);
    }

    // =========================
    // SAVE
    // =========================
    public void SaveWeapon()
    {
        PlayerPrefs.SetInt(
            "HasSword",
            HasSword ? 1 : 0
        );

        PlayerPrefs.Save();
    }
}
