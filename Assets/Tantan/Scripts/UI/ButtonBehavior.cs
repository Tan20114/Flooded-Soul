using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;

    [SerializeField] Animator textBoxAnimator;
    [SerializeField] int animationIndex;
    [SerializeField] GameObject buttonLock;
    [SerializeField] bool isUnlocked = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        if (!buttonLock) return;

        button.image.color = isUnlocked ? Color.white : Color.black;
        button.interactable = isUnlocked;
        buttonLock.SetActive(!isUnlocked);
    }

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

    public void Unlock() => isUnlocked = true;
}
