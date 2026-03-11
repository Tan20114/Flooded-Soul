using UnityEngine;
using System.Collections;

public class CatFishing : MonoBehaviour
{
    [Header("Generate part")]
    [SerializeField] UpgradeData upgradeData;
    [SerializeField] GameObject rewardPrefab;
    [SerializeField] Vector2[] cat1Pos;
    [SerializeField] Vector2[] cat2Pos;
    [SerializeField] Vector2 cat3Pos;
    [SerializeField] Vector2 cat4Pos;

    [Header("Audio")]
    [SerializeField] AudioClip catGenerateSFX;

    int Cat1Level
    {
        get => GlobalManager.Instance.cat1Level;
        set
        {
            GlobalManager.Instance.cat1Level = Mathf.Clamp(value, 0, 4);
        }
    }

    int Cat2Level
    {
        get => GlobalManager.Instance.cat2Level;
        set
        {
            GlobalManager.Instance.cat2Level = Mathf.Clamp(value, 0, 4);
        }
    }

    int Cat3Level
    {
        get => GlobalManager.Instance.cat3Level;
        set
        {
            GlobalManager.Instance.cat3Level = Mathf.Clamp(value, 0, 4);
        }
    }

    int Cat4Level
    {
        get => GlobalManager.Instance.cat4Level;
        set
        {
            GlobalManager.Instance.cat4Level = Mathf.Clamp(value, 0, 4);
        }
    }

    private void Start()
    {
        StartCoroutine(CatLoop(Cat1Generate, () => upgradeData.cat1Possibilities[Cat1Level].interval));
        StartCoroutine(CatLoop(Cat2Generate, () => upgradeData.cat2Possibilities[Cat2Level].interval));
        StartCoroutine(CatLoop(Cat3Generate, () => upgradeData.cat3Possibilities[Cat3Level].interval));
        StartCoroutine(CatLoop(Cat4Generate, () => upgradeData.cat4Possibilities[Cat4Level].interval));
    }

    IEnumerator CatLoop(System.Action generateMethod, System.Func<float> getInterval)
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= getInterval())
            {
                timer = 0f;
                generateMethod.Invoke();
            }

            yield return null;
        }
    }

    int GenerateCatPoint(int percent1,int percent2,int percent3,int percent4)
    {
        int total = percent1 + percent2 + percent3 + percent4;

        int roll = Random.Range(1, total);

        if (roll <= percent1)
            return 1;
        else if (roll <= percent1 + percent2)
            return 2;
        else if (roll <= percent1 + percent2 + percent3)
            return 3;
        else
            return 4;
    }

    void Cat1Generate()
    {
        if (GlobalManager.Instance.cat1Level < 1) return;

        CatLevelData cat1Data = upgradeData.cat1Possibilities[GlobalManager.Instance.cat1Level - 1];

        int point = GenerateCatPoint(cat1Data.value1,cat1Data.value2,cat1Data.value3,cat1Data.value4);

        if (GlobalManager.Instance.currentScene == 0)
        {
            SFXManager.instance.PlaySoundFXClip(catGenerateSFX);
            FishingRewardVisual reward = Lean.Pool.LeanPool.Spawn(rewardPrefab, cat1Pos[GlobalManager.Instance.boatLevel - 1],Quaternion.identity).GetComponent<FishingRewardVisual>();
            reward.Init(point);
            HelperFunction.Delay(this, 1f, () => Lean.Pool.LeanPool.Despawn(reward.gameObject, .75f));
        }
        Debug.Log("Cat1 generated: " + point);

        GlobalManager.Instance.fishPoints += point;
    }

    void Cat2Generate()
    {
        if (GlobalManager.Instance.cat2Level < 1 || GlobalManager.Instance.boatLevel < 2) return;

        CatLevelData cat2Data = upgradeData.cat2Possibilities[GlobalManager.Instance.cat2Level - 1];

        int point = GenerateCatPoint(cat2Data.value1, cat2Data.value2, cat2Data.value3, cat2Data.value4);

        if (GlobalManager.Instance.currentScene == 0)
        {
            SFXManager.instance.PlaySoundFXClip(catGenerateSFX);
            FishingRewardVisual reward = Lean.Pool.LeanPool.Spawn(rewardPrefab, cat2Pos[GlobalManager.Instance.boatLevel - 2], Quaternion.identity).GetComponent<FishingRewardVisual>();
            reward.Init(point);
            HelperFunction.Delay(this, 1f, () => Lean.Pool.LeanPool.Despawn(reward.gameObject, .75f));
        }

        Debug.Log("Cat2 generated: " + point);

        GlobalManager.Instance.fishPoints += point;
    }

    void Cat3Generate()
    {
        if (GlobalManager.Instance.cat3Level < 1 || GlobalManager.Instance.boatLevel < 3) return;

        CatLevelData cat3Data = upgradeData.cat3Possibilities[GlobalManager.Instance.cat3Level - 1];

        int point = GenerateCatPoint(cat3Data.value1, cat3Data.value2, cat3Data.value3, cat3Data.value4);

        if (GlobalManager.Instance.currentScene == 0)
        {
            SFXManager.instance.PlaySoundFXClip(catGenerateSFX);
            FishingRewardVisual reward = Lean.Pool.LeanPool.Spawn(rewardPrefab, cat3Pos, Quaternion.identity).GetComponent<FishingRewardVisual>();
            reward.Init(point);
            HelperFunction.Delay(this, 1f, () => Lean.Pool.LeanPool.Despawn(reward.gameObject, .75f));
        }

        Debug.Log("Cat3 generated: " + point);

        GlobalManager.Instance.fishPoints += point;
    }

    void Cat4Generate()
    {
        if (GlobalManager.Instance.cat4Level < 1 || GlobalManager.Instance.boatLevel < 3) return;

        CatLevelData cat4Data = upgradeData.cat4Possibilities[GlobalManager.Instance.cat4Level - 1];

        int point = GenerateCatPoint(cat4Data.value1, cat4Data.value2, cat4Data.value3, cat4Data.value4);

        if (GlobalManager.Instance.currentScene == 0)
        {
            SFXManager.instance.PlaySoundFXClip(catGenerateSFX);
            FishingRewardVisual reward = Lean.Pool.LeanPool.Spawn(rewardPrefab, cat4Pos, Quaternion.identity).GetComponent<FishingRewardVisual>();
            reward.Init(point);
            HelperFunction.Delay(this, 1f, () => Lean.Pool.LeanPool.Despawn(reward.gameObject, .75f));
        }

        Debug.Log("Cat4 generated: " + point);

        GlobalManager.Instance.fishPoints += point;
    }
}
