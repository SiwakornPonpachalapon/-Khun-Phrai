using System.Collections;
using UnityEngine;

public class BossAmmoSpawner : MonoBehaviour
{
    [Header("Ammo")]
    public GameObject ammoPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 15f;

    // ให้มี Ammo บนสนามพร้อมกันสูงสุดกี่อัน
    public int maxAmmoOnArena = 2;

    private int currentAmmoCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentAmmoCount < maxAmmoOnArena)
            {
                SpawnAmmo();
            }
        }
    }

    void SpawnAmmo()
    {
        if (spawnPoints.Length == 0)
            return;

        int randomIndex =
            Random.Range(0, spawnPoints.Length);

        Transform point =
            spawnPoints[randomIndex];

        GameObject ammo = Instantiate(
            ammoPrefab,
            point.position,
            point.rotation
        );

        currentAmmoCount++;

        // บอก Pickup ว่าใคร Spawn มัน
        AmmoPickup pickup =
            ammo.GetComponent<AmmoPickup>();

        if (pickup != null)
        {
            pickup.SetSpawner(this);
        }
    }

    public void AmmoCollected()
    {
        currentAmmoCount--;
        currentAmmoCount =
            Mathf.Max(0, currentAmmoCount);
    }
}