using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Biome/Biome Data")]
public class BiomeContainer : ScriptableObject
{
    public AnimationClip[] layer1;
    public AnimationClip[] layer2;
    public AnimationClip[] layer3;
    public AnimationClip[] layer4;
    public AnimationClip[] layer5;
    public Sprite layerSky;
    public Sprite layerWave;
    public Sprite underWater;
    public Sprite shop;
    public AudioClip bgm;
}
