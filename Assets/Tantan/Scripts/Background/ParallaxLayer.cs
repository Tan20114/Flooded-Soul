using UnityEngine;

public enum LayerType
{
    Sky,
    Layer1,
    Layer2,
    Layer3,
    Layer4,
    Layer5,
    Wave,
    UnderWater,
    shop
}

public class ParallaxLayer : MonoBehaviour
{
    protected BoatController player => FindAnyObjectByType<BoatController>();

    protected ParallaxManager pm;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header("Properties")]
    [SerializeField] LayerType layerType;
    [SerializeField] float parallaxFactor = 0.5f;
    float smooth = 2f;
    protected float speed => pm.Speed * parallaxFactor;

    private void Awake()
    {
        pm = FindAnyObjectByType<ParallaxManager>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        HelperFunction.Delay(this, .01f, () =>
        {
            RandomBiomeLayer();
            rb.linearVelocity = new Vector2(-speed, 0);
        });
    }

    protected virtual void OnEnable() => ParallaxManager.OnBiomeChanged += HandleBiomeChange;

    private void OnDisable() => ParallaxManager.OnBiomeChanged -= HandleBiomeChange;

    void HandleBiomeChange(BiomeContainer biome) => RandomBiomeLayer();

    protected virtual void Update()
    {
        MovementControl();

        RegenLayer();
    }

    protected virtual void RegenLayer()
    {
        if (transform.position.x <= -pm.RegenPoint.position.x)
        {
            transform.position = new Vector2(pm.RegenPoint.position.x, transform.position.y);
            RandomBiomeLayer();
        }
    }

    protected void MovementControl()
    {
        float targetSpeed;

        if (layerType == LayerType.Sky)
            targetSpeed = player.state == BoatState.Moving ? -speed : -speed * .2f;
        else if ((GlobalManager.Instance.CurrentBiome == BiomeType.Ocean || GlobalManager.Instance.CurrentBiome == BiomeType.Forest) && layerType == LayerType.Layer5)
            targetSpeed = player.state == BoatState.Moving ? -speed : -speed * .3f;
        else
            targetSpeed = player.state == BoatState.Moving ? -speed : 0f;

        float newX = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, Time.deltaTime * smooth);

        rb.linearVelocity = new Vector2(newX, 0f);
    }

    protected virtual void RandomBiomeLayer()
    {
        sr.sprite = layerType switch
        {
            LayerType.Sky => pm.CurrentBiomeAsset.layerSky,
            LayerType.Layer1 => pm.CurrentBiomeAsset.layer1[Random.Range(0, pm.CurrentBiomeAsset.layer1.Length)],
            LayerType.Layer2 => pm.CurrentBiomeAsset.layer2[Random.Range(0, pm.CurrentBiomeAsset.layer2.Length)],
            LayerType.Layer3 => pm.CurrentBiomeAsset.layer3[Random.Range(0, pm.CurrentBiomeAsset.layer3.Length)],
            LayerType.Layer4 => pm.CurrentBiomeAsset.layer4[Random.Range(0, pm.CurrentBiomeAsset.layer4.Length)],
            LayerType.Layer5 => pm.CurrentBiomeAsset.layer5[Random.Range(0, pm.CurrentBiomeAsset.layer5.Length)],
            LayerType.Wave => pm.CurrentBiomeAsset.layerWave,
            LayerType.UnderWater => pm.CurrentBiomeAsset.underWater,
            _ => null
        };
    }
}