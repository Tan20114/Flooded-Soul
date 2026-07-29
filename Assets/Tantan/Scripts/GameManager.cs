using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    [SerializeField] ShopSpawner spawner;

    [Header("Status")]
    public bool autoStop = true;
    public bool inSession = false;

    [Header("Timer")]
    public float timeToCount = 0;

    private void Update()
    {
        if (!inSession) return;

        timeToCount -= Time.deltaTime;
        Debug.Log("Time Left : " + (int)timeToCount);
        
        if (timeToCount < 5)
        {
            spawner.SpawnShop();
        }

        if (timeToCount < 0)
        {
            timeToCount = 0;
            EndFocus(true);
        }
    }

    public void ToggleSound() => GlobalManager.Instance.isSoundOn = !GlobalManager.Instance.isSoundOn;

    public void StartFocus() => inSession = true;

    public void EndFocus(bool isSuccessful)
    {
        if (isSuccessful)
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
    }
}