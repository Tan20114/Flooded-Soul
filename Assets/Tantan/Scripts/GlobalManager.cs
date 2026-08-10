using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : SingletonPersistant<GlobalManager>
{
    CollectionManager cm => GetComponent<CollectionManager>();

    [Header("Data")]
    Coroutine currentBuffCycle = null;
    public BiomeType CurrentBiome;
    public int previousScene = 0;
    public int currentScene = 0;
    public int biomeChangeLastStep = 0;

    [Header("Status")]
    public bool isAlwaysOnTop = true;
    public bool isSoundOn = true;
    public bool isTutorialCompleted = false;
    bool isFirstLoad = true;
    public bool oceanVisited = false;
    public bool iceVisited = false;
    public bool forestVisited = false;

    public int boatLevel = 1;
    public int hookLevel = 1;
    public int cat1Level = 0;
    public int cat2Level = 0;
    public int cat3Level = 0;
    public int cat4Level = 0;

    [Header("Buff")]
    // 0 = Rarity Up, 1 = Strength Up, 2 = Double Passive Income
    public bool[] buffs;
    public float buffDuration = 0;

    [Header("Achievements")]
    public bool story1Unlocked = false; // 10 All Legend
    public bool story2Unlocked = false; // 100 Scamambas
    public bool story3Unlocked = false; // First Upgrade
    public bool story4Unlocked = false; // All Cat
    public bool story5Unlocked = false; // 25 Plab Fish
    public bool story6Unlocked = false; // 2 Cat
    public bool story7Unlocked = false; // All Biome

    [Header("Currency")]
    public int fishPoints = 0;
    public float distance = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initial();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void FixedUpdate()
    {
        BiomeCheck();
        AchievementCheck();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isFirstLoad)
        {
            previousScene = 2;
            isFirstLoad = false;
        }
        else
            previousScene = currentScene;

        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CurrentBiome", (int)CurrentBiome);
        PlayerPrefs.SetInt("PreviousScene", previousScene);
        PlayerPrefs.SetInt("BiomeChangeLastStep", biomeChangeLastStep);

        PlayerPrefs.SetInt("BoatLevel", boatLevel);
        PlayerPrefs.SetInt("HookLevel", hookLevel);
        PlayerPrefs.SetInt("Cat1Level", cat1Level);
        PlayerPrefs.SetInt("Cat2Level", cat2Level);
        PlayerPrefs.SetInt("Cat3Level", cat3Level);
        PlayerPrefs.SetInt("Cat4Level", cat4Level);

        PlayerPrefs.SetInt("FishPoints", fishPoints);
        PlayerPrefs.SetFloat("Distance", distance);

        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.SetInt("OceanVisited", oceanVisited ? 1 : 0);
        PlayerPrefs.SetInt("IceVisited", iceVisited ? 1 : 0);
        PlayerPrefs.SetInt("ForestVisited", forestVisited ? 1 : 0);
        PlayerPrefs.SetInt("TutorialCompleted", isTutorialCompleted ? 1 : 0);
        cm.Save();

        PlayerPrefs.Save();
    }

    IEnumerator AutoSave(float time)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(time);
            SaveData();
        }
    }

    void LoadData()
    {
        if (!PlayerPrefs.HasKey("BoatLevel"))
            return;

        CurrentBiome = (BiomeType)PlayerPrefs.GetInt("CurrentBiome", 0);
        previousScene = PlayerPrefs.GetInt("PreviousScene", 3);
        biomeChangeLastStep = PlayerPrefs.GetInt("BiomeChangeLastStep", 0);

        boatLevel = PlayerPrefs.GetInt("BoatLevel", 1);
        hookLevel = PlayerPrefs.GetInt("HookLevel", 1);

        cat1Level = PlayerPrefs.GetInt("Cat1Level", 0);
        cat2Level = PlayerPrefs.GetInt("Cat2Level", 0);
        cat3Level = PlayerPrefs.GetInt("Cat3Level", 0);
        cat4Level = PlayerPrefs.GetInt("Cat4Level", 0);

        fishPoints = PlayerPrefs.GetInt("FishPoints", 0);
        distance = PlayerPrefs.GetFloat("Distance", 0);

        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        oceanVisited = PlayerPrefs.GetInt("OceanVisited", 0) == 1;
        iceVisited = PlayerPrefs.GetInt("IceVisited", 0) == 1;
        forestVisited = PlayerPrefs.GetInt("ForestVisited", 0) == 1;
        isTutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

        cm.Load();
    }

    void Initial()
    {
        LoadData();
        StartCoroutine(AutoSave(60f));
    }

    void BiomeCheck()
    {
        switch (CurrentBiome)
        {
            case BiomeType.Ocean:
                oceanVisited = true;
                break;
            case BiomeType.Ice:
                iceVisited = true;
                break;
            case BiomeType.Forest:
                forestVisited = true;
                break;
        }
    }

    public void BuffActivate(int buffIndex, float duration)
    {
        currentBuffCycle = StartCoroutine(BuffOn(buffIndex, duration));
    }

    IEnumerator BuffOn(int buffIndex, float duration)
    {
        Debug.Log($"Buff On : Duration : {duration}");
        buffs[buffIndex] = true;
        yield return new WaitForSeconds(duration);
        Debug.Log("Buff Off");
        buffs[buffIndex] = false;
        currentBuffCycle = null;
        buffDuration = 0;
    }

    public void ResetBuff()
    {
        if (currentBuffCycle != null)
        {
            StopCoroutine(currentBuffCycle);
            currentBuffCycle = null;
        }

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = false;
        }

        buffDuration = 0;
    }

    #region Achievement
    void AchievementCheck()
    {
        story1Unlocked = Achievement1Condition();
        story2Unlocked = Achievement2Condition();
        story3Unlocked = Achievement3Condition();
        story4Unlocked = Achievement4Condition();
        story5Unlocked = Achievement5Condition();
        story6Unlocked = Achievement6Condition();
        story7Unlocked = Achievement7Condition();
    }

    bool Achievement1Condition()
    {
        SerializedDictionary<LegendaryFishType, int> legendary = cm.GetLegendaryFishCollection();

        return legendary[LegendaryFishType.PlabFish] >= 10
            && legendary[LegendaryFishType.JollyFish] >= 10
            && legendary[LegendaryFishType.KelpboneFish] >= 10;
    }

    bool Achievement2Condition()
    {
        return cm.GetCommonFishCollection()[CommonFishType.SacabambaspisFish] >= 100;
    }

    bool Achievement3Condition()
    {
        return boatLevel >= 2 || hookLevel >= 2 || cat1Level >= 1 || cat2Level >= 1 || cat3Level >= 1 || cat4Level >= 1;
    }

    bool Achievement4Condition()
    {
        return cat1Level >= 1 && cat2Level >= 1 && cat3Level >= 1 && cat4Level >= 1;
    }

    bool Achievement5Condition()
    {
        return cm.GetLegendaryFishCollection()[LegendaryFishType.PlabFish] >= 25;
    }

    bool Achievement6Condition()
    {
        return new[]
        {
            cat1Level,
            cat2Level,
            cat3Level,
            cat4Level
        }.Count(level => level > 0) >= 2;
    }

    bool Achievement7Condition()
    {
        return oceanVisited && iceVisited && forestVisited;
    }
    #endregion
}