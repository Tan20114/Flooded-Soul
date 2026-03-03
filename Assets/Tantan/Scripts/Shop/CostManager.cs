using UnityEngine;

[CreateAssetMenu(fileName = "Cost", menuName = "Cost/Cost Data")]
public class CostManager : ScriptableObject
{
    [Header("Fishing and Sailing")]
    public int[] boatCost;
    public int[] hookCost;
    [Header("Cat Cost")]
    public int[] cat1Cost;
    public int[] cat2Cost;
    public int[] cat3Cost;
    public int[] cat4Cost;
}
