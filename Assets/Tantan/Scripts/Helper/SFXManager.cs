using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(soundFXObject);
        audioSource.volume = GlobalManager.Instance.isSoundOn ? 1 : 0;
        audioSource.clip = audioClip;

        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

    }
}
