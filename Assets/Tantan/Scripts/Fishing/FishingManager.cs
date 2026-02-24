using Lean.Pool;
using UnityEngine;

public class FishingManager : Singleton<FishingManager>
{
    #region Variables
    [Header("References")]
    FishSpawner spawner => FindAnyObjectByType<FishSpawner>();
    CollectionManager collection => FindAnyObjectByType<CollectionManager>();
    FishingHook hook => FindAnyObjectByType<FishingHook>();

    [Header("Status")]
    public bool isMinigame = false;

    [Header("Objects")]
    [SerializeField] GameObject minigameAreaPrefab;
    public Transform fishCatchLine;
    [SerializeField] Fish targetFish;
    public Fish TargetFish
    {
        get => targetFish;
    }
    GameObject minigameArea;
    public GameObject MinigameArea
    {
        get => minigameArea;
        private set => minigameArea = value;
    }
    #endregion

    #region Minigame
    public void StartMinigame(Fish target)
    {
        isMinigame = true;
        targetFish = target;

        minigameArea = Instantiate(minigameAreaPrefab, targetFish.transform.position, Quaternion.identity);
    }

    public void EndMinigame(bool isSuccess)
    {
        if (targetFish == null) return;

        LeanGameObjectPool pool = HelperFunction.GetFishPool(targetFish);

        if (isSuccess)
        {
            Debug.Log("FishPoint value: " + targetFish.FishPoint);
            GlobalManager.Instance.fishPoints += targetFish.FishPoint;
            collection.FishCategorizedCollection(targetFish);
        }

        pool.Capacity--;
        spawner.RandomAddCapacity();

        pool.Despawn(targetFish.gameObject);
        spawner.RespawnFish(targetFish.fishType);

        hook.ResetHookPosition();

        isMinigame = false;
        targetFish = null;

        if (minigameArea != null)
            Destroy(minigameArea);
    }
    #endregion

    #region Debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        if (isMinigame && targetFish != null)
            Gizmos.DrawWireCube(minigameArea.transform.position, minigameAreaPrefab.GetComponent<SpriteRenderer>().bounds.size);
    }
    #endregion
}
