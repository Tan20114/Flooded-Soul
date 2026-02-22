using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Data")]
    public BiomeType CurrentBiome;

    [Header("Status")]
    public bool autoStop = false;

    [Header("Currency")]
    public int fishPoints = 0;
    public int distance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}