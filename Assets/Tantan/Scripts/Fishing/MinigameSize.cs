using UnityEngine;

public class MinigameSize : MonoBehaviour
{
    [SerializeField] UpgradeData upgradeData;

    void Update()
    {
        transform.localScale = new Vector2(upgradeData.minigameRadius[GlobalManager.Instance.hookLevel - 1],transform.localScale.y);
    }
}
