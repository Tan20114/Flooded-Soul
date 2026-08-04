using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Reference")]
    FishingSceneUI fui;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Animator tutorialAnimator;

    [Header("Parameter")]
    [SerializeField] int tutorialIndex = 0;
    [SerializeField] string[] textList;


    private void OnEnable()
    {
        HelperFunction.Delay(this, 1.5f, () =>
        {
            Debug.Log("Time Stop");
            Time.timeScale = 0;
        });
    }

    public void TriggerTutorial()
    {
        if (!GlobalManager.Instance.isTutorialCompleted)
        {
            if (tutorialIndex >= textList.Length)
            {
                GlobalManager.Instance.isTutorialCompleted = true;
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
            else
            {
                tutorialText.text = textList[tutorialIndex];
                tutorialAnimator.SetTrigger($"{++tutorialIndex}");
            }
        }
    }
}
