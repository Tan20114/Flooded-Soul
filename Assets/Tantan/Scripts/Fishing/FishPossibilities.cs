using UnityEngine;

public class FishPossibilities : MonoBehaviour
{
    [Header("Fish Possibilities")]
    [Range(0, 100)] public float common = 60;
    public float uncommon = 30;
    public float rare = 8;
    public float legendary = 2;

    public float CommonMax => common;
    public float UncommonMax => common + uncommon;
    public float RareMax => common + uncommon + rare;
}
