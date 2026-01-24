using Lean.Pool;
using Unity.VisualScripting;
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
    FishSpawner spawner => FindAnyObjectByType<FishSpawner>();
    FishingHook hook => FindAnyObjectByType<FishingHook>();

    [Header("Parameter")]
    public FishType fishType = FishType.Common;
    bool isSwimmingRight = false;
    [SerializeField] float swimSpeed = 1f;
    float ogSpeed = 0;
    [SerializeField] int fishPoint = 1;
    [SerializeField] bool isClicked = false;
    [SerializeField] float resistanceForce = 1f;
    [SerializeField] float fishVisionRange = 2f;
    public int FishPoint { get; }
    float swimDir = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ogSpeed = swimSpeed;
        RandomDirection();
        rb.linearVelocity = Vector2.right * swimSpeed * swimDir;
    }

    // Update is called once per frame
    void Update()
    {
        MoveRestriction(boundingArea);
        MinigameRestriction();
    }

    private void FixedUpdate()
    {
        // Horizontal Swimming Movement
        rb.linearVelocity = new Vector2(swimDir * (HookInFishVision ? swimSpeed * 1.5f : swimSpeed), rb.linearVelocity.y);

        // Vertical Swimming Movement
        if (!FishingManager.Instance.isMinigame) return;
        if (this != FishingManager.Instance.TargetFish) return;

        if (!isClicked)
            rb.linearVelocity = rb.linearVelocity = new Vector2(rb.linearVelocity.x, -swimSpeed / 2);
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
        #endregion

        transform.position = pos;
        sr.flipX = isSwimmingRight;
    }

    void MinigameRestriction()
    {
        if (!FishingManager.Instance.isMinigame) return;
        if (this != FishingManager.Instance.TargetFish) return;

        SpriteRenderer minigameArea = FishingManager.Instance.MinigameArea.GetComponent<SpriteRenderer>();
        LeanGameObjectPool pool = HelperFunction.GetFishPool(this);

        float halfAreaWidth = minigameArea.bounds.size.x / 2;
        float halfAreaHeight = minigameArea.bounds.size.y / 2;

        float halfFishWidth = sr.bounds.size.x / 2;
        float halfFishHeight = sr.bounds.size.y / 2;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = HelperFunction.GetWorldMouse();
            Collider2D fish = Physics2D.OverlapCircle(mouseWorldPos, .2f, LayerMask.GetMask("Fish"));

            if (fish && fish.gameObject == FishingManager.Instance.TargetFish.gameObject)
            {
                Debug.Log(fish.gameObject.name);
                if (isClicked) return;

                isClicked = true;

                swimSpeed /= 1.25f;

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, (hook.DragUpForce - resistanceForce));
                HelperFunction.Delay(this, .5f, () => isClicked = false);

                swimDir *= Random.Range(0, 100) > 80 ? 1 : -1;
                isSwimmingRight = swimDir > 0 ? true : false;
            }
        }

        if (transform.position.x > minigameArea.transform.position.x + halfAreaWidth - halfFishWidth || transform.position.x < minigameArea.transform.position.x - halfAreaWidth + halfFishWidth)
        {
            pool.Despawn(this.gameObject);
            spawner.RespawnFish(pool, fishType);

            isClicked = false;
            swimSpeed = ogSpeed;

            FishingManager.Instance.EndMinigame(false);
        }

        if (transform.position.y > FishingManager.Instance.fishCatchLine.position.y)
        {
            pool.Despawn(this.gameObject);
            spawner.RespawnFish(pool, fishType);

            isClicked = false;
            swimSpeed = ogSpeed;

            FishingManager.Instance.EndMinigame(true);
        }
    }

    void RandomDirection()
    {
        swimDir = Random.Range(-1f, 1f) < 0.5f ? -1f : 1f;
        isSwimmingRight = swimDir > 0 ? true : false;
    }

    bool HookInFishVision => Physics2D.Linecast(transform.position, new Vector2(transform.position.x + (isSwimmingRight ? fishVisionRange : -fishVisionRange), transform.position.y), LayerMask.GetMask("Hook"));

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(HelperFunction.GetWorldMouse(), .2f);
        Gizmos.color = Color.orange;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (isSwimmingRight ? fishVisionRange : -fishVisionRange), transform.position.y));
    }
}
