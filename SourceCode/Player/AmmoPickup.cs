using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Ammo")]
    public int ammoAmount = 4;

    private BossAmmoSpawner spawner;

    private bool collected = false;

    public void SetSpawner(BossAmmoSpawner newSpawner)
    {
        spawner = newSpawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        collected = true;

        PlayerManager.Instance.AddAmmo(ammoAmount);

        PlayerManager.Instance.UpdateInventory();

        // บอก Spawner ว่าอันนี้ถูกเก็บแล้ว
        if (spawner != null)
        {
            spawner.AmmoCollected();
        }

        Destroy(gameObject);
    }
}