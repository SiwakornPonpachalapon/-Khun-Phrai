using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public EnemyState CurrentState { get; private set; }

    public event Action<EnemyState> OnStateChanged;

    void Awake()
    {
        Instance = this;
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}