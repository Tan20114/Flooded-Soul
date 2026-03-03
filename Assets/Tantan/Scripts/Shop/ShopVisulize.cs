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

        hookCostText.text = $"Cost : {sm.HookCostUpdate()} fish";
        boatCostText.text = $"Cost : {sm.BoatCostUpdate()} fish";
        cat1CostText.text = $"Cost : {sm.Cat1CostUpdate()} fish";
        cat2CostText.text = $"Cost : {sm.Cat2CostUpdate()} fish";
        cat3CostText.text = $"Cost : {sm.Cat3CostUpdate()} fish";
        cat4CostText.text = $"Cost : {sm.Cat4CostUpdate()} fish";
    }
}
