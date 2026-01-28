using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializedDictionary("Fish Species", "Fish Caught")]
    public SerializedDictionary<CommonFishType, int> commonFishCollection;
    [SerializedDictionary("Fish Species", "Fish Caught")]
    public SerializedDictionary<UncommonFishType, int> uncommonFishCollection;
    [SerializedDictionary("Fish Species", "Fish Caught")]
    public SerializedDictionary<RareFishType, int> rareFishCollection;
    [SerializedDictionary("Fish Species", "Fish Caught")]
    public SerializedDictionary<LegendaryFishType, int> legendaryFishCollection;

    public void AddFishToCollection(CommonFishType fish) => commonFishCollection[fish]++;
    public void AddFishToCollection(UncommonFishType fish) => uncommonFishCollection[fish]++;
    public void AddFishToCollection(RareFishType fish) => rareFishCollection[fish]++;
    public void AddFishToCollection(LegendaryFishType fish) => legendaryFishCollection[fish]++;
}