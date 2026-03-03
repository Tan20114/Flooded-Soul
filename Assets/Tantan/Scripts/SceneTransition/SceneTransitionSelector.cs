using UnityEngine;

public class SceneTransitionSelector : MonoBehaviour
{
    SceneLoader sl => FindAnyObjectByType<SceneLoader>();
    Animator animator => GetComponent<Animator>();
    [Range(1, 3)]
    [SerializeField] int sceneInIdex;
    public int SceneInIdex { get => sceneInIdex; set => sceneInIdex = value; }
    int sceneIdex = 0;
    public int SceneIdex { get => sceneIdex; set => sceneIdex = value; }

    bool isInFin = false;
    [SerializeField] bool isFishingScene;

    private void Start()
    {
        if (GlobalManager.Instance.currentScene == 0)
        {
            sceneInIdex = GlobalManager.Instance.previousScene;

            if (GlobalManager.Instance.previousScene == 1)
                animator.SetTrigger("isFishOut");
        }
        else
            sceneInIdex = GlobalManager.Instance.currentScene;

        animator.SetInteger("sceneIndex", sceneInIdex);
        HelperFunction.Delay(this,sl.AnimTime,() => {
            isInFin = true;
        });
    }

    void Update()
    {
        Debug.Log(isInFin);
        if (!isInFin) return;
        animator.SetBool("isFish",isFishingScene);
        animator.SetInteger("sceneIndex", sceneIdex);
    }

    public void SetSceneIndex(int index) => sceneIdex = index;
}
