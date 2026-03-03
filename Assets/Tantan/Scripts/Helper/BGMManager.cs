using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] BiomeContainer[] data;

    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] AudioSource sourceA;
    [SerializeField] AudioSource sourceB;

    AudioSource activeSource;
    AudioSource inactiveSource;

    private void OnEnable() => ParallaxManager.OnBiomeChanged += HandleBiomeChanged;

    private void OnDisable() => ParallaxManager.OnBiomeChanged -= HandleBiomeChanged;

    private void Start()
    {
        activeSource = sourceA;
        inactiveSource = sourceB;

        activeSource.clip = data[(int)GlobalManager.Instance.CurrentBiome].bgm;
        activeSource.Play();
    }

    void HandleBiomeChanged(BiomeContainer biome)
    {
        if (biome.bgm == null) return;

        StopAllCoroutines();
        StartCoroutine(Crossfade(biome.bgm));
    }

    IEnumerator Crossfade(AudioClip newClip)
    {
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, .8f, time / fadeDuration);

            if (!GlobalManager.Instance.isSoundOn)
            {
                activeSource.volume = 0f;
                inactiveSource.volume = 0f;
            }
            else
            {
                activeSource.volume = .8f - t;
                inactiveSource.volume = t;
            }

            yield return null;
        }

        activeSource.Stop();

        var temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;
    }


    private void Update() => activeSource.volume = GlobalManager.Instance.isSoundOn ? .8f : 0;
}
