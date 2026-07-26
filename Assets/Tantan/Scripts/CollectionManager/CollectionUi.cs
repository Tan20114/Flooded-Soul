using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum PageType
{
    Fish = 1,
    Cat = 2,
    Story = 3,
    Tutorial = 4
}

public class CollectionUi : MonoBehaviour
{
    [Header("Reference")]
    CollectionManager cm => FindAnyObjectByType<CollectionManager>();
    [SerializeField] PageType currentPageType;
    [SerializeField] Image pageRenderer;
    [SerializeField] GameObject currentButtonList;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button previousPageButton;
    [SerializeField] Image imageShow;
    [SerializeField] Image dataShow;

    [Header("Collection Page")]
    [SerializedDictionary("Page Type", "Page Sprites")]
    [SerializeField] SerializedDictionary<PageType, Sprite> baseSprites;
    [SerializedDictionary("Page Type", "Button List")]
    [SerializeField] SerializedDictionary<PageType, GameObject> pageButtons;
    [SerializeField] Sprite[] blankShowSprites;
    [SerializedDictionary("Fish Index", "Fish Sprites")]
    [SerializeField] SerializedDictionary<int, List<Sprite>> fishingPages;
    [SerializedDictionary("Cat Index", "Cat Sprites")]
    [SerializeField] SerializedDictionary<int, List<Sprite>> catPages;
    [SerializedDictionary("Story Index", "Story Sprites")]
    [SerializeField] SerializedDictionary<int, List<Sprite>> storyPages;
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
        currentButtonList = pageButtons[currentPageType];
        GoToType((int)currentPageType);
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

    public void GoToPage(string index)
    {
        string[] parts = index.Split('|');
        foreach (string part in parts)
            Debug.Log(part);
        currentPageType = (PageType)int.Parse(parts[0]);
        int pageIndex = int.Parse(parts[1]);

        switch (currentPageType)
        {
            case PageType.Fish:
                imageShow.sprite = fishingPages[pageIndex][0];
                dataShow.sprite = fishingPages[pageIndex][1];
                break;
            case PageType.Cat:
                imageShow.sprite = catPages[pageIndex][0];
                dataShow.sprite = catPages[pageIndex][1];
                break;
            case PageType.Story:
                imageShow.sprite = storyPages[pageIndex][0];
                dataShow.sprite = storyPages[pageIndex][1];
                break;
        }
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

    public void PreviousPage()
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

    public void GoToType(int type)
    {
        currentPageType = (PageType)type;

        pageRenderer.sprite = baseSprites[currentPageType];

        imageShow.sprite = blankShowSprites[0];
        dataShow.sprite = blankShowSprites[1];

        currentButtonList.SetActive(false);

        if (currentPageType == PageType.Tutorial) return;

        currentButtonList = pageButtons[currentPageType];
        currentButtonList.SetActive(true);
    }

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