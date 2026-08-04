using TMPro;
using UnityEngine;

public class PomodoroTimeSet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] timeText;

    const int MaxMinutes = 90;

    public void AddTime()
    {
        int totalMinutes = GetTotalMinutes();
        totalMinutes = Mathf.Min(totalMinutes + 15, MaxMinutes);
        SetTime(totalMinutes);
    }

    public void SubtractTime()
    {
        int totalMinutes = GetTotalMinutes();
        totalMinutes = Mathf.Max(totalMinutes - 15, 0);
        SetTime(totalMinutes);
    }

    int GetTotalMinutes()
    {
        int minutes = int.Parse(timeText[0].text);
        int hours = int.Parse(timeText[1].text);

        return hours * 60 + minutes;
    }

    void SetTime(int totalMinutes)
    {
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        timeText[0].text = minutes.ToString("D2");
        timeText[1].text = hours.ToString("D2");
    }

    public void FocusTime()
    {
        if(GlobalManager.Instance.isTutorialCompleted)
        {
            GameManager.Instance.timeToCount = GetTotalMinutes() * 60;
            GlobalManager.Instance.buffDuration = (GetTotalMinutes() * 10f);
        }
        else
        {
            GameManager.Instance.timeToCount = 30;
            GlobalManager.Instance.buffDuration = 30;
        }
    }
}