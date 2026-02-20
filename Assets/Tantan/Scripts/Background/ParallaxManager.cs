using System;
using UnityEngine;

public enum BiomeType
{
    Ocean = 0,
    Forest = 2,
    Ice = 1
}

public class ParallaxManager : MonoBehaviour
{
    public static event Action<BiomeContainer> OnBiomeChanged;

    [Header("Reference")]
    [SerializeField] Transform regenPoint;
    public Transform RegenPoint { get => regenPoint; }
    [SerializeField] BiomeContainer[] biomeList;

    [Header("Properties")]
    BiomeContainer currentBiome;
    public BiomeContainer CurrentBiomeAsset { get => currentBiome; }
    [SerializeField]int currentBiomeIndex = 0;
    public int CurrentBiomeIndex
    {
        get => currentBiomeIndex;
        set => currentBiomeIndex = Mathf.Clamp(value,0,2);
    }
    [Range(0,5)]
    [SerializeField] float speed = 1.0f;
    public float Speed { get => speed; }
    [HideInInspector] public bool isBiomeChange = false;

    void Awake() => currentBiome = biomeList[CurrentBiomeIndex];

    private void Start() => SetBiome();

    private void Update() => ChangeBiome();

    void ChangeBiome()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            CurrentBiomeIndex--;
            ApplyBiome();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            CurrentBiomeIndex++;
            ApplyBiome();
        }
    }

    void ApplyBiome()
    {
        currentBiome = biomeList[CurrentBiomeIndex];
        SetBiome();
        OnBiomeChanged?.Invoke(currentBiome);
    }

    void SetBiome() => GameManager.Instance.CurrentBiome = (BiomeType)currentBiomeIndex;
}