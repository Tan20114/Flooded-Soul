using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
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
    [SerializedDictionary("Story Index", "Story Description")]
    [SerializeField] SerializedDictionary<int, GameObject> storyDescriptions;
    [SerializeField] Sprite storyLockPage;
    [SerializeField] Sprite tutorialPage;

    private void Start()
    {
        currentButtonList = pageButtons[currentPageType];
        GoToType((int)currentPageType);
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
                foreach (GameObject description in storyDescriptions.Values)
                {
                    description.SetActive(false);
                }
                storyDescriptions[pageIndex].SetActive(true);
                break;
        }
    }

    #endregion

    #region Type Navigation

    public void GoToType(int type)
    {
        currentPageType = (PageType)type;

        pageRenderer.sprite = baseSprites[currentPageType];

        foreach (GameObject description in storyDescriptions.Values)
        {
            description.SetActive(false);
        }

        if (currentPageType != PageType.Tutorial)
        {
            imageShow.gameObject.SetActive(true);
            dataShow.gameObject.SetActive(true);

            imageShow.sprite = blankShowSprites[0];
            dataShow.sprite = blankShowSprites[1];
        }
        else
        {
            imageShow.gameObject.SetActive(false);
            dataShow.gameObject.SetActive(false);
        }

        currentButtonList.SetActive(false);

        if (currentPageType == PageType.Tutorial) return;

        currentButtonList = pageButtons[currentPageType];
        currentButtonList.SetActive(true);
    }
    #endregion
}