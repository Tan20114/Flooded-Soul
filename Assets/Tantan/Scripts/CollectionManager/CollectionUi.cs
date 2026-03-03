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

    [Header("Collection Page")]
    [SerializeField] Sprite[] fishingPages;
    [SerializeField] Sprite[] catPages;
    [SerializeField] Sprite[] storyPages;
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
        set => currentFishPage = WrapIndex(value, fishingPages.Length);
    }

    int CurrentCatPage
    {
        get => currentCatPage;
        set => currentCatPage = WrapIndex(value, catPages.Length);
    }

    int CurrentStoryPage
    {
        get => currentStoryPage;
        set => currentStoryPage = WrapIndex(value, storyPages.Length);
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
        RefreshUI();
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

        RefreshUI();
    }

    void RefreshUI()
    {
        UpdateButtons();
        pageRenderer.sprite = GetCurrentPage();
    }

    void UpdateButtons()
    {
        bool shouldHideButtons = false;

        if (currentPageType == PageType.Tutorial)
        {
            shouldHideButtons = true;
        }
        else if (currentPageType == PageType.Story && !AnyStoryUnlocked())
        {
            shouldHideButtons = true;
        }

        nextPageButton.gameObject.SetActive(!shouldHideButtons);
        previousPageButton.gameObject.SetActive(!shouldHideButtons);
    }

    bool AnyStoryUnlocked()
    {
        if (storyUnlocked == null) return false;

        for (int i = 0; i < storyUnlocked.Length; i++)
        {
            if (storyUnlocked[i])
                return true;
        }

        return false;
    }

    Sprite GetCurrentPage()
    {
        switch (currentPageType)
        {
            case PageType.Fish:
                return SafeGetSprite(fishingPages, CurrentFishPage);

            case PageType.Cat:
                return SafeGetSprite(catPages, CurrentCatPage);

            case PageType.Story:
                {
                    int index = CurrentStoryPage;

                    if (index >= storyUnlocked.Length || !storyUnlocked[index])
                        return storyLockPage;

                    return SafeGetSprite(storyPages, index);
                }

            case PageType.Tutorial:
                return tutorialPage;

            default:
                return null;
        }
    }

    Sprite SafeGetSprite(Sprite[] array, int index)
    {
        if (array == null || array.Length == 0)
            return null;

        index = Mathf.Clamp(index, 0, array.Length - 1);
        return array[index];
    }

    #region Page Navigation

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

        RefreshUI();
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

        RefreshUI();
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
        RefreshUI();
    }

    public void PreviousType()
    {
        int currentType = (int)currentPageType;

        if (currentType > 0)
            currentType--;
        else
            currentType = (int)PageType.Tutorial;

        currentPageType = (PageType)currentType;
        RefreshUI();
    }

    #endregion
}