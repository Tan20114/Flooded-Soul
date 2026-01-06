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
            LayerType.Sky => pm.CurrentBiome.layerSky,
            LayerType.Layer1 => pm.CurrentBiome.layer1[Random.Range(0, pm.CurrentBiome.layer1.Length)],
            LayerType.Layer2 => pm.CurrentBiome.layer2[Random.Range(0, pm.CurrentBiome.layer2.Length)],
            LayerType.Layer3 => pm.CurrentBiome.layer3[Random.Range(0, pm.CurrentBiome.layer3.Length)],
            LayerType.Layer4 => pm.CurrentBiome.layer4[Random.Range(0, pm.CurrentBiome.layer4.Length)],
            LayerType.Layer5 => pm.CurrentBiome.layer5[Random.Range(0, pm.CurrentBiome.layer5.Length)],
            LayerType.Wave => pm.CurrentBiome.layerWave,
            LayerType.UnderWater => pm.CurrentBiome.underWater,
            _ => null
        };
    }
}
