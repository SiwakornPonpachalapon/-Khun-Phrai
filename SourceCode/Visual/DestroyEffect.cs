using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destroyTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}