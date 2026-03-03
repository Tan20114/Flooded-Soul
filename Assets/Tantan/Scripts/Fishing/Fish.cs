using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public enum FishType
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public enum CommonFishType
{
    None = 0,
    SacabambaspisFish = 1,
    MudFish = 2,
    WavefinFish = 3,
    IcefinFish = 4,
    HydrasacleFish = 5
}

public enum UncommonFishType
{
    None = 0,
    DogFish = 1,
    FrostPetalFish = 2,
    GoofSlimeFish = 3,
}

public enum RareFishType
{
    None = 0,
    Seaturtle = 1,
    ClingFish = 2,
    RedJellyfish = 3,
}

public enum LegendaryFishType
{
    None = 0,
    PlabFish = 1,
    JollyFish = 2,
    KelpboneFish = 3
}

public class Fish : MonoBehaviour, IBoundArea
{
    [Header("References")]
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    Animator animator => GetComponent<Animator>();
    SpriteRenderer boundingArea => GameObject.FindGameObjectWithTag("FishBound").GetComponent<SpriteRenderer>();
    FishSpawner spawner => FindAnyObjectByType<FishSpawner>();
    FishingHook hook => FindAnyObjectByType<FishingHook>();

    [Header("Parameter")]
    public int id;
    public FishType fishType = FishType.Common;
    public CommonFishType commonFishType = CommonFishType.None;
    public UncommonFishType uncommonFishType = UncommonFishType.None;
    public RareFishType rareFishType = RareFishType.None;
    public LegendaryFishType legendaryFishType = LegendaryFishType.None;
    bool isSwimmingRight = false;
    public float swimSpeed = 1f;
    float currentSpeed = 0;
    public float speedDownRatio = .75f;
    public int fishPoint = 1;
    bool isClicked = false;
    public float resistanceForce = 1f;
    public float fishVisionRange = 2f;
    public int FishPoint { get => fishPoint; }
    float swimDir = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = swimSpeed;
        RandomFish();
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
        rb.linearVelocity = new Vector2(swimDir * (HookInFishVision ? currentSpeed * 1.5f : currentSpeed), rb.linearVelocity.y);

        // Vertical Swimming Movement
        if (!FishingManager.Instance.isMinigame) return;
        if (this != FishingManager.Instance.TargetFish) return;

        if (!isClicked)
            rb.linearVelocity = rb.linearVelocity = new Vector2(rb.linearVelocity.x, -currentSpeed / 2);
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

        #region Fish Clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = HelperFunction.GetWorldMouse();
            Collider2D fish = Physics2D.OverlapCircle(mouseWorldPos, .2f, LayerMask.GetMask("Fish"));

            if (fish && fish.gameObject == FishingManager.Instance.TargetFish.gameObject)
            {
                Debug.Log(fish.gameObject.name);
                if (isClicked) return;

                isClicked = true;

                hook.HookUpAnim();
                SFXManager.instance.PlaySoundFXClip(hook.hookUpSFX);

                currentSpeed *= .75f;

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, (hook.DragUpForce - resistanceForce));
                HelperFunction.Delay(this, .5f, () => isClicked = false);

                swimDir *= Random.Range(0, 100) > 80 ? 1 : -1;
                isSwimmingRight = swimDir > 0 ? true : false;
            }
        }
        #endregion

        #region Fishing Condition
        #region Catch Success Condition
        if (transform.position.y > FishingManager.Instance.fishCatchLine.position.y)
        {
            Debug.Log("Success");
            isClicked = false;
            currentSpeed = swimSpeed;

            FishingManager.Instance.EndMinigame(true);
        }
        #endregion
        #region Catch Fail Condition
        else if (transform.position.x > minigameArea.transform.position.x + halfAreaWidth - halfFishWidth || transform.position.x < minigameArea.transform.position.x - halfAreaWidth + halfFishWidth)
        {
            Debug.Log("Fail");
            isClicked = false;
            currentSpeed = swimSpeed;

            FishingManager.Instance.EndMinigame(false);
        }
        #endregion
        #endregion
    }

    void RandomDirection()
    {
        swimDir = Random.Range(-1f, 1f) < 0.5f ? -1f : 1f;
        isSwimmingRight = swimDir > 0 ? true : false;
    }

    void RandomFish()
    {
        switch (fishType)
        {
            case FishType.Common:
                List<int> commonPity = new List<int>() { 1, 2 };
                commonPity.Add(GlobalManager.Instance.CurrentBiome switch
                {
                    BiomeType.Ice => (int)CommonFishType.IcefinFish,
                    BiomeType.Ocean => (int)CommonFishType.WavefinFish,
                    BiomeType.Forest => (int)CommonFishType.HydrasacleFish,
                    _ => (int)uncommonFishType
                });

                commonFishType = (CommonFishType)commonPity[Random.Range(0,3)];

                id = GetFishTypeID(fishType) + (int)commonFishType;
                break;
            case FishType.Uncommon:
                uncommonFishType = GlobalManager.Instance.CurrentBiome switch
                {
                    BiomeType.Ice => UncommonFishType.FrostPetalFish,
                    BiomeType.Ocean => UncommonFishType.DogFish,
                    BiomeType.Forest => UncommonFishType.GoofSlimeFish,
                    _ => UncommonFishType.DogFish
                };

                id = GetFishTypeID(fishType) + (int)uncommonFishType;
                break;
            case FishType.Rare:
                rareFishType = GlobalManager.Instance.CurrentBiome switch
                {
                    BiomeType.Ocean => RareFishType.Seaturtle,
                    BiomeType.Ice => RareFishType.ClingFish,
                    BiomeType.Forest => RareFishType.RedJellyfish,
                    _ => RareFishType.Seaturtle
                };

                id = GetFishTypeID(fishType) + (int)rareFishType;
                break;
            case FishType.Legendary:
                legendaryFishType = GlobalManager.Instance.CurrentBiome switch
                {
                    BiomeType.Ocean => LegendaryFishType.PlabFish,
                    BiomeType.Ice => LegendaryFishType.JollyFish,
                    BiomeType.Forest => LegendaryFishType.KelpboneFish,
                    _ => LegendaryFishType.PlabFish
                };

                id = GetFishTypeID(fishType) + (int)legendaryFishType;
                break;
        }

        animator.SetInteger("ID", id);
    }

    int GetFishTypeID(FishType type)
    {
        int id = type switch
        {
            FishType.Common => 0,
            FishType.Uncommon => 5,
            FishType.Rare => 8,
            FishType.Legendary => 11,
            _ => 0
        };

        return id;
    }

    public void ResetFish()
    {
        RandomFish();
        currentSpeed = 0;
        HelperFunction.Delay(this, 3f, () => currentSpeed = swimSpeed);
    }

    public void FishEscape() => animator.SetTrigger("Escape");

    bool HookInFishVision => Physics2D.Linecast(transform.position, new Vector2(transform.position.x + (isSwimmingRight ? fishVisionRange : -fishVisionRange), transform.position.y), LayerMask.GetMask("Hook"));

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
        Gizmos.color = Color.orange;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (isSwimmingRight ? fishVisionRange : -fishVisionRange), transform.position.y));
    }
}