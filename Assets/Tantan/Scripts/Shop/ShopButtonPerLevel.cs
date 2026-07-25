using UnityEngine;
using UnityEngine.UI;

public class ShopButtonPerLevel : MonoBehaviour
{
    [SerializeField] Sprite[] levelSprites;
    Image buttonImage => GetComponent<Image>();

    // Update is called once per frame
    void Update()
    {
        buttonImage.sprite = levelSprites[GlobalManager.Instance.hookLevel - 1];
    }
}
