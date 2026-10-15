using UnityEngine;

public class CombatEffect : MonoBehaviour
{
    public static CombatEffect Instance;

    [Header("Effect")]
    [SerializeField] private GameObject slashPrefab;

    [Header("Knockback")]
    public float knockbackForce = 4f;

    void Awake()
    {
        Instance = this;
    }

    public void PlayHit(EnemyHealth enemy, Transform attackPoint)
    {
        SpawnSlash(attackPoint);

        Vector3 dir =
            (enemy.transform.position - attackPoint.position).normalized;

        enemy.Knockback(dir, knockbackForce);
        HitStopManager.Instance.Play(0.04f,0.12f);
    }

    void SpawnSlash(Transform attackPoint)
    {
        Instantiate(
            slashPrefab,
            attackPoint.position,
            attackPoint.rotation
        );
    }
}