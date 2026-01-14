using UnityEngine;

public class FishingHook : MonoBehaviour, IBoundArea
{
    [Header("References")]
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] SpriteRenderer boundingArea;

    [Header("Parameter")]
    [SerializeField] float followSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
        MoveRestriction(boundingArea);
    }

    void FollowMouse()
    {
        Vector2 distance = transform.position - HelperFunction.GetWorldMouse();
        Vector2 dir = distance.normalized;

        rb.linearVelocity = -dir * followSpeed;
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
}
