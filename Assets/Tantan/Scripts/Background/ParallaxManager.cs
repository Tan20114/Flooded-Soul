using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BiomeType
{
    Ocean = 0,
    Ice = 1,
    Forest = 2
}

public class ParallaxManager : MonoBehaviour
{
    public static event Action<BiomeContainer> OnBiomeChanged;

    [Header("Reference")]
    [SerializeField] Transform regenPoint;
    public Transform RegenPoint { get => regenPoint; }
    [SerializeField] BiomeContainer[] biomeList;
    [SerializeField] Animator biomeTrans;

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

    #region Change Biome Condition
    [Header("Biome Changing")]
    [SerializeField] float biomeChangeDistance = 100f;
    #endregion

    private void Start()
    {
        currentBiomeIndex = (int)GlobalManager.Instance.CurrentBiome;
        currentBiome = biomeList[CurrentBiomeIndex];  
    }

    private void Update() => ChangeBiome();

    void ChangeBiome()
    {
        int currentStep = (int)GlobalManager.Instance.distance / (int)biomeChangeDistance;

        if (currentStep > GlobalManager.Instance.biomeChangeLastStep)
        {
            GlobalManager.Instance.biomeChangeLastStep = currentStep;

            int randomBiomeIndex = Random.Range(0,3);

            if (randomBiomeIndex == currentBiomeIndex) return;

            currentBiomeIndex = randomBiomeIndex;
            ApplyBiome();
        }
    }

    void ApplyBiome()
    {
        biomeTrans.SetTrigger("BiomeIn");

        HelperFunction.Delay(this, 1f, () =>
        {
            currentBiome = biomeList[CurrentBiomeIndex];
            SetBiome();
            OnBiomeChanged?.Invoke(currentBiome);
        });

        HelperFunction.Delay(this, 1,() => biomeTrans.SetTrigger("BiomeOut"));
    }

    void SetBiome() => GlobalManager.Instance.CurrentBiome = (BiomeType)currentBiomeIndex;
}