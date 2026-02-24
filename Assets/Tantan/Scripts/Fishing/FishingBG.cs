using UnityEngine;

public class FishingBG : MonoBehaviour
{
    SpriteRenderer sr => GetComponent<SpriteRenderer>();

    [SerializeField] BiomeContainer[] biomeAsset; 

    void Start() => sr.sprite = biomeAsset[(int)GlobalManager.Instance.CurrentBiome].underWater;
}
