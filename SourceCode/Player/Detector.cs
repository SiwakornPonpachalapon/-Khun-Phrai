using UnityEngine;

public class Detector : MonoBehaviour
{
    public float detectRadius = 3f;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Tree"))
            {
                Debug.Log("เจอต้นไม้");
            }
        }
    }
}