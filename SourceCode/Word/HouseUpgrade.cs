using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class HouseUpgrade : MonoBehaviour
{
    public static HouseUpgrade Instantiateb;
    public GameObject[] houselevel;
    public int currentlevel = 0;

    public ParticleSystem smokeEffect;

    public Camera playerCamera;
    public Camera upgradeCamera;

    public Transform houseCenter;
    [Header("Upgrade Cost")]
    public int woodCost = 10;
    public int stoneCost = 10;
    public int ironCost = 10;
    private bool PlayerEnter;
    void Awake()
    {
        Instantiateb = this;
    }

    void Start()
    {
        currentlevel = PlayerPrefs.GetInt("LvHouse",currentlevel);
        UpdateHouse();

        upgradeCamera.gameObject.SetActive(false);
    }
    void Update()
    {
        if (PlayerEnter && Keyboard.current.fKey.wasPressedThisFrame && QuestManager.Instance.currentQuest >= 3)
        {
            QuestHouseGroup.Instance.UpdateHouses();
            UpgradeHouse();
            
        }
    }

    public void UpgradeHouse()
    {
        // บ้านเต็ม Level แล้ว
        if (currentlevel >= houselevel.Length - 1)
        {
            //Debug.Log("บ้าน Level สูงสุดแล้ว");
            return;
        }

        // ตรวจของในโกดัง
        if (!BoxManager.Instancec.HasResources(
            woodCost,
            stoneCost,
            ironCost))
        {
            /*Debug.Log(
                $"ของในโกดังไม่พอ! ต้องการ " +
                $"Wood {woodCost} / " +
                $"Stone {stoneCost} / " +
                $"Iron {ironCost}"
            );*/

            return;
        }

        // หักของ
        BoxManager.Instancec.UseResources(
            woodCost,
            stoneCost,
            ironCost
        );
        // เล่น Animation Upgrade
        StartCoroutine(UpgradeAnimation());
    }

    IEnumerator UpgradeAnimation()
    {
        QuestHouseGroup.Instance.UpdateHouses();
        // ปิดกล้องผู้เล่น
        playerCamera.gameObject.SetActive(false);

        // เปิดกล้อง cinematic
        upgradeCamera.gameObject.SetActive(true);

        // เปิดควัน
        smokeEffect.Play();

        // หมุนกล้องรอบบ้าน
        yield return StartCoroutine(RotateCamera());

        // เปลี่ยนบ้าน
        currentlevel++;
        UpdateHouse();

        // แจ้ง QuestManager ว่าบ้านเสร็จแล้ว
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.HouseUpgraded();
        }

        // รออีกนิด
        yield return new WaitForSeconds(1f);

        // ปิดควัน
        smokeEffect.Stop();

        // กลับกล้องผู้เล่น
        upgradeCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        QuestNavigationTrail.Instance.UpdateNavigation();
    }

    IEnumerator RotateCamera()
    {
        float duration = 2f;
        float timer = 0;

        Vector3[] angles =
        {
            new Vector3(0, 5, -15),   // หน้า
            new Vector3(15, 5, 0),    // ซ้าย
            new Vector3(0, 5, 15),    // หลัง
            new Vector3(-15, 5, 0),    // ขวา
            new Vector3(0, 5, -15)   // หน้า
        };

        foreach (Vector3 offset in angles)
        {
            Vector3 startPos = upgradeCamera.transform.position;
            Vector3 targetPos = houseCenter.position + offset;

            timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                upgradeCamera.transform.position =
                    Vector3.Lerp(startPos, targetPos, timer / duration);

                upgradeCamera.transform.LookAt(houseCenter);

                yield return null;
            }
        }
    }

    void UpdateHouse()
    {
        for (int i = 0; i < houselevel.Length; i++)
        {
            houselevel[i].SetActive(i == currentlevel);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerEnter = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerEnter = false;
        }
    }
    public void SaveHouse()
    {
        PlayerPrefs.SetInt("LvHouse",currentlevel);
    }
}
