using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [Header("Status")]
    public bool isMinigame = false;

    [Header("Objects")]
    [SerializeField] GameObject minigameAreaPrefab;
    Fish targetFish;
    public Fish TargetFish
    {
        get => targetFish;
    }
    GameObject minigameArea;
    public GameObject MinigameArea
    {
        get => minigameArea;
        private set => minigameArea = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMinigame(Fish target)
    {
        isMinigame = true;
        targetFish = target;

        minigameArea = Instantiate(minigameAreaPrefab, targetFish.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        if (isMinigame && targetFish != null)
            Gizmos.DrawWireCube(minigameArea.transform.position, minigameAreaPrefab.GetComponent<SpriteRenderer>().bounds.size);
    }
}
