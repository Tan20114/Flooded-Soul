using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingSceneUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fishText;

    [SerializeField] Button pauseButton;
    [SerializeField] Button backButton;
    [SerializeField] Sprite[] pauseSprites;

    bool isPause = false;

    private void Update()
    {
        Time.timeScale = isPause ? 0 : 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        backButton.interactable = !FishingManager.Instance.isMinigame;
        backButton.image.raycastTarget = !FishingManager.Instance.isMinigame;

        fishText.text = GlobalManager.Instance.fishPoints.ToString("0000");
        pauseButton.image.sprite = isPause ? pauseSprites[0] : pauseSprites[1];
    }

    public void TogglePause()
    {
        isPause = !isPause;
    }
}
