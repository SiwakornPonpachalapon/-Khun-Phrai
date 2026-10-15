
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 20f;

    private Collider swordCollider;

    // ป้องกันการทำดาเมจซ้ำในหนึ่งครั้งที่ฟัน
    private bool hasHit = false;

    void Awake()
    {
        swordCollider = GetComponent<Collider>();

        // เริ่มเกมให้ดาบยังไม่ทำดาเมจ
        swordCollider.enabled = false;
    }

    // Animation Event: เริ่มจังหวะที่ดาบฟัน
    public void Hit()
    {
        hasHit = false;
        swordCollider.enabled = true;
    }

    // Animation Event: จบจังหวะที่ดาบฟัน
    public void EndHit()
    {
        swordCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!swordCollider.enabled || hasHit)
            return;

        // ตรวจว่าชน Player หรือไม่
        if (other.CompareTag("Player"))
        {
            hasHit = true;

            //Debug.Log("Enemy sword hit Player!");

        }
    }
}