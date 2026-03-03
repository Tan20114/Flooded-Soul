using UnityEngine;

public class FishingRewardVisual : MonoBehaviour
{
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] Sprite[] sprites;
    [SerializeField] float moveSpeed = 1.5f;

    public void Init(int fishPoint)
    {
        int index = Mathf.Clamp(fishPoint - 1, 0, sprites.Length - 1);
        sr.sprite = sprites[index];
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}