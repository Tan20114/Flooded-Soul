using UnityEngine;

public class FishingBG : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();

    void Start() => animator.SetInteger("Biome", (int)GlobalManager.Instance.CurrentBiome);
}
