using UnityEngine;

public class TouchFishingInput : IFishingInput
{
    public Vector2 GetPointerWorldPosition() => Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
}