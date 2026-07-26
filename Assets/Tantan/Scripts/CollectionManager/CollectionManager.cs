using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System.Collections.Generic;
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

    public void Save()
    {
        string commonFishJson = JsonConvert.SerializeObject(commonFishCollection);
        string uncommonFishJson = JsonConvert.SerializeObject(uncommonFishCollection);
        string rareFishJson = JsonConvert.SerializeObject(rareFishCollection);
        string legendaryFishJson = JsonConvert.SerializeObject(legendaryFishCollection);

        PlayerPrefs.SetString("CommonFishCollection", commonFishJson);
        PlayerPrefs.SetString("UncommonFishCollection", uncommonFishJson);
        PlayerPrefs.SetString("RareFishCollection", rareFishJson);
        PlayerPrefs.SetString("LegendaryFishCollection", legendaryFishJson);
    }

    public void Load()
    {
        string commonFishJson = PlayerPrefs.GetString("CommonFishCollection");
        string uncommonFishJson = PlayerPrefs.GetString("UncommonFishCollection");
        string rareFishJson = PlayerPrefs.GetString("RareFishCollection");
        string legendaryFishJson = PlayerPrefs.GetString("LegendaryFishCollection");

        if (!string.IsNullOrEmpty(commonFishJson))
            commonFishCollection = JsonConvert.DeserializeObject<SerializedDictionary<CommonFishType, int>>(commonFishJson);

        if (!string.IsNullOrEmpty(uncommonFishJson))
            uncommonFishCollection = JsonConvert.DeserializeObject<SerializedDictionary<UncommonFishType, int>>(uncommonFishJson);

        if (!string.IsNullOrEmpty(rareFishJson))
            rareFishCollection = JsonConvert.DeserializeObject<SerializedDictionary<RareFishType, int>>(rareFishJson);

        if (!string.IsNullOrEmpty(legendaryFishJson))
            legendaryFishCollection = JsonConvert.DeserializeObject<SerializedDictionary<LegendaryFishType, int>>(legendaryFishJson);
    }

    public SerializedDictionary<CommonFishType, int> GetCommonFishCollection() => commonFishCollection;
    public SerializedDictionary<UncommonFishType, int> GetUncommonFishCollection() => uncommonFishCollection;
    public SerializedDictionary<RareFishType, int> GetRareFishCollection() => rareFishCollection;
    public SerializedDictionary<LegendaryFishType, int> GetLegendaryFishCollection() => legendaryFishCollection;

    public int TotalCommonFish()
    {
        int total = 0;
        foreach (KeyValuePair<CommonFishType, int> kvp in commonFishCollection)
        {
            total += kvp.Value;
        }
        return total;
    }

    public int TotalUncommonFish()
    {
        int total = 0;
        foreach (KeyValuePair<UncommonFishType, int> kvp in uncommonFishCollection)
        {
            total += kvp.Value;
        }
        return total;
    }

    public int TotalRareFish()
    {
        int total = 0;
        foreach (KeyValuePair<RareFishType, int> kvp in rareFishCollection)
        {
            total += kvp.Value;
        }
        return total;
    }

    public int TotalLegendaryFish()
    {
        int total = 0;
        foreach (KeyValuePair<LegendaryFishType, int> kvp in legendaryFishCollection)
        {
            total += kvp.Value;
        }
        return total;
    }

    public int TotalFish() => TotalCommonFish() + TotalUncommonFish() + TotalRareFish() + TotalLegendaryFish();
}