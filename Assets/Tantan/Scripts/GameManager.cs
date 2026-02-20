using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Data")]
    public BiomeType CurrentBiome;

    [Header("Currency")]
    public int fishPoints = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
