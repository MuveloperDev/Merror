using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CameraState : MonoBehaviour
{
    public enum CamState
    {
        NONE,
        PANIC,
        FADEIN,
        FADEOUT,
        DEATH,
    }

    [Header("Directing Effects")]
    [SerializeField] private ChromaticAberration chromaticAberration = null;
    [SerializeField] private PostProcessVolume postProcessVolume = null;
    [SerializeField] private AmbientOcclusion ambientOcclusion = null;
    [SerializeField] private Vignette vignette = null;
    [SerializeField] private Bloom bloom = null;
    [SerializeField] private LensDistortion lensDistortion = null;
    [SerializeField] private DepthOfField depthOfField = null;
    [SerializeField] private ColorGrading colorGrading = null;

    [Header("FadeIn&Out")]
    [SerializeField] private Image fadeInOutPanel = null;
    [SerializeField] private bool fadeInOut = false;
    [SerializeField] private bool isProcess = false;
    [SerializeField] CamState prevState = CamState.NONE;

    float time = 0f;

    // CallBackFunction After FadeOut
    public Action DoFadeOutState;


    private void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out ambientOcclusion);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out lensDistortion);
        postProcessVolume.profile.TryGetSettings(out depthOfField);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    void Start() => Init();


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //    TurnOnState(CamState.FADEIN);
        //if (Input.GetKeyDown(KeyCode.J))
        //    TurnOnState(CamState.FADEOUT);
        //if (Input.GetKeyDown(KeyCode.K))
        //    TurnOnState(CamState.DEATH);
    }
    void Init()
    {
        ambientOcclusion.intensity.value = 0.5f;
        vignette.intensity.value = 0.5f;
        bloom.intensity.value = 20.06f;
        lensDistortion.intensity.value = 17f;
        chromaticAberration.intensity.value = 0f;
        depthOfField.focusDistance.value = 2.3f;
        colorGrading.colorFilter.value = Color.white;
        bloom.color.value = Color.white;
        vignette.color.value = Color.black;
        time = 0f;
        fadeInOutPanel.gameObject.SetActive(false);
    }


    public void TurnOnState(CamState STATE)
    {
        if (isProcess)
        {
            Debug.Log("Processing Camera State by MH");
            return;
        }

        isProcess = true;

        Debug.Log("TurnOnState");

        // 상태해제 테스트용
        if (prevState == STATE)
        {
            TurnOffState();
            return;
        }
        prevState = STATE;
        StartCoroutine(STATE.ToString() + "_STATE");
    }

    public void TurnOffState()
    {
        if (prevState == CamState.FADEOUT && DoFadeOutState != null) DoFadeOutState();

        Debug.Log("TurnOffState");
        StopCoroutine(prevState.ToString() + "_STATE");
        prevState = CamState.NONE;
        Init();

        isProcess = false;
    }


    IEnumerator PANIC_STATE()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Clamp(time * 5f, 0.1f,0.8f);
            lensDistortion.intensity.value = Mathf.Clamp(time * 30f, 0f, 80f);
            chromaticAberration.intensity.value = Mathf.Clamp(time * 5f, 0.1f, 1f);
            bloom.intensity.value = Mathf.Clamp(time * 30f, 0f, 60f);
            depthOfField.focusDistance.value = Mathf.Clamp(-time * 0.01f, 0f, 60f);

            if (lensDistortion.intensity.value >= 80f)
            {
                yield return new WaitForSeconds(3f);
                break;
            }    
        }
        TurnOffState();
    }

    IEnumerator DEATH_STATE()
    {
        yield return new WaitForFixedUpdate();
        Color color = Color.white;
        vignette.color.value = Color.red;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            color.g = color.b -= 0.005f;
            colorGrading.colorFilter.value = color;
            bloom.color.value = color;
            if (color.g <= 0)
            {
                break;
            }
        }
        StartCoroutine(FADEOUT_STATE());

    }

    IEnumerator FADEIN_STATE()
    {
        fadeInOut = false;
        PanelSetColor(1f);
        fadeInOutPanel.gameObject.SetActive(true);
        vignette.intensity.value = 1f;
        StartCoroutine(DirectingEffect_Panel());
        yield return new WaitForFixedUpdate();

    }
    IEnumerator FADEOUT_STATE()
    {
        fadeInOutPanel.gameObject.SetActive(true);
        PanelSetColor(0f);
        fadeInOut = true;
        StartCoroutine(DirectingEffect_Bloom());
        yield return new WaitForFixedUpdate();
    }




    #region Fade In & Out
    IEnumerator DirectingEffect_Panel()
    {
        Color color = fadeInOutPanel.color;
        float alpha = fadeInOut ? 0f : 1f;
        float value = fadeInOut ? 0.005f : -0.005f;
        while (true)
        {
            alpha += value;
            if (fadeInOutPanel.color.a <= 0f && !fadeInOut)
            {
                // Next DirectingEffects
                StartCoroutine(DirectingEffect_Bloom());
                yield break;
            }
            if (fadeInOutPanel.color.a >= 1f && fadeInOut)
            {
                fadeInOut = !fadeInOut;
                TurnOffState();
                yield break;
            }


            color.a = alpha;
            fadeInOutPanel.color = color;
            yield return new WaitForFixedUpdate(); 
        }
    }


    IEnumerator DirectingEffect_Bloom()
    {
        float value = fadeInOut ? 0.5f : -0.5f;
        bloom.intensity.value = fadeInOut ? 20f : 35f;
        while (true)
        {
            if (bloom.intensity.value <= 13f && !fadeInOut)
            {
                // FadeIn Next DirectingEffects
                StartCoroutine(DirectingEffect_Vignette());
                StartCoroutine(DirectingEffect_AmbientOcclusion());
                yield break;
            }
            if (bloom.intensity.value >= 40f && fadeInOut)
            {
                // FadeOut Next DirectingEffects
                StartCoroutine(DirectingEffect_Vignette());
                yield break;
            }

            bloom.intensity.value += value;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DirectingEffect_Vignette()
    {
        float value = fadeInOut ? 0.01f : -0.01f;
        
        while (true)
        {
            if (vignette.intensity.value <= 0.52f && !fadeInOut)
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
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DirectingEffect_AmbientOcclusion()
    {
        while (ambientOcclusion.intensity.value > 0.5f)
        {
            ambientOcclusion.intensity.value -= 0.05f;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DirectingEffect_ChromaticAberration()
    {
        while (true)
        {
            if (chromaticAberration.intensity.value <= 0.3f)
            {
                fadeInOut = !fadeInOut;
                fadeInOutPanel.gameObject.SetActive(false);
                TurnOffState();
                yield break;
            }
            chromaticAberration.intensity.value -= 0.0005f;
            yield return new WaitForFixedUpdate();
        }
    }

    void PanelSetColor(float value)
    {
        Color color = fadeInOutPanel.color;
        color.a = value;
        fadeInOutPanel.color = color;
    }
    #endregion
}
