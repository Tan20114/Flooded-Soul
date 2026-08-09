using TMPro;
using UnityEngine;

public class PomodoroTimerVisual : MonoBehaviour
{
    [SerializeField] bool isHour = false;
    [SerializeField] Animator visualOnly;
    [SerializeField] TextMeshProUGUI visualText;
    [SerializeField] TextMeshProUGUI baseText;
    [SerializeField] AudioClip paperFlip;

    private string previousText;

    private void OnEnable()
    {
        visualText.text = baseText.text;
        previousText = visualText.text;
    }

    private void Update()
    {
        if (!isHour)
            visualText.text = ((int)(GameManager.Instance.timeToCount / 60)).ToString("00");
        else
            visualText.text = ((int)(GameManager.Instance.timeToCount / 3600)).ToString("00");

        if (visualText.text != previousText)
        {
            previousText = visualText.text;
            visualOnly.SetTrigger("ReverseFlip");
            SFXManager.instance.PlaySoundFXClip(paperFlip);
        }
    }
}
