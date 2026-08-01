using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SailingVisualizer : MonoBehaviour
{
    BoatController boatController => FindAnyObjectByType<BoatController>();

    [Header("Currency")]
    [SerializeField] TextMeshProUGUI fishPointText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI hourText;

    [Header("Button")]
    #region TopMost
    [SerializeField] Button topMostButton;
    [SerializeField] Sprite[] topMostSprites;
    #endregion
    [SerializeField] Button fishButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button soundButton;
    [SerializeField] Button collectionButton;
    [SerializeField] Button focusButton;
    [SerializeField] Button endSessionButton;
    [SerializeField] Button[] timeSetButton;

    [Header("Image")]
    [SerializeField] GameObject buffPanel;
    [SerializeField] Image buffImage;
    [SerializeField] Sprite[] buffSprites;

    [Header("Animator")]
    [SerializeField] Animator[] paperAnimator;

    bool isHourDataChange = false;
    string prevText = string.Empty;
    string currentText = string.Empty;

    private void Start()
    {
        prevText = hourText.text;
        currentText = hourText.text;
    }

    private void LateUpdate()
    {
        topMostButton.image.sprite = GlobalManager.Instance.isAlwaysOnTop ? topMostSprites[1] : topMostSprites[0];
        soundButton.image.color = GlobalManager.Instance.isSoundOn ? Color.white : Color.gray;
        FishNShopSwitch();
        fishButton.interactable = GlobalManager.Instance.buffDuration > 0;
        buffPanel.SetActive(GlobalManager.Instance.buffDuration > 0 && !GameManager.Instance.inSession);
        collectionButton.interactable = boatController.state == BoatState.Idle && !GameManager.Instance.inSession;

        fishPointText.text = GlobalManager.Instance.fishPoints.ToString("0000");
        distanceText.text = $"{(GlobalManager.Instance.distance / 1000f):F2} km";

        focusButton.gameObject.SetActive(boatController.state == BoatState.Idle && !GameManager.Instance.inSession);
        endSessionButton.gameObject.SetActive(GameManager.Instance.inSession && boatController.state != BoatState.Idle);

        foreach (Button b in timeSetButton)
            b.gameObject.SetActive(!GameManager.Instance.inSession);

        if (buffPanel.activeSelf)
            buffImage.sprite = buffSprites[Array.IndexOf(GlobalManager.Instance.buffs, true)];
    }

    void FishNShopSwitch()
    {
        if (boatController.isInShopArea && !GameManager.Instance.inSession)
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

    public void IncreaseAnim()
    {
        prevText = currentText;
        currentText = hourText.text;

        isHourDataChange = !prevText.Equals(currentText);

        paperAnimator[0].SetTrigger("PaperFlip");

        if (isHourDataChange)
        {
            paperAnimator[1].SetTrigger("PaperFlip");
            isHourDataChange = false;
        }
    }

    public void DecreaseAnim()
    {
        prevText = currentText;
        currentText = hourText.text;

        isHourDataChange = !prevText.Equals(currentText);

        paperAnimator[0].SetTrigger("ReverseFlip");

        if (isHourDataChange)
        {
            paperAnimator[1].SetTrigger("ReverseFlip");
            isHourDataChange = false;
        }
    }
}
