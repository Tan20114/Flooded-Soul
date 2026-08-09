using Lean.Pool;
using System.Collections;
using UnityEngine;

public class FishingManager : Singleton<FishingManager>
{
    #region Variables
    [Header("References")]
    SceneLoader sl => FindAnyObjectByType<SceneLoader>();
    [SerializeField] LeanGameObjectPool rewardPool;
    FishSpawner spawner => FindAnyObjectByType<FishSpawner>();
    CollectionManager collection => FindAnyObjectByType<CollectionManager>();
    FishingHook hook => FindAnyObjectByType<FishingHook>();

    [Header("Status")]
    bool isSceneOut = false;
    public bool isMinigame = false;
    [SerializeField] float maxMinigameTime = 7.5f;
    [SerializeField] float minMinigameTime = 4f;
    [SerializeField] float fastestFishSpeed = 3f;
    [SerializeField] float slowestFishSpeed = 1.0f;
    [SerializeField] float maxHookForce = 3.2f;
    [SerializeField] float minHookForce = 2.6f;
    [Range(0,1)] [SerializeField] float difficultyMultiplier = 1.0f;
    public float calculatedTime = 0;
    float currentMinigameTime = 0;
    public float CurrentMinigameTime
    {
        get => currentMinigameTime;
    }

    [Header("Objects")]
    public Transform fishCatchLine;
    [SerializeField] Fish targetFish;
    public Fish TargetFish
    {
        get => targetFish;
    }

    private void Update()
    {
        if (isMinigame && targetFish != null)
        {
            currentMinigameTime -= Time.deltaTime;
            if (currentMinigameTime <= 0)
                EndMinigame(false);
        }
        else if (!isMinigame && GlobalManager.Instance.buffDuration <= 0 && !isSceneOut)
        {
            isSceneOut = true;
            sl.ChangeScene(0);
        }
    }

    [Header("Audio")]
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip failSound;
    #endregion

    #region Minigame
    public void StartMinigame(Fish target)
    {
        calculatedTime = CatchTimeCalculate(target.swimSpeed, hook.DragUpForce);
        currentMinigameTime = calculatedTime;

        isMinigame = true;
        targetFish = target;
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

            currentMinigameTime = 0;
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

            currentMinigameTime = 0;

            StartCoroutine(DespawnAfterEscape(targetFish, pool));

            targetFish = null;
        }
    }

    float CatchTimeCalculate(float fishSpeed, float hookForce)
    {
        Debug.Log("Fish Speed : " + fishSpeed);
        Debug.Log("Hook Force : " + hookForce);

        float difficulty = fishSpeed / hookForce;
        float maxDifficulty = fastestFishSpeed / minHookForce;
        float minDifficulty = slowestFishSpeed / maxHookForce;

        float normalizedDifficulty = Mathf.InverseLerp(minDifficulty, maxDifficulty, difficulty);
        float diffcultyMutiplied = Mathf.Pow(normalizedDifficulty, 1 - difficultyMultiplier);

        return Mathf.Lerp(maxMinigameTime, minMinigameTime, diffcultyMutiplied);
    }

    IEnumerator DespawnAfterEscape(Fish fish, LeanGameObjectPool pool)
    {
        yield return new WaitForSeconds(.26f);
        pool.Despawn(fish.gameObject);
    }
    #endregion
}