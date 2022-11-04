using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ControllerPostPorcessing : MonoBehaviour
{
    [Header("Directing Effects")]
    [SerializeField] private ChromaticAberration chromaticAberration = null;
    [SerializeField] private PostProcessVolume postProcessVolume = null;
    [SerializeField] private AmbientOcclusion ambientOcclusion = null;
    [SerializeField] private Vignette vignette = null;
    [SerializeField] private Bloom bloom = null;
    [SerializeField] private Image blackPanel = null;

    [SerializeField] private bool fadeInOut;
    private void Start() => Init();

    void Init()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out ambientOcclusion);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        // Init
        bloom.intensity.value = 40f;
        vignette.intensity.value = 1f;
        ambientOcclusion.intensity.value = 4f;
        chromaticAberration.intensity.value = 1f;
        fadeInOut = false;

        // StartDirectingEffect
        StartCoroutine(DirectingEffect_Panel());
        
    }


    IEnumerator DirectingEffect_Panel()
    {
        Color color = blackPanel.color;
        float alpha = fadeInOut ? 0f : 1f;
        float value = fadeInOut ? 0.01f : -0.01f;
        while (true)
        {
            alpha += value;
            if (blackPanel.color.a <= 0f && !fadeInOut)
            {
                // Next DirectingEffects
                StartCoroutine(DirectingEffect_Bloom());
                yield break;
            }
            if (blackPanel.color.a >= 1f && fadeInOut) yield break;

            
            color.a = alpha;
            blackPanel.color = color;
            yield return null;
        }
    }


    IEnumerator DirectingEffect_Bloom()
    {
        float value = fadeInOut ? 0.1f : -0.5f;
        Debug.Log("InBloom");
        while (true)
        {
            if (bloom.intensity.value <= 13f && !fadeInOut)
            {
                // FadeIn Next DirectingEffects
                StartCoroutine(DirectingEffect_Vignette());
                StartCoroutine(DirectingEffect_AmbientOcclusion());
                yield break;
            }
            if (bloom.intensity.value >= 30f && fadeInOut)
            {
                Debug.Log("FadeIn");
                // FadeOut Next DirectingEffects
                StartCoroutine(DirectingEffect_Vignette());
                yield break;
            }

            bloom.intensity.value += value;
            yield return null;
        }
    }

    IEnumerator DirectingEffect_Vignette()
    {
        float value = fadeInOut ? 0.001f : -0.002f;
        while (true)
        {
            if (vignette.intensity.value <= 0f && !fadeInOut)
            {
                // Fade in next directing effects
                StartCoroutine(DirectingEffect_ChromaticAberration());
                yield break;
            }
            if (vignette.intensity.value >= 1f && fadeInOut)
            {
                // FadeOut Next DirectingEffects
                StartCoroutine(DirectingEffect_Panel());
                yield break;
            }
            vignette.intensity.value += value;
            yield return null;
        }
    }

    IEnumerator DirectingEffect_AmbientOcclusion()
    {
        while (ambientOcclusion.intensity.value > 0f)
        {
            ambientOcclusion.intensity.value -= 0.05f;
            yield return null;
        }
    }

    IEnumerator DirectingEffect_ChromaticAberration()
    {
        while (true)
        {
            if (chromaticAberration.intensity.value <= 0.3f)
            {
                fadeInOut = !fadeInOut;
                StartCoroutine(FadeOut());
                yield break;
            }
            chromaticAberration.intensity.value -= 0.0005f;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");
        yield return new WaitForSecondsRealtime(24f);
        StartCoroutine(DirectingEffect_Bloom());
    }

}
