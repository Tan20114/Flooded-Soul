using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Biome/Biome Data")]
public class BiomeContainer : ScriptableObject
{
    public Sprite[] layer1;
    public Sprite[] layer2;
    public Sprite[] layer3;
    public Sprite[] layer4;
    public Sprite[] layer5;
    public Sprite layerSky;
    public Sprite layerWave;
    public Sprite underWater;
}
