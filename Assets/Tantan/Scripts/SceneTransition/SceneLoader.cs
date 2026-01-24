using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transAnimator;
    [SerializeField] float animTime = 1f;

    [SerializeField] GameObject image;

    public IEnumerator LoadScene(int index)
    {
        transAnimator.SetTrigger("SceneOut");
        yield return new WaitForSecondsRealtime(animTime);
        SceneManager.LoadScene(index);
    }

    public IEnumerator ExitGame()
    {
        transAnimator.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(animTime);
        Application.Quit();
    }
}
