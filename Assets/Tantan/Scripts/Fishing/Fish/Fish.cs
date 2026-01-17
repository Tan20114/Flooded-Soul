using UnityEngine;

public enum FishType
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public class Fish : MonoBehaviour, IBoundArea
{
    [Header("References")]
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    SpriteRenderer boundingArea => GameObject.FindGameObjectWithTag("FishBound").GetComponent<SpriteRenderer>();
    FishingManager fm => FindAnyObjectByType<FishingManager>();

    [Header("Parameter")]
    public FishType fishType = FishType.Common;
    bool isSwimmingRight = false;
    [SerializeField] float swimSpeed = 1f;
    float swimDir = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomDirection();
        rb.linearVelocity = Vector2.right * swimSpeed * swimDir;
    }

    // Update is called once per frame
    void Update()
    {
        MoveRestriction(boundingArea);
        MinigameRestriction();
    }

    public void MoveRestriction(SpriteRenderer boundingArea)
    {
        float halfScreenWidth = boundingArea.bounds.size.x / 2;
        float halfScreenHeight = boundingArea.bounds.size.y / 2;

        float halfFishWidth = sr.bounds.size.x / 2;
        float halfFishHeight = sr.bounds.size.y / 2;

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, boundingArea.transform.position.y - halfScreenHeight + halfFishHeight, boundingArea.transform.position.y + halfScreenHeight - halfFishHeight);

        #region Horizontal Movement Restriction (Fish Only)
        if (pos.x <= boundingArea.transform.position.x - halfScreenWidth + halfFishWidth / 2)
        {
            swimDir = 1;
            isSwimmingRight = true;
        }
        else if (pos.x >= boundingArea.transform.position.x + halfScreenWidth - halfFishWidth / 2)
        {
            swimDir = -1;
            isSwimmingRight = false;
        }

        rb.linearVelocity = Vector2.right * swimSpeed * swimDir;
        sr.flipX = isSwimmingRight;
        #endregion

        transform.position = pos;
    }

    void MinigameRestriction()
    {
        if (!fm.isMinigame) return;
        if (this != fm.TargetFish) return;
        SpriteRenderer minigameArea = fm.MinigameArea.GetComponent<SpriteRenderer>();

        float halfAreaWidth = minigameArea.bounds.size.x / 2;
        float halfAreaHeight = minigameArea.bounds.size.y / 2;

        float halfFishWidth = sr.bounds.size.x / 2;
        float halfFishHeight = sr.bounds.size.y / 2;

        if (transform.position.x > minigameArea.transform.position.x + halfAreaWidth - halfFishWidth)
        {
            Debug.Log("Fish escaped!");
        }
        else if (transform.position.x < minigameArea.transform.position.x - halfAreaWidth + halfFishWidth)
        {
            Debug.Log("Fish escaped!");
        }
    }

    void RandomDirection()
    {
        swimDir = Random.Range(-1f, 1f) < 0.5f ? -1f : 1f;
        isSwimmingRight = swimDir > 0 ? true : false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}
