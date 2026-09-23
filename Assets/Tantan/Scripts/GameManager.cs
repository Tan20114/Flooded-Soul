using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] ShopSpawner spawner;
    [SerializeField] GameObject tutorialPanel;

    [Header("Status")]
    public bool autoStop = true;
    public bool inSession = false;

    [Header("Timer")]
    public float timer = 0;
    public float timeToCount = 0;

    private void Start()
    {
        tutorialPanel.SetActive(!GlobalManager.Instance.isTutorialCompleted);
    }

    private void Update()
    {
        if (!inSession) return;

        timeToCount -= Time.deltaTime;

        if (timeToCount < 34)
        {
            spawner.SpawnShop();
        }

        if (timeToCount < 0 && inSession)
        {
            timeToCount = 0;
            EndFocus(true);
        }
    }

    public void ToggleSound() => GlobalManager.Instance.isSoundOn = !GlobalManager.Instance.isSoundOn;

    public void StartFocus()
    {
        if (timeToCount <= 0) return;
        inSession = true;
    }

    public void EndFocus(bool isSuccessful)
    {
        inSession = false;

        float elapse = timer - timeToCount;
        Debug.Log($"Focus Ended. Elapsed Time: {elapse}, Time to Count: {timeToCount}, Is Successful: {isSuccessful}");

        if (!isSuccessful)
        {
            if (elapse > .5f * timer)
                GlobalManager.Instance.buffDuration = (int)(timer - timeToCount) / 6;
            else
                GlobalManager.Instance.buffDuration = 0;

            HelperFunction.Delay(this, .1f, spawner.SpawnShop);
            HelperFunction.Delay(this, 35, RandomBuff);
        }
        else
            RandomBuff();
    }

    void RandomBuff()
    {
        int ranVal = 0;
        int catLevel = GlobalManager.Instance.cat1Level + GlobalManager.Instance.cat2Level + GlobalManager.Instance.cat3Level + GlobalManager.Instance.cat4Level;

        do
        {
            ranVal = Random.Range(0, 3);
        }
        while (ranVal == 2 && catLevel < 1);

        GlobalManager.Instance.BuffActivate(ranVal, GlobalManager.Instance.buffDuration);
    }
}