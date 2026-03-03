using UnityEngine;

[System.Serializable]
public class CatLevelData
{
    [Header("Interval")]
    public int interval;

    [Header("Cat Chance")]
    public int value1;
    public int value2;
    public int value3;
    public int value4;

    public int Total => value1 + value2 + value3 + value4;

    public CatLevelData GetNormalized()
    {
        int total = Total;
        if (total <= 0) return this;

        float scale = 100f / total;

        return new CatLevelData
        {
            value1 = Mathf.RoundToInt(value1 * scale),
            value2 = Mathf.RoundToInt(value2 * scale),
            value3 = Mathf.RoundToInt(value3 * scale),
            value4 = Mathf.RoundToInt(value4 * scale)
        };
    }
}

[CreateAssetMenu(fileName = "Upgrade Data", menuName = "Upgrade/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public float[] boatSpeed;
    public float[] hookSpeed;
    public float[] hookForce;
    public float[] minigameRadius;
    public CatLevelData[] cat1Possibilities;
    public CatLevelData[] cat2Possibilities;
    public CatLevelData[] cat3Possibilities;
    public CatLevelData[] cat4Possibilities;
}
