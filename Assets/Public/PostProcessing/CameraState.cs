using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class CameraState : MonoBehaviour
{
    public enum CamState
    {
        NONE,
        PANIC,
    }

    [Header("Directing Effects")]
    [SerializeField] private ChromaticAberration chromaticAberration = null;
    [SerializeField] private PostProcessVolume postProcessVolume = null;
    [SerializeField] private AmbientOcclusion ambientOcclusion = null;
    [SerializeField] private Vignette vignette = null;
    [SerializeField] private Bloom bloom = null;
    [SerializeField] private LensDistortion lensDistortion = null;
    [SerializeField] private DepthOfField depthOfField = null;


    CamState prevState = CamState.NONE;

    float time = 0f;
    private void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out ambientOcclusion);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out lensDistortion);
        postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    void Start() => Init();


    void Init()
    {


        ambientOcclusion.intensity.value = 0.5f;
        vignette.intensity.value = 0.5f;
        bloom.intensity.value = 20.06f;
        lensDistortion.intensity.value = 17f;
        chromaticAberration.intensity.value = 0f;
        depthOfField.focusDistance.value = 77f;
        time = 0f;

    }


    public void TurnOnState(CamState STATE)
    {
        Debug.Log("TurnOnState");

        // 상태해제 테스트용
        if (prevState == STATE)
        {
            TurnOffState();
            return;
        }
        else if (prevState != CamState.NONE)
        {
            TurnOffState();
        }
        prevState = STATE;
        StartCoroutine(STATE.ToString() + "_STATE");
    }


    void TurnOffState()
    {
        Debug.Log("TurnOffState");
        StopCoroutine(prevState.ToString() + "_STATE");
        prevState = CamState.NONE;
        Init();
    }


    IEnumerator PANIC_STATE()
    {
        Debug.Log("PANIC");

        while (true)
        {
            yield return null;
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
}
