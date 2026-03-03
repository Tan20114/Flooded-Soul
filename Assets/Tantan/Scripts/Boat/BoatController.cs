using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum BoatState
{
    Idle,
    Moving
}

public class BoatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UpgradeData upgradeData;
    [SerializeField] LayerMask shopMask;
    [SerializeField] Vector2 colliderSize;
    [SerializeField] Vector2 colliderOffset;
    [SerializeField] Vector2 shopColliderSize = Vector2.one;
    [SerializeField] Vector2 shopColliderOffset = new Vector2(-5.5f,-4f);
    [SerializeField] GameObject[] boatVisual;
    [SerializeField] Animator boatAnimator;
    [SerializeField] Animator[] waveAnimator;
    Animator ghostAnimator;
    Animator cat1Animator;
    Animator cat2Animator;
    Animator cat3Animator;
    Animator cat4Animator;

    [Header("Parameter")]
    public BoatState state = BoatState.Moving;
    [SerializeField] float boatSpeed = 2f;
    bool shopEntered = false;
    public bool isInShopArea = false;

    public void Sail()
    {
        shopEntered = true;
        state = BoatState.Moving;
    }

    public void Stop() => state = BoatState.Idle;

    public void ToggleAutoStop() => GameManager.Instance.autoStop = !GameManager.Instance.autoStop;

    private void Update()
    {
        StopVisualize();
        GetCatAnimator();
        CatVisualize();
        Travel();
        BoatLevelVisualize();
        ShopCollide();
    }

    void ShopCollide()
    {
        isInShopArea = Physics2D.OverlapBox(colliderOffset, colliderSize, 0, shopMask);
        bool isInShopAutoStop = Physics2D.OverlapBox(shopColliderOffset, shopColliderSize, 0, shopMask);

        if (isInShopAutoStop)
        {
            if (state == BoatState.Moving && GameManager.Instance.autoStop && !shopEntered)
            {
                Stop();
            }
        }
        else
            shopEntered = false;
    }

    private void BoatLevelVisualize()
    {
        switch (GlobalManager.Instance.boatLevel)
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
            boatVisual[i].SetActive(i == GlobalManager.Instance.boatLevel - 1);
        }
    }

    void Travel()
    {
        if (state == BoatState.Idle) return;

        GlobalManager.Instance.distance += upgradeData.boatSpeed[GlobalManager.Instance.boatLevel - 1] * Time.deltaTime;
    }

    void StopVisualize()
    {
        boatAnimator.SetBool("isStop", state == BoatState.Moving ? false : true);
        boatVisual[GlobalManager.Instance.boatLevel - 1].GetComponent<BoatData>().ghostAnimator.SetBool("isStop", state == BoatState.Moving ? false : true);
        waveAnimator[GlobalManager.Instance.boatLevel - 1].SetBool("isStop", state == BoatState.Moving ? false : true);
    }

    void GetCatAnimator()
    {
        cat1Animator = boatVisual[GlobalManager.Instance.boatLevel - 1].GetComponent<BoatData>().cat1Animator;
        cat2Animator = boatVisual[GlobalManager.Instance.boatLevel - 1].GetComponent<BoatData>().cat2Animator;
        cat3Animator = boatVisual[GlobalManager.Instance.boatLevel - 1].GetComponent<BoatData>().cat3Animator;
        cat4Animator = boatVisual[GlobalManager.Instance.boatLevel - 1].GetComponent<BoatData>().cat4Animator;
    }

    void CatVisualize()
    {
        UpdateCat(cat1Animator, GlobalManager.Instance.cat1Level);
        UpdateCat(cat2Animator, GlobalManager.Instance.cat2Level);
        UpdateCat(cat3Animator, GlobalManager.Instance.cat3Level);
        UpdateCat(cat4Animator, GlobalManager.Instance.cat4Level);
    }

    void UpdateCat(Animator animator, int level)
    {
        if (animator == null) return;

        bool isActive = level > 0;
        animator.gameObject.SetActive(isActive);

        if (isActive)
            animator.SetInteger("level", level);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireCube(colliderOffset, colliderSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(shopColliderOffset, shopColliderSize);
    }
}
