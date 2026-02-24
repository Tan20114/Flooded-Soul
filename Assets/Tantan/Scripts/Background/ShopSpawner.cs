using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] ShopLayer shop;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
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
}
