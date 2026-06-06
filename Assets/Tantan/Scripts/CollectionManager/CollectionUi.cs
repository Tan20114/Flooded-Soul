using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum PageType
{
    Fish,
    Cat,
    Story,
    Tutorial
}

public class CollectionUi : MonoBehaviour
{
    [Header("Reference")]
    CollectionManager cm => FindAnyObjectByType<CollectionManager>();
    [SerializeField] Image pageRenderer;
    [SerializeField] PageType currentPageType;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button previousPageButton;
    [SerializeField] Image imageShow;
    [SerializeField] Image dataShow;

    [Header("Collection Page")]
    [SerializedDictionary("Fish Index", "Fish Sprites")]
    [SerializeField] SerializedDictionary<int, List<Sprite>> fishingPages;
    [SerializeField] List<Sprite> catPages;
    [SerializeField] List<Sprite> storyPages;
    [SerializeField] Sprite storyLockPage;
    [SerializeField] Sprite tutorialPage;

    [Header("Story Unlock State")]
    [SerializeField] bool[] storyUnlocked;

    int currentFishPage;
    int currentCatPage;
    int currentStoryPage;

    #region Page Index Wrapping

    int CurrentFishPage
    {
        get => currentFishPage;
        set => currentFishPage = WrapIndex(value, fishingPages.Count);
    }

    int CurrentCatPage
    {
        get => currentCatPage;
        set => currentCatPage = WrapIndex(value, catPages.Count);
    }

    int CurrentStoryPage
    {
        get => currentStoryPage;
        set => currentStoryPage = WrapIndex(value, storyPages.Count);
    }

    int WrapIndex(int value, int length)
    {
        if (length == 0) return 0;

        if (value < 0)
            return length - 1;
        if (value >= length)
            return 0;

        return value;
    }

    #endregion

    private void Start()
    {
        CheckStoryCondition();
    }

    private void Update()
    {
        CheckStoryCondition();
    }

    // Call this manually if collection updates
    public void CheckStoryCondition()
    {
        if (cm == null) return;

        // Story 1 unlock condition
        bool l1 = cm.legendaryFishCollection[LegendaryFishType.PlabFish] > 0;
        bool l2 = cm.legendaryFishCollection[LegendaryFishType.JollyFish] > 0;
        bool l3 = cm.legendaryFishCollection[LegendaryFishType.KelpboneFish] > 0;

        if (l1 && l2 && l3)
            storyUnlocked[0] = true;

        // Story 2 unlock condition
        if (cm.commonFishCollection[CommonFishType.SacabambaspisFish] >= 25)
            storyUnlocked[1] = true;
    }

    #region Page Navigation

    public void GoToPage(int index)
    {
        imageShow.sprite = fishingPages[index][0];
        dataShow.sprite = fishingPages[index][1];
    }

    public void ResetButtonUI()
    {

    }

    public void NextPage()
    {
        switch (currentPageType)
        {
            case PageType.Fish:
                CurrentFishPage++;
                break;

            case PageType.Cat:
                CurrentCatPage++;
                break;

            case PageType.Story:
                CurrentStoryPage++;
                break;
        }

    }

    public void PreviosPage()
    {
        switch (currentPageType)
        {
            case PageType.Fish:
                CurrentFishPage--;
                break;

            case PageType.Cat:
                CurrentCatPage--;
                break;

            case PageType.Story:
                CurrentStoryPage--;
                break;
        }
    }

    #endregion

    #region Type Navigation

    public void NextType()
    {
        int currentType = (int)currentPageType;

        if (currentType < (int)PageType.Tutorial)
            currentType++;
        else
            currentType = 0;

        currentPageType = (PageType)currentType;
    }

    public void PreviousType()
    {
        int currentType = (int)currentPageType;

        if (currentType > 0)
            currentType--;
        else
            currentType = (int)PageType.Tutorial;

        currentPageType = (PageType)currentType;
    }

    #endregion
}