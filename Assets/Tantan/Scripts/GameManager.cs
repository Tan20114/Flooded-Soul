using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Status")]
    public bool autoStop = true;

    public void ToggleSound() => GlobalManager.Instance.isSoundOn = !GlobalManager.Instance.isSoundOn;
}