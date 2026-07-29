using UnityEngine;

public class ShopDespawner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Shop"));

        if (hit != null)
        {
            hit.GetComponent<ShopLayer>().state = ShopState.despawned;
            hit.gameObject.SetActive(false);
        }
    }
}
