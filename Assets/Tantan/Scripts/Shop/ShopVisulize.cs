using TMPro;
using UnityEngine;

public class ShopVisulize : MonoBehaviour
{
    ShopManager sm => FindAnyObjectByType<ShopManager>();

    [Header("Currency")]
    [SerializeField] TextMeshProUGUI fishPointTxt;

    [Header("Cost")]
    [SerializeField] TextMeshProUGUI hookCostText; 
    [SerializeField] TextMeshProUGUI boatCostText;
    [SerializeField] TextMeshProUGUI cat1CostText;
    [SerializeField] TextMeshProUGUI cat2CostText;
    [SerializeField] TextMeshProUGUI cat3CostText;
    [SerializeField] TextMeshProUGUI cat4CostText;

    private void LateUpdate()
    {
        fishPointTxt.text = GlobalManager.Instance.fishPoints.ToString("0000");

#if UNITY_ANDROID || UNITY_IOS
        hookCostText.text = $"{sm.HookCostUpdate()}";
        boatCostText.text = $"{sm.BoatCostUpdate()}";
        cat1CostText.text = $"{sm.Cat1CostUpdate()}";
        cat2CostText.text = $"{sm.Cat2CostUpdate()}";
        cat3CostText.text = $"{sm.Cat3CostUpdate()}";
        cat4CostText.text = $"{sm.Cat4CostUpdate()}";
#else
        hookCostText.text = $"Cost : {sm.HookCostUpdate()} fish";
        boatCostText.text = $"Cost : {sm.BoatCostUpdate()} fish";
        cat1CostText.text = $"Cost : {sm.Cat1CostUpdate()} fish";
        cat2CostText.text = $"Cost : {sm.Cat2CostUpdate()} fish";
        cat3CostText.text = $"Cost : {sm.Cat3CostUpdate()} fish";
        cat4CostText.text = $"Cost : {sm.Cat4CostUpdate()} fish";
#endif
    }
}
