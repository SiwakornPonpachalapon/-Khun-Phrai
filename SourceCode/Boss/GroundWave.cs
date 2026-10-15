using UnityEngine;

public class GroundWave : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float lifeTime = 3f;

    [Header("Damage")]
    public float damage = 20f;

    [Header("Ground Effect")]
    public GameObject groundEffectPrefab;
    public float effectSpawnInterval = 0.12f;

    private Vector3 moveDirection;

    private float effectTimer;

    private bool hasHitPlayer;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }


    void Update()
    {
        // =========================
        // MOVE
        // =========================

        transform.position +=
            moveDirection * speed * Time.deltaTime;


        // =========================
        // SPAWN GROUND EFFECT
        // =========================

        effectTimer -= Time.deltaTime;

        if (effectTimer <= 0f)
        {
            SpawnGroundEffect();

            effectTimer = effectSpawnInterval;
        }
    }


    void SpawnGroundEffect()
    {
        if (groundEffectPrefab == null)
            return;

        Instantiate(
            groundEffectPrefab,
            transform.position,
            transform.rotation
        );
    }


    void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer)
            return;

        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            //Debug.Log("Boss Wave Hit Player!");

            PlayerManager.Instance.SetHp(-damage);
        }
    }
}