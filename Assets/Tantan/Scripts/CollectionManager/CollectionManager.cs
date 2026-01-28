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

    public void FishCategorizedCollection(Fish fish)
    {
        switch (fish.fishType)
        {
            case FishType.Common:
                {
                    AddFishToCollection(fish.commonFishType);
                    break;
                }
            case FishType.Uncommon:
                {
                    AddFishToCollection(fish.uncommonFishType);
                    break;
                }
            case FishType.Rare:
                {
                    AddFishToCollection(fish.rareFishType);
                    break;
                }
            case FishType.Legendary:
                {
                    AddFishToCollection(fish.legendaryFishType);
                    break;
                }
        }
    }

    void AddFishToCollection(CommonFishType fish) => commonFishCollection[fish]++;
    void AddFishToCollection(UncommonFishType fish) => uncommonFishCollection[fish]++;
    void AddFishToCollection(RareFishType fish) => rareFishCollection[fish]++;
    void AddFishToCollection(LegendaryFishType fish) => legendaryFishCollection[fish]++;
}