using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Status")]
    public bool autoStop = false;

    public void ToggleAutoStop() => autoStop = !autoStop;
    public void ToggleSound() => GlobalManager.Instance.isSoundOn = !GlobalManager.Instance.isSoundOn;
}