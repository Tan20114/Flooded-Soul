using UnityEngine;
using UnityEngine.UI;

public class CollectionPageSelectionButton : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] Image[] renderer;
    Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void OnClick()
    {
        Reset();
        img.sprite = sprites[1];
    }

    void Reset()
    {
        foreach (Image img in renderer)
        {
            img.sprite = img.GetComponent<CollectionPageSelectionButton>().sprites[0];
        }
    }
}
