using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CostManager costData;

    #region Boat
    int maxBoatLevel = 3;

    public int BoatCostUpdate() => (GlobalManager.Instance.boatLevel < maxBoatLevel) ? costData.boatCost[GlobalManager.Instance.boatLevel - 1] : 0;
    #endregion

    #region Hook
    int maxHookLevel = 4;

    public int HookCostUpdate() => (GlobalManager.Instance.hookLevel < maxHookLevel) ? costData.hookCost[GlobalManager.Instance.hookLevel - 1] : 0;
    #endregion

    #region Cat
    int maxCatLevel = 4;

    public int Cat1CostUpdate() => (GlobalManager.Instance.cat1Level < maxCatLevel) ? costData.cat1Cost[GlobalManager.Instance.cat1Level] : 0;
    public int Cat2CostUpdate() => (GlobalManager.Instance.cat2Level < maxCatLevel) ? costData.cat2Cost[GlobalManager.Instance.cat2Level] : 0;
    public int Cat3CostUpdate() => (GlobalManager.Instance.cat3Level < maxCatLevel) ? costData.cat3Cost[GlobalManager.Instance.cat3Level] : 0;
    public int Cat4CostUpdate() => (GlobalManager.Instance.cat4Level < maxCatLevel) ? costData.cat4Cost[GlobalManager.Instance.cat4Level] : 0;
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
