using Lean.Pool;
using System.Collections;
using UnityEngine;

public class FishingManager : Singleton<FishingManager>
{
    #region Variables
    [Header("References")]
    [SerializeField] LeanGameObjectPool rewardPool;
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

    [Header("Audio")]
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip failSound;
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

            SFXManager.instance.PlaySoundFXClip(successSound);
            FishingRewardVisual reward = rewardPool.Spawn(targetFish.transform.position, Quaternion.identity).GetComponent<FishingRewardVisual>();
            reward.Init(targetFish.FishPoint);
            rewardPool.Despawn(reward.gameObject, .75f);

            GlobalManager.Instance.fishPoints += targetFish.FishPoint;
            collection.FishCategorizedCollection(targetFish);

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
        else
        {
            targetFish.FishEscape();

            pool.Capacity--;
            spawner.RandomAddCapacity();
            spawner.RespawnFish(targetFish.fishType);

            SFXManager.instance.PlaySoundFXClip(failSound);
            hook.ResetHookPosition();

            isMinigame = false;

            if (minigameArea != null)
                Destroy(minigameArea);

            StartCoroutine(DespawnAfterEscape(targetFish, pool));

            targetFish = null;
        }
    }

    IEnumerator DespawnAfterEscape(Fish fish, LeanGameObjectPool pool)
    {
        yield return new WaitForSeconds(.26f);
        pool.Despawn(fish.gameObject);
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
