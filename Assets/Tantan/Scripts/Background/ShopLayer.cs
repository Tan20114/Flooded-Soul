using UnityEngine;

public class ShopLayer : ParallaxLayer
{
    protected override void RandomBiomeLayer()
    {
        Sprite shopSprite = pm.CurrentBiomeAsset.shop;
        if (shopSprite != null)
        {
            sr.sprite = shopSprite;
        }
    }
}
