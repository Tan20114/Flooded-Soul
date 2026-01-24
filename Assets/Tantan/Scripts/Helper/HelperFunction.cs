using Lean.Pool;
using System;
using System.Collections;
using UnityEngine;

public abstract class HelperFunction : MonoBehaviour
{
    public static Vector3 GetWorldMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        return mouseWorldPos;
    }

    public static LeanGameObjectPool GetFishPool(Fish fish)
    {
        return fish.fishType switch
        {
            FishType.Common => FindAnyObjectByType<FishSpawner>().commonFishPool,
            FishType.Uncommon => FindAnyObjectByType<FishSpawner>().uncommonFishPool,
            FishType.Rare => FindAnyObjectByType<FishSpawner>().rareFishPool,
            FishType.Legendary => FindAnyObjectByType<FishSpawner>().legendaryFishPool,
            _ => null,
        };
    }

    static IEnumerator TimeDelay(float time ,Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    public static void Delay(MonoBehaviour runner,float time,Action action)
    {
        runner.StartCoroutine(TimeDelay(time,action));
    }
}
