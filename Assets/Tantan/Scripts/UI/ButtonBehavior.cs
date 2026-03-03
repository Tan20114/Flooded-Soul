using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator textBoxAnimator;
    [SerializeField] int animationIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        textBoxAnimator.SetInteger("id", animationIndex);
        textBoxAnimator.SetBool("isHover", true);
        Debug.Log($"Pointer Entered : {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textBoxAnimator.SetInteger("id", 0);
        textBoxAnimator.SetBool("isHover", false);
        Debug.Log($"Pointer Exit : {gameObject.name}");
    }
}
