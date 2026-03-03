using UnityEngine;

public enum ShopState
{
    spawned,
    despawned
}

public class ShopLayer : ParallaxLayer
{
    public ShopState state = ShopState.despawned;

    protected override void Update()
    {
        MovementControl();
        RegenLayer();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RandomBiomeLayer();
    }

    protected override void RegenLayer()
    {
        if (transform.position.x <= -pm.RegenPoint.position.x)
        {
            transform.position = new Vector2(pm.RegenPoint.position.x, transform.position.y);
            state = ShopState.despawned;
            gameObject.SetActive(false);
        }
    }

    protected override void RandomBiomeLayer()
    {
        Sprite shopSprite = pm.CurrentBiomeAsset.shop;
        if (shopSprite != null)
        {
            sr.sprite = shopSprite;
        }
    }
}
