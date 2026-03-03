using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CostManager costData;

    #region Boat
    int maxBoatLevel = 3;

    public int BoatCostUpdate() => costData.boatCost[GlobalManager.Instance.boatLevel - 1];
    #endregion

    #region Hook
    int maxHookLevel = 4;

    public int HookCostUpdate() => costData.hookCost[GlobalManager.Instance.hookLevel - 1];
    #endregion

    #region Cat
    int maxCatLevel = 3;

    public int Cat1CostUpdate() => costData.cat1Cost[GlobalManager.Instance.cat1Level];
    public int Cat2CostUpdate() => costData.cat2Cost[GlobalManager.Instance.cat2Level];
    public int Cat3CostUpdate() => costData.cat3Cost[GlobalManager.Instance.cat3Level];
    public int Cat4CostUpdate() => costData.cat4Cost[GlobalManager.Instance.cat4Level];
    #endregion

    public void UpgradeBoat()
    {
        if (GlobalManager.Instance.boatLevel < maxBoatLevel)
        {
            if (GlobalManager.Instance.fishPoints >= BoatCostUpdate())
            {
                GlobalManager.Instance.fishPoints -= BoatCostUpdate();
                GlobalManager.Instance.boatLevel++;
            }
        }
    }

    public void UpgradeHook()
    {         
        if (GlobalManager.Instance.hookLevel < maxHookLevel)
        {
            if (GlobalManager.Instance.fishPoints >= HookCostUpdate())
            {
                GlobalManager.Instance.fishPoints -= HookCostUpdate();
                GlobalManager.Instance.hookLevel++;
            }
        }
    }

    public void UpgradeCat(int catNumber)
    {
        switch (catNumber)
        {
            case 1:
                if (GlobalManager.Instance.cat1Level < maxCatLevel)
                {
                    if (GlobalManager.Instance.fishPoints >= Cat1CostUpdate())
                    {
                        GlobalManager.Instance.fishPoints -= Cat1CostUpdate();
                        GlobalManager.Instance.cat1Level++;
                    }
                }
                break;
            case 2:
                if (GlobalManager.Instance.cat2Level < maxCatLevel && GlobalManager.Instance.boatLevel > 1)
                {
                    if (GlobalManager.Instance.fishPoints >= Cat2CostUpdate())
                    {
                        GlobalManager.Instance.fishPoints -= Cat2CostUpdate();
                        GlobalManager.Instance.cat2Level++;
                    }
                }
                break;
            case 3:
                if (GlobalManager.Instance.cat3Level < maxCatLevel && GlobalManager.Instance.boatLevel > 2)
                {
                    if (GlobalManager.Instance.fishPoints >= Cat3CostUpdate())
                    {
                        GlobalManager.Instance.fishPoints -= Cat3CostUpdate();
                        GlobalManager.Instance.cat3Level++;
                    }
                }
                break;
            case 4:
                if (GlobalManager.Instance.cat4Level < maxCatLevel && GlobalManager.Instance.boatLevel > 2)
                {
                    if (GlobalManager.Instance.fishPoints >= Cat4CostUpdate())
                    {
                        GlobalManager.Instance.fishPoints -= Cat4CostUpdate();
                        GlobalManager.Instance.cat4Level++;
                    }
                }
                break;
        }
    }
}
