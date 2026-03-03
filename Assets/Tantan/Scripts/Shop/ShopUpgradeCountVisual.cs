using UnityEngine;
using UnityEngine.UI;

enum Reference
{
    Hook,
    Boat,
    Cat1,
    Cat2,
    Cat3,
    Cat4
}

public class ShopUpgradeCountVisual : MonoBehaviour
{
    [SerializeField] Reference dataRef;
    int dataToCount => dataRef switch
    {
        Reference.Hook => GlobalManager.Instance.hookLevel,
        Reference.Boat => GlobalManager.Instance.boatLevel,
        Reference.Cat1 => GlobalManager.Instance.cat1Level,
        Reference.Cat2 => GlobalManager.Instance.cat2Level,
        Reference.Cat3 => GlobalManager.Instance.cat3Level,
        Reference.Cat4 => GlobalManager.Instance.cat4Level,
        _ => 0
    };
    [SerializeField] Image[] counter;
    [SerializeField] Sprite[] counterSprite;

    void Update()
    {
        if (dataToCount > 0)
        {
            for (int i = 0; i < dataToCount; i++)
            {
                counter[i].sprite = counterSprite[1];
            }
        }
    }
}
