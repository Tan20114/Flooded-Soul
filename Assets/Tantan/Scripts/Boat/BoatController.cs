using System.Linq;
using UnityEngine;

public enum BoatState
{
    Idle,
    Moving
}

public class BoatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LayerMask shopMask;
    [SerializeField] Vector2 colliderSize;
    [SerializeField] Vector2 colliderOffset;
    [SerializeField] GameObject[] boatVisual;
    [SerializeField] Animator boatAnimator;
    [SerializeField] Animator[] waveAnimator;

    [Header("Parameter")]
    public BoatState state = BoatState.Moving;
    [SerializeField] int boatLevel = 1;
    bool shopEntered = false;
    public int BoatLevel
    {
        get => boatLevel;
        set
        {
            if (value == boatLevel) return;

            boatLevel = Mathf.Clamp(value, 1, 3);
            OnBoatLevelChanged();
        }
    }

    public void Sail()
    {
        shopEntered = true;
        state = BoatState.Moving;
    }

    public void Stop() => state = BoatState.Idle;

    public void ToggleAutoStop() => GameManager.Instance.autoStop = !GameManager.Instance.autoStop;

    private void Start() => OnBoatLevelChanged();

    private void Update()
    {
        boatAnimator.SetBool("isStop", state == BoatState.Moving? false : true);
        waveAnimator[boatLevel - 1].SetBool("isStop", state == BoatState.Moving? false : true);
        ShopCollide();
    }

    void ShopCollide()
    {
        if (Physics2D.OverlapBox(colliderOffset,colliderSize, 0, shopMask))
        {
            if (state == BoatState.Moving && GameManager.Instance.autoStop && !shopEntered)
            {
                Stop();
            }
        }
    }

    private void OnBoatLevelChanged()
    {
        waveAnimator[boatLevel - 1].SetInteger("level", boatLevel);

        switch (boatLevel)
        {
            case 1:
                colliderSize = new Vector2(2f, 1f);
                colliderOffset = new Vector2(-5, -4);
                break;
            case 2:
                colliderSize = new Vector2(3, 1);
                colliderOffset = new Vector2(-4.5f, -4);
                break;
            case 3:
                colliderSize = new Vector2(4, 1);
                colliderOffset = new Vector2(-4, -4);
                break;
        }

        for (int i = 0; i < boatVisual.Length; i++)
        {
            boatVisual[i].SetActive(i == boatLevel - 1);
        }
    }

    public void UpgradeBoat()
    {
        BoatLevel++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireCube(colliderOffset, colliderSize);
    }
}
