using UnityEngine;
using UnityEngine.UI;

public class CollectionPageSelectionButton : MonoBehaviour
{
    CollectionManager cm;
    Button button;

    [SerializeField] int itemID;
    [SerializeField] int id;
    public Sprite[] sprites;
    [SerializeField] Image[] renderer;
    Image img;
    bool condition;

    private void Awake()
    {
        img = GetComponent<Image>();
        cm = FindAnyObjectByType<CollectionManager>();
        button = GetComponent<Button>();
    }

    public void OnClick()
    {
        Reset();
        img.sprite = sprites[1];
    }

    private void Update()
    {
        switch (itemID)
        {
            case 1:
                condition = cm.GetCommonFishCollection()[(CommonFishType)id] > 0;
                button.interactable = condition;
                img.color = condition ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
                break;
            case 2:
                condition = cm.GetUncommonFishCollection()[(UncommonFishType)id] > 0;
                button.interactable = condition;
                img.color = condition ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
                break;
            case 3:
                condition = cm.GetRareFishCollection()[(RareFishType)id] > 0;
                button.interactable = condition;
                img.color = condition ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
                break;
            case 4:
                condition = cm.GetLegendaryFishCollection()[(LegendaryFishType)id] > 0;
                button.interactable = condition;
                img.color = condition ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
                break;
            case 5:
                int level = 0;

                switch (id)
                {
                    case 1: level = GlobalManager.Instance.cat1Level; break;
                    case 2: level = GlobalManager.Instance.cat2Level; break;
                    case 3: level = GlobalManager.Instance.cat3Level; break;
                    case 4: level = GlobalManager.Instance.cat4Level; break;
                }

                condition = level > 0;
                button.interactable = condition;
                img.color = condition ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
                break;
        }
    }

    void Reset()
    {
        foreach (Image img in renderer)
        {
            img.sprite = img.GetComponent<CollectionPageSelectionButton>().sprites[0];
        }
    }
}
