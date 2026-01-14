using Lean.Pool;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] LeanGameObjectPool commonFishPool;
    [SerializeField] LeanGameObjectPool uncommonFishPool;
    [SerializeField] LeanGameObjectPool rareFishPool;
    [SerializeField] LeanGameObjectPool legendaryFishPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
