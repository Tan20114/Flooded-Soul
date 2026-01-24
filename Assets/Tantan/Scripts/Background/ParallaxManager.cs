using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform regenPoint;
    public Transform RegenPoint { get => regenPoint; }
    [SerializeField] BiomeContainer[] biomeList;
    BiomeContainer currentBiome;
    public BiomeContainer CurrentBiome { get => currentBiome; }

    [Header("Properties")]
    [Range(0,5)]
    [SerializeField] float speed = 1.0f;
    public float Speed { get => speed; }

    void Awake()
    {
        currentBiome = biomeList[0];
    }
}