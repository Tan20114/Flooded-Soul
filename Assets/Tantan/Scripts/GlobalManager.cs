using UnityEngine;

public class GlobalManager : SingletonPersistant<GlobalManager>
{
    [Header("Data")]
    public BiomeType CurrentBiome;

    [Header("Status")]
    public int boatLevel = 1;
    public int hookLevel = 1;
    public int cat1Level = 0;
    public int cat2Level = 0;
    public int cat3Level = 0;
    public int cat4Level = 0;

    [Header("Currency")]
    public int fishPoints = 0;
    public int distance = 0;
}