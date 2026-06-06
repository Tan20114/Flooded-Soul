using UnityEngine;

public class CollectionPageResize : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    RectTransform rt => GetComponent<RectTransform>();
    RectTransform canvasRT;

    private void Awake()
    {
        canvasRT = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rt.sizeDelta = new Vector2(canvasRT.sizeDelta.x / 2, rt.sizeDelta.y);
    }
}