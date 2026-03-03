using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] ShopLayer shop;
    [SerializeField] float shopSpawnDistance = 50;
    [SerializeField] int shopSpawnChance = 20;

    void Update()
    {
        CheckShopSpawn();
    }

    void CheckShopSpawn()
    {
        int currentStep = (int)(GlobalManager.Instance.distance / shopSpawnDistance);
        int ranVal = Random.Range(1, 101);

        if (currentStep > GlobalManager.Instance.lastShopStep)
        {
            GlobalManager.Instance.lastShopStep = currentStep;
            if (ranVal <= shopSpawnChance)
                SpawnShop();
        }
    }

    void SpawnShop()
    {
        if (shop.state == ShopState.despawned)
        {
            shop.transform.position = new Vector2(spawnPoint.position.x,shop.transform.position.y);
            shop.state = ShopState.spawned;
            shop.gameObject.SetActive(true);
        }
    }

    public void DespawnShop()
    {
        if (shop.state == ShopState.despawned) return;
        transform.position = new Vector2(spawnPoint.position.x, shop.transform.position.y);
        shop.state = ShopState.despawned;
        gameObject.SetActive(false);
    }
}
