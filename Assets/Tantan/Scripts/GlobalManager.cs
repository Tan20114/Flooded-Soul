using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalManager : SingletonPersistant<GlobalManager>
{
    [Header("Data")]
    public BiomeType CurrentBiome;
    public int previousScene = 0;
    public int currentScene = 0;
    public int biomeChangeLastStep = 0;
    public int lastShopStep = -1;

    [Header("Status")]
    public bool isAlwaysOnTop = true;
    public bool isSoundOn = true;
    bool isFirstLoad = true;

    public int boatLevel = 1;
    public int hookLevel = 1;
    public int cat1Level = 0;
    public int cat2Level = 0;
    public int cat3Level = 0;
    public int cat4Level = 0;

    [Header("Currency")]
    public int fishPoints = 0;
    public float distance = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        currentScene = scene.buildIndex;
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
        PlayerPrefs.SetInt("CurrentScene", currentScene);
        PlayerPrefs.SetInt("BiomeChangeLastStep", biomeChangeLastStep);
        PlayerPrefs.SetInt("LastShopStep", lastShopStep);

        PlayerPrefs.SetInt("BoatLevel", boatLevel);
        PlayerPrefs.SetInt("HookLevel", hookLevel);
        PlayerPrefs.SetInt("Cat1Level", cat1Level);
        PlayerPrefs.SetInt("Cat2Level", cat2Level);
        PlayerPrefs.SetInt("Cat3Level", cat3Level);
        PlayerPrefs.SetInt("Cat4Level", cat4Level);

        PlayerPrefs.SetInt("FishPoints", fishPoints);
        PlayerPrefs.SetFloat("Distance", distance);

        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);

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
        currentScene = PlayerPrefs.GetInt("CurrentScene", 0);
        biomeChangeLastStep = PlayerPrefs.GetInt("BiomeChangeLastStep", 0);
        lastShopStep = PlayerPrefs.GetInt("LastShopStep", -1);

        boatLevel = PlayerPrefs.GetInt("BoatLevel", 1);
        hookLevel = PlayerPrefs.GetInt("HookLevel", 1);

        cat1Level = PlayerPrefs.GetInt("Cat1Level", 0);
        cat2Level = PlayerPrefs.GetInt("Cat2Level", 0);
        cat3Level = PlayerPrefs.GetInt("Cat3Level", 0);
        cat4Level = PlayerPrefs.GetInt("Cat4Level", 0);

        fishPoints = PlayerPrefs.GetInt("FishPoints", 0);
        distance = PlayerPrefs.GetFloat("Distance", 0);

        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
    }

    public void Initial()
    {
        LoadData();
        StartCoroutine(AutoSave(60f));
    }
}