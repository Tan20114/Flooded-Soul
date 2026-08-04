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

        if (isSuccessful)
            RandomBuff();
        else
        {
            GlobalManager.Instance.buffDuration = 0;
            HelperFunction.Delay(this, .1f, spawner.SpawnShop);
        }
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