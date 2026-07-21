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
        input = new MouseFishingInput();
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

        float verticalMagnitude = dir.y > 0 ? 1.2f : .7f;

        Vector2 finalDir = new Vector2(dir.x, dir.y * verticalMagnitude);

        Vector2 drift = new Vector2(Mathf.Sin(Time.time * 1.5f) * 0.15f, Mathf.Cos(Time.time * 2f) * 0.1f);

        Vector3 mousePos = input.GetPointerWorldPosition();

        sr.flipX = mousePos.x < transform.position.x;

        if (distance.magnitude < 0.1f)
            rb.linearVelocity = drift;
        else
            rb.linearVelocity = (-finalDir * followSpeed) + drift;
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

    void ParameterUpdate()
    {
        followSpeed = upgradeData.hookSpeed[GlobalManager.Instance.hookLevel - 1];
        dragUpForce = upgradeData.hookForce[GlobalManager.Instance.hookLevel - 1];
    }

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
