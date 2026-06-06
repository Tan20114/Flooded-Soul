using UnityEngine;
using UnityEngine.UI;

public class CollectionPageGridResize : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Canvas canvas;
    GridLayoutGroup grid => GetComponent<GridLayoutGroup>();
    RectTransform canvasRT;

    [Header("Properties")]
    float cellX,cellY;
    [SerializeField] [Range(0, 1)] float cellXPercent = .05f;
    [SerializeField] [Range(0, 1)] float cellYPercent = .1139f;
    [SerializeField] [Range(0, 1)] float paddingLeftPercent = .1139f;
    [SerializeField] [Range(0, 1)] float paddingTopPercent = .05f;

    private void Awake()
    {
        canvasRT = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cellX = canvasRT.sizeDelta.x * cellXPercent;
        cellY = canvasRT.rect.height * cellYPercent;
        grid.cellSize = new Vector2(cellX, cellY);
        grid.padding.left = (int)(canvasRT.sizeDelta.x * paddingLeftPercent);
        grid.padding.top = (int)(canvasRT.sizeDelta.y * paddingTopPercent);
    }
}