using UnityEngine;
using UnityEngine.UI;

public class ShopBGVisualize : MonoBehaviour
{
    Image bgImage;
    [SerializeField] BiomeContainer[] biomeContainers;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bgImage.sprite = biomeContainers[(int)GlobalManager.Instance.CurrentBiome].insideShop;
    }
}
