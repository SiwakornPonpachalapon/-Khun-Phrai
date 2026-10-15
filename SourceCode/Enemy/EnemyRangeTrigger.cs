using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{
    public enum RangeType
    {
        Chase,
        Attack
    }

    public RangeType rangeType;

    private EnemyMovement movement;
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        movement = GetComponentInParent<EnemyMovement>();
        enemyAttack = GetComponentInParent<EnemyAttack>();

        //Debug.Log($"{name} Trigger Ready : {rangeType}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //Debug.Log($"Player เข้า {rangeType} Range");

        if (rangeType == RangeType.Chase)
        {
            movement.SetPlayerInChaseRange(true);
        }
        else if (rangeType == RangeType.Attack)
        {
            enemyAttack.SetPlayerInAttackRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //Debug.Log($"Player ออกจาก {rangeType} Range");

        if (rangeType == RangeType.Chase)
        {
            movement.SetPlayerInChaseRange(false);
        }
        else if (rangeType == RangeType.Attack)
        {
            enemyAttack.SetPlayerInAttackRange(false);
        }
    }
}