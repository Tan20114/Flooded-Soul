using Lean.Pool;
using UnityEngine;

public class FishingHook : MonoBehaviour, IBoundArea
{
    [Header("References")]
    IFishingInput input;
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] UpgradeData upgradeData;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer boundingArea;
    [SerializeField] LayerMask fishLayer;
    [SerializeField] LineRenderer stringRenderer;
    [SerializeField] Transform stringStartPoint;
    [SerializeField] Transform stringEndPoint;

    [Header("Parameter")]
    [SerializeField] Vector2 startPos;
    [SerializeField] float followSpeed = 5.0f;
    [SerializeField] float dragUpForce = 5;
    Vector2 minigameStartPoint = Vector2.zero;

    [Header("Audio")]
    public AudioClip hookUpSFX;

    public float DragUpForce => dragUpForce;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        input = new TouchFishingInput();
#else
        input = new MouseFishingInput();
#endif
    }

    void Start()
    {
        startPos = transform.position;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ParameterUpdate();
        FishingString();
        MinigameStartPointSet();

        if (FishingManager.Instance.isMinigame)
            FollowFish();
        else
            FollowPointer();
        MoveRestriction(boundingArea);
    }

    private void LateUpdate()
    {
        HookVisualize();
    }

    void MinigameStartPointSet()
    {
        if(FishingManager.Instance.isMinigame && minigameStartPoint == Vector2.zero)
            minigameStartPoint = transform.position;
        else if(!FishingManager.Instance.isMinigame)
            minigameStartPoint = Vector2.zero;
    }

    void FollowPointer()
    {
        Vector2 pointerPos = input.GetPointerWorldPosition();
        Vector2 distance = (Vector2)transform.position - pointerPos;
        Vector2 dir = distance.normalized;

        if (distance.magnitude < 0.1f)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = -dir * followSpeed;
    }

    void FollowFish()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = FishingManager.Instance.TargetFish.transform.position;
    }

    public void MoveRestriction(SpriteRenderer boundingArea)
    {
        float halfScreenWidth = boundingArea.bounds.size.x / 2;
        float halfScreenHeight = boundingArea.bounds.size.y / 2;

        float halfHookWidth = sr.bounds.size.x / 2;
        float halfHookHeight = sr.bounds.size.y / 2;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, boundingArea.transform.position.x - halfScreenWidth + halfHookWidth, boundingArea.transform.position.x + halfScreenWidth - halfHookWidth);
        pos.y = Mathf.Clamp(pos.y, boundingArea.transform.position.y - halfScreenHeight + halfHookHeight, boundingArea.transform.position.y + halfScreenHeight - halfHookHeight);
        transform.position = pos;
    }

    void FishingString()
    {
        stringStartPoint.position = new Vector2(stringEndPoint.position.x, stringStartPoint.position.y);

        stringRenderer.SetPosition(0, stringStartPoint.position);
        stringRenderer.SetPosition(1, stringEndPoint.position);

        if (FishingManager.Instance.isMinigame)
        {
            stringRenderer.startColor = MinigameStringColor();
            stringRenderer.endColor = MinigameStringColor();
        }
        else
        {
            stringRenderer.startColor = Color.white;
            stringRenderer.endColor = Color.white;
        }
    }

    void ParameterUpdate()
    {
        followSpeed = upgradeData.hookSpeed[GlobalManager.Instance.hookLevel - 1];
        dragUpForce = upgradeData.hookForce[GlobalManager.Instance.hookLevel - 1];
    }

    float MinigameDistanceCheck()
    {
        float maxDistance = FishingManager.Instance.MinigameArea.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        return Vector2.Distance(transform.position, minigameStartPoint) / maxDistance;
    }

    Color MinigameStringColor() => Color.Lerp(Color.white, Color.red, MinigameDistanceCheck());

    public void ResetHookPosition() => transform.position = startPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FishingManager.Instance.isMinigame) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            FishType type = collision.GetComponent<Fish>().fishType;

            FishingManager.Instance.StartMinigame(collision.GetComponent<Fish>());
        }
    }

    void HookVisualize()
    {
        animator.SetInteger("level", GlobalManager.Instance.hookLevel - 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, sr.bounds.size);
    }

    public void HookUpAnim() => animator.SetTrigger("HookUpTrigger");
}
