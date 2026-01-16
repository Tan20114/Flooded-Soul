using Lean.Pool;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    [Header ("References")]
    SpriteRenderer fishBound => GameObject.FindGameObjectWithTag("FishBound").GetComponent<SpriteRenderer>();
    FishPossibilities possibilities => GetComponent<FishPossibilities>();
    [SerializeField] Transform[] spawnPoints;
    
    [Header("Fish Pool")]
    [SerializeField] LeanGameObjectPool commonFishPool;
    [SerializeField] LeanGameObjectPool uncommonFishPool;
    [SerializeField] LeanGameObjectPool rareFishPool;
    [SerializeField] LeanGameObjectPool legendaryFishPool;

    [Header("Fish Spawn Area")]
    [SerializeField] float initialYRatio = 0.2f;
    [SerializeField] float commonYRatio = 0.4f;
    [SerializeField] float uncommonYRatio = 0.6f;
    [SerializeField] float rareYRatio = 0.75f;
    [SerializeField] float legendaryYRatio = 0.9f;
    [SerializeField] float fishSpawnOffset = 2f;

    [Header("Properties")]
    [SerializeField] int maxFishCount = 8;

    int commonFishCount = 0;
    int uncommonFishCount = 0;
    int rareFishCount = 0;
    int legendaryFishCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomFishPossibilities();
    }

    // Update is called once per frame
    void Update()
    {
        if (commonFishCount < commonFishPool.Capacity)
        {
            commonFishPool.Spawn(new Vector2(RandomSpawnpoint().position.x,CalculateYPos(FishType.Common)),Quaternion.identity,commonFishPool.transform);
            commonFishCount++;
        }
        if (uncommonFishCount < uncommonFishPool.Capacity)
        {
            uncommonFishPool.Spawn(new Vector2(RandomSpawnpoint().position.x, CalculateYPos(FishType.Uncommon)), Quaternion.identity, uncommonFishPool.transform);
            uncommonFishCount++;
        }
        if (rareFishCount < rareFishPool.Capacity)
        {
            rareFishPool.Spawn(new Vector2(RandomSpawnpoint().position.x, CalculateYPos(FishType.Rare)), Quaternion.identity, rareFishPool.transform);
            rareFishCount++;
        }
        if (legendaryFishCount < legendaryFishPool.Capacity)
        {
            legendaryFishPool.Spawn(new Vector2(RandomSpawnpoint().position.x, CalculateYPos(FishType.Legendary)), Quaternion.identity, legendaryFishPool.transform);
            legendaryFishCount++;
        }
    }

    Transform RandomSpawnpoint() => spawnPoints[Random.Range(0, spawnPoints.Length)];

    float CalculateYPos(FishType type)
    {
        fishSpawnOffset = fishBound.transform.position.y / 2;
        float allYRange = fishBound.bounds.size.y;

        float fishYRange = 0;

        switch (type)
        {
            case FishType.Common:
                fishYRange = Random.Range(allYRange * -initialYRatio, allYRange * -commonYRatio);
                break;
            case FishType.Uncommon:
                fishYRange = Random.Range(allYRange * -commonYRatio, allYRange * -uncommonYRatio);
                break;
            case FishType.Rare:
                fishYRange = Random.Range(allYRange * -uncommonYRatio, allYRange * -rareYRatio);
                break;
            case FishType.Legendary:
                fishYRange = Random.Range(allYRange * -rareYRatio, allYRange * -legendaryYRatio);
                break;
            default:
                fishYRange = -4.5f;
                break;
        }

        return fishYRange + fishSpawnOffset;
    }

    void RandomFishPossibilities()
    {
        int commonFishCapacity = 0;
        int uncommonFishCapacity = 0;
        int rareFishCapacity = 0;
        int legendaryFishCapacity = 0;

        for (int i = 0; i < maxFishCount; i++)
        {
            int ranVal = Random.Range(0, 100);

            if (ranVal < possibilities.CommonMax)
                commonFishCapacity++;
            else if (ranVal < possibilities.UncommonMax)
                uncommonFishCapacity++;
            else if (ranVal < possibilities.RareMax)
                rareFishCapacity++;
            else
                legendaryFishCapacity++;
        }

        commonFishPool.Capacity = commonFishCapacity;
        uncommonFishPool.Capacity = uncommonFishCapacity;
        rareFishPool.Capacity = rareFishCapacity;
        legendaryFishPool.Capacity = legendaryFishCapacity;
    }

    private void OnDrawGizmos()
    {
        if (fishBound == null) return;

        float allYRange = fishBound.bounds.size.y;
        float offset = fishBound.transform.position.y / 2f;
        float width = fishBound.bounds.size.x;

        DrawBand(Color.gray, allYRange, offset, initialYRatio, commonYRatio);
        DrawBand(Color.green, allYRange, offset, commonYRatio, uncommonYRatio);
        DrawBand(Color.blue, allYRange, offset, uncommonYRatio, rareYRatio);
        DrawBand(Color.yellow, allYRange, offset, rareYRatio, legendaryYRatio);
    }

    void DrawBand(Color color, float allYRange, float offset, float minRatio, float maxRatio)
    {
        float minY = -allYRange * minRatio + offset;
        float maxY = -allYRange * maxRatio + offset;

        float height = Mathf.Abs(maxY - minY);
        float centerY = (minY + maxY) * 0.5f;

        Gizmos.color = color;
        Gizmos.DrawWireCube(new Vector3(fishBound.bounds.center.x, centerY, 0f),new Vector3(fishBound.bounds.size.x, height, 0f));
    }
}
