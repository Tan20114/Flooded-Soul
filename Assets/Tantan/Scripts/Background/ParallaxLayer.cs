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
    UnderWater
}

public class ParallaxLayer : MonoBehaviour
{
    ParallaxManager pm;
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Properties")]
    [SerializeField] LayerType layerType;
    [SerializeField] float parallaxFactor = 0.5f;
    float speed => pm.Speed * parallaxFactor;

    private void Awake()
    {
        pm = FindAnyObjectByType<ParallaxManager>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector2(-speed, 0);
        RandomBiomeLayer(); 
    }

    private void OnEnable() => ParallaxManager.OnBiomeChanged += HandleBiomeChange;

    private void OnDisable() => ParallaxManager.OnBiomeChanged -= HandleBiomeChange;

    void HandleBiomeChange(BiomeContainer biome) => RandomBiomeLayer();

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= -pm.RegenPoint.position.x)
        {
            transform.position = new Vector2(pm.RegenPoint.position.x, transform.position.y);
            RandomBiomeLayer();
        }
    }

    void RandomBiomeLayer()
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
