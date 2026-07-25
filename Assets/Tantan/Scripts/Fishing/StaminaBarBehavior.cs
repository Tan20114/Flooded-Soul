using UnityEngine;
using UnityEngine.UI;

public class StaminaBarBehavior : MonoBehaviour
{
    [SerializeField] GameObject staminaSlider;
    [SerializeField] Image staminaFill;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] Color[] barColor;

    void Update()
    {
        staminaSlider.gameObject.SetActive(FishingManager.Instance.isMinigame);

        if (!staminaSlider.gameObject.activeSelf) return;

        staminaSlider.transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
        staminaFill.fillAmount = TCalculator();
        staminaFill.color = Color.Lerp(barColor[1], barColor[0], TCalculator());
    }

    float TCalculator() => FishingManager.Instance.CurrentMinigameTime / FishingManager.Instance.calculatedTime;
}
