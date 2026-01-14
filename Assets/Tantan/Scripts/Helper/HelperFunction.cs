using UnityEngine;

public abstract class HelperFunction : MonoBehaviour
{
    public static Vector3 GetWorldMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        return mouseWorldPos;
    }
}
