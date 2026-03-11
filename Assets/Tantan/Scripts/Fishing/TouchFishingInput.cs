using UnityEngine;

public class TouchFishingInput : IFishingInput
{
    public Vector2 GetPointerWorldPosition()
    {
        if (Input.touchCount > 0)
            return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        return Vector2.zero;
    }
}