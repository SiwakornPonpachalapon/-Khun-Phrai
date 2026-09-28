using TMPro;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public static DayNight Instantiatea;
    public Gradient lightColor;
    public Light sun;          // ลาก Directional Light มาใส่
    private float dayLength = 1800f; // เวลาวันต่อวิ

    private float timeOfDay;   // 0 - 1
    public TMP_Text Timetext;
    private int DayCount;
    private int hour;
    private int minute;
    void Awake()
    {
        Instantiatea = this;
    }
    void Start()
    {
        timeOfDay = PlayerPrefs.GetFloat("TimeDay",0.25f);
        DayCount = PlayerPrefs.GetInt("Day",1);
    }

    void Update()
    {
        // เดินเวลา
        timeOfDay += Time.deltaTime / dayLength;
        
        // หมดวัน แล้วนับวัน +1
        if (timeOfDay >= 1f)
        {
            DayCount++;
            timeOfDay = 0f;
        }

        // สูตรลับความอร่อยพระอาทิตย์เคลื่อนที่
        float sunAngle = timeOfDay * 360f; // หมุนรอบโลก 360 องศา
        sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0); // ระยะห่างลองๆ เล่นดูอันนี้ไม่ตายตัว
        sun.color = lightColor.Evaluate(timeOfDay); // ปรับแสง

        //คำนวณเวลา 24 h
        float totalHours = timeOfDay * 24f;

        hour = Mathf.FloorToInt(totalHours);

        float fractionalHour = totalHours - hour;

        minute = Mathf.FloorToInt(fractionalHour * 60f);

        //แสดงเวลา
        Timetext.text = $"Day {DayCount} Time: {hour:00}:{minute:00}";
    }
    public void Savetime()
    {
        PlayerPrefs.SetFloat("TimeDay",timeOfDay);
        PlayerPrefs.SetInt("Day",DayCount);
    }
}
