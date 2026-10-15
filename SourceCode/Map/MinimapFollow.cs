using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = 50f;
        transform.position = newPos;
    }
}
