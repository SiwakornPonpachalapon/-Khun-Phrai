using UnityEngine;
using System.Collections;


public class EnemyHealth : MonoBehaviour
{
    private EnemyVisual visual;
    private Rigidbody rb;
    
    // =========================
    // HP ศัตรู
    // =========================

    public float hp = 100;

    // =========================
    // สถานะ Stun
    // =========================
    // ป้องกันศัตรูทำ Action ระหว่างโดนตี
    public bool isStunned;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        visual = GetComponent<EnemyVisual>();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        visual.PlayHitFlash();

        Debug.Log($"{name} โดน {damage}");

        StartCoroutine(HitStun());

        if (hp <= 0)
        {
            Die();
        }
    }

    IEnumerator HitStun()
    {
        isStunned = true;

        yield return new WaitForSeconds(0.3f);

        isStunned = false;
    }

    void Die()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.EnemyKilled();
        }
        Destroy(gameObject);
    }
    public void Knockback(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
