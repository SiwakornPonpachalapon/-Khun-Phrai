using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FoodSystem : MonoBehaviour
{
    public static FoodSystem InstancefoodSystem;
    public float maxFood = 100f;
    public float currentFood;
    [Header("UI Image")]
    public Image foodBar;
    public Image HpBar;
    public TMP_Text yummytext;

    void Awake()
    {
        InstancefoodSystem = this;
    }
    void Start()
    {
        currentFood = PlayerPrefs.GetFloat("currentFood",maxFood);
        StartCoroutine(ReduceHpEverySecond());
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentFood += 20f;
            StartCoroutine(Yummy());
        }
        currentFood -= Time.deltaTime * (1f / 60f);// หารเท่าไรคือลด เช่น 60 1 นาทีลด 1 หน่วย
        HpBar.fillAmount = PlayerManager.Instance.Hp / PlayerManager.Instance.MaxHp;
        foodBar.fillAmount = currentFood / maxFood;
    }
    IEnumerator ReduceHpEverySecond()
    {
        while(true)
        {
            if(currentFood <= 0)
            {
                PlayerManager.Instance.SetHp(-1);
            }

            yield return new WaitForSeconds(1f); // รอ 1 วินาที
        }
    }
    public void savefood()
    {
        PlayerPrefs.SetFloat("currentFood",currentFood);
    }
    IEnumerator Yummy()
    {    
        yummytext.text = "Yummy";
        yield return new WaitForSeconds(3f);
        yummytext.text = "";
    }
}
