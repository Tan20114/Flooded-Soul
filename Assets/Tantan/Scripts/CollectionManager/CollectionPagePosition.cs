using UnityEngine;
using UnityEngine.UI;

public class CollectionPagePosition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image parent;
    RectTransform parentRT;
    RectTransform rt => GetComponent<RectTransform>();

    [Header("Properties")]
    [SerializeField] [Range(-1f, 1f)] float xPercent = .5f;
    [SerializeField] [Range(-1f, 1f)] float yPercent = .5f;
    
    [SerializeField] [Range(0f, 1f)] float widthPercent = 0.5f;
    [SerializeField] [Range(0f, 1f)] float heightPercent = 0.5f;

    private void Awake()
    {
        parentRT = parent.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        rt.anchoredPosition = new Vector2(xPercent * parentRT.rect.width, yPercent * parentRT.rect.height);
        rt.sizeDelta = new Vector2(widthPercent * parentRT.rect.width, heightPercent * parentRT.rect.height);
    }
}