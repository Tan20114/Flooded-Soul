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
        HelperFunction.Delay(this, 0.01f, RandomBiomeLayer);
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
        sr.sprite = pm.CurrentBiomeAsset.shop;
    }
}
