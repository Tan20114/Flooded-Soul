using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform regenPoint;
    public Transform RegenPoint { get => regenPoint; }
    [SerializeField] BiomeContainer[] biomeList;

    [Header("Properties")]
    BiomeContainer currentBiome;
    public BiomeContainer CurrentBiome { get => currentBiome; }
    [SerializeField]int currentBiomeIndex = 0;
    int CurrentBiomeIndex
    {
        get 
        {
            return currentBiomeIndex;
        }
        set
        {
            currentBiomeIndex = Mathf.Clamp(value,0,2);
        }
    }
    [Range(0,5)]
    [SerializeField] float speed = 1.0f;
    public float Speed { get => speed; }
    [HideInInspector] public bool isBiomeChange = false;

    void Awake()
    {
        currentBiome = biomeList[CurrentBiomeIndex];
    }

    private void Update()
    {
        ChangeBiome();
    }

    void ChangeBiome()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            CurrentBiomeIndex--;
            isBiomeChange = true;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            CurrentBiomeIndex++;
            isBiomeChange = true;
        }

        if(isBiomeChange)
        {
            isBiomeChange = false;
            Debug.Log("BiomeChange");
            currentBiome = biomeList[CurrentBiomeIndex];
        }
    }
}
