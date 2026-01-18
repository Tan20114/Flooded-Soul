using Lean.Pool;
using UnityEngine;

public class FishingHook : MonoBehaviour, IBoundArea
{
    [Header("References")]
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] SpriteRenderer boundingArea;
    [SerializeField] LayerMask fishLayer;
    FishingManager fm => FindAnyObjectByType<FishingManager>();

    [Header("Parameter")]
    [SerializeField] float followSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (fm.isMinigame)
            FollowFish();
        else
            FollowMouse();
        MoveRestriction(boundingArea);
    }

    void FollowMouse()
    {
        Vector2 distance = transform.position - HelperFunction.GetWorldMouse();
        Vector2 dir = distance.normalized;

        if(distance.magnitude < 0.1f)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = -dir * followSpeed;
    }

    void FollowFish()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = fm.TargetFish.transform.position;
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



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fm.isMinigame) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            FishType type = collision.GetComponent<Fish>().fishType;

            fm.StartMinigame(collision.GetComponent<Fish>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, sr.bounds.size);
    }
}
