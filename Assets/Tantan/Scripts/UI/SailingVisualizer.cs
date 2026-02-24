using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SailingVisualizer : MonoBehaviour
{
    BoatController boatController => FindAnyObjectByType<BoatController>();

    [Header("Currency")]
    [SerializeField] TextMeshProUGUI fishPointText;

    [Header("Button")]
    [SerializeField] Button fishButton;
    [SerializeField] Button shopButton;

    private void LateUpdate()
    {
        fishPointText.text = GlobalManager.Instance.fishPoints.ToString();

        FishNShopSwitch();
    }

    void FishNShopSwitch()
    {
        if (boatController.isInShopArea)
        {
            shopButton.gameObject.SetActive(true);
            fishButton.gameObject.SetActive(false);
        }
        else
        {
            shopButton.gameObject.SetActive(false);
            fishButton.gameObject.SetActive(true);
        }
    }
}
