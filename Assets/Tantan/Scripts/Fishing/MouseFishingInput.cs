using UnityEngine;

public class MouseFishingInput : IFishingInput
{
    public Vector2 GetPointerWorldPosition() => HelperFunction.GetWorldMouse();
}