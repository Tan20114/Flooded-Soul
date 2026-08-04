using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform tutorialSpawnPoint;
    public ShopLayer shop;


    public void SpawnShop()
    {
        if (shop.state == ShopState.despawned)
        {
            Transform refPoint = spawnPoint;

            shop.transform.position = new Vector2(refPoint.position.x, shop.transform.position.y);
            shop.gameObject.SetActive(true);
            shop.state = ShopState.spawned;
        }
    }

    public void DespawnShop()
    {
        if (shop.state == ShopState.despawned) return;
        Transform refPoint = spawnPoint;
        shop.transform.position = new Vector2(refPoint.position.x, shop.transform.position.y);
        shop.state = ShopState.despawned;
        gameObject.SetActive(false);
    }
}
