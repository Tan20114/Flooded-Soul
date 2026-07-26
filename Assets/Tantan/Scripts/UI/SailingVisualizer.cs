using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SailingVisualizer : MonoBehaviour
{
    BoatController boatController => FindAnyObjectByType<BoatController>();

    [Header("Currency")]
    [SerializeField] TextMeshProUGUI fishPointText;
    [SerializeField] TextMeshProUGUI distanceText;

    [Header("Button")]
    #region TopMost
    [SerializeField] Button topMostButton;
    [SerializeField] Sprite[] topMostSprites;
    #endregion
    [SerializeField] Button sailButton;
    [SerializeField] Button fishButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button soundButton;

    private void LateUpdate()
    {
        topMostButton.image.sprite = GlobalManager.Instance.isAlwaysOnTop ? topMostSprites[1] : topMostSprites[0];
        sailButton.gameObject.SetActive(boatController.state == BoatState.Idle);
        soundButton.image.color = GlobalManager.Instance.isSoundOn ? Color.white : Color.gray;
        FishNShopSwitch();

        fishPointText.text = GlobalManager.Instance.fishPoints.ToString("0000");
        distanceText.text = $"{(GlobalManager.Instance.distance / 1000f):F2} km";

    }

    void FishNShopSwitch()
    {
        if (boatController.isInShopArea)
        {
            shopButton.gameObject.SetActive(true);
            fishButton.gameObject.SetActive(true);
        }
        else
        {
            shopButton.gameObject.SetActive(false);
            fishButton.gameObject.SetActive(false);
        }
    }
}
