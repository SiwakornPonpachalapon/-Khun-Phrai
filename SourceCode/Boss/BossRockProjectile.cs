using UnityEngine;

public class BossRockProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float travelTime = 1.2f;
    public float arcHeight = 6f;

    [Header("Damage")]
    public float damage = 30f;
    public float explosionRadius = 2f;

    [Header("Effect")]
    public GameObject impactEffect;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float timer;

    private bool launched;


    public void Launch(Vector3 target)
    {
        startPosition = transform.position;
        targetPosition = target;

        timer = 0f;
        launched = true;
    }


    void Update()
    {
        if (!launched)
            return;


        // =========================
        // TIME
        // =========================

        timer += Time.deltaTime;

        float t = timer / travelTime;

        t = Mathf.Clamp01(t);


        // =========================
        // ตำแหน่งแนวตรง
        // =========================

        Vector3 position =
            Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );


        // =========================
        // ทำเส้นโค้ง
        // =========================

        float height =
            Mathf.Sin(t * Mathf.PI)
            * arcHeight;

        position.y += height;

        transform.position = position;


        // =========================
        // หมุนหิน
        // =========================

        transform.Rotate(
            300f * Time.deltaTime,
            200f * Time.deltaTime,
            100f * Time.deltaTime
        );


        // =========================
        // ถึงเป้าหมาย
        // =========================

        if (t >= 1f)
        {
            Impact();
        }
    }


    void Impact()
    {
        launched = false;


        // =========================
        // EFFECT
        // =========================

        if (impactEffect != null)
        {
            Instantiate(
                impactEffect,
                targetPosition,
                Quaternion.identity
            );
        }


        // =========================
        // DAMAGE
        // =========================

        Collider[] hits = Physics.OverlapSphere(
            targetPosition,
            explosionRadius
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerManager.Instance.SetHp(-damage);

                // เจอ Player แล้วพอ
                break;
            }
        }


        Destroy(gameObject);
    }


    // =========================
    // DEBUG
    // =========================

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            targetPosition,
            explosionRadius
        );
    }
}