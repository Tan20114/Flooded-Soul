using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] ShopLayer shop;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnShop();
        }
    }

    public void SpawnShop()
    {
        if (shop.state == ShopState.despawned)
        {
            shop.transform.position = new Vector2(spawnPoint.position.x,shop.transform.position.y);
            shop.gameObject.SetActive(true);
            shop.state = ShopState.spawned;
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
