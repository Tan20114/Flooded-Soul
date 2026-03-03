using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : SingletonPersistant<GlobalManager>
{
    AudioSource source => GetComponent<AudioSource>();

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
}