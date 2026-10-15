using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instancec;
    private int BoxWood;
    private int wood;
    private int BoxStone;
    private int stone;
    private int BoxIron;
    private int iron;
    private bool PlayerEnter = false;
    public TMP_Text BoxText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instancec = this;
    }
    void Start()
    {
        BoxWood = PlayerPrefs.GetInt("BoxWood",0);
        BoxStone = PlayerPrefs.GetInt("BoxStone",0);
        BoxIron = PlayerPrefs.GetInt("BoxIron",0);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerEnter == true && Keyboard.current.fKey.wasPressedThisFrame && QuestManager.Instance.currentQuest >= 2)
        {
            //Wood
            wood = PlayerManager.Instance.Wood;
            PlayerManager.Instance.AddLogWood(-wood);
            BoxWood += wood;
            //Stone
            stone = PlayerManager.Instance.Stone;
            BoxStone += stone;
            PlayerManager.Instance.AddStone(-stone);
            //Iron
            iron = PlayerManager.Instance.Iron;
            BoxIron += iron;
            PlayerManager.Instance.AddIron(-iron);
            //Quest 2
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.StorageCompleted();
            }
        }
        QuestHouseGroup.Instance.UpdateHouses();
        //แสดงค่า
        BoxText.text = $"Wood : {BoxWood:00} Stone : {BoxStone:00} Iron : {BoxIron:00}";
        
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
    // ตรวจว่าของในกล่องพอหรือไม่
    public bool HasResources(int wood, int stone, int iron)
    {
        return BoxWood >= wood &&
            BoxStone >= stone &&
            BoxIron >= iron;
    }


    // ใช้ของจากกล่อง
    public bool UseResources(int wood, int stone, int iron)
    {
        // ของไม่พอ
        if (!HasResources(wood, stone, iron))
        {
            //Debug.Log("ทรัพยากรในโกดังไม่พอ");
            return false;
        }

        // หักของ
        BoxWood -= wood;
        BoxStone -= stone;
        BoxIron -= iron;

        //Debug.Log($"ใช้ทรัพยากร Wood {wood} / Stone {stone} / Iron {iron}");

        return true;
    }
    public void SaveBox()
    {
        PlayerPrefs.SetInt("BoxWood",BoxWood);
        PlayerPrefs.SetInt("BoxStone",BoxStone);
        PlayerPrefs.SetInt("BoxIron",BoxIron);
    }
}
