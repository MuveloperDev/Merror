using MyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class SerializableDictionary<T1, T2> where T1 : class where T2 : class
{
    [SerializeField] private T1[] keys;
    public T1[] Keys { get { return keys; } }
    [SerializeField] private T2[] values;
    public T2[] Values { get { return values; } }

    [SerializeField] private Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>();
    public void Init()
    {
        for (int i = 0; i < keys.Length; i++) dictionary.Add(keys[i], values[i]);
    }

    public T2 GetValue(T1 key) => dictionary[key];
}
[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerManager : MonoBehaviour
{
    public enum VideoCategory
    {
        PUBLIC,
        CHAPTER1,
    }
    [SerializeField] private SerializableDictionary<string, VideoClip> clips = new SerializableDictionary<string, VideoClip>();

    [SerializeField] private VideoPlayer videoPlayer = null;

    private void Start() => Init();
    void Init()
    { 
        videoPlayer = GetComponent<VideoPlayer>();
        clips.Init();
    } 


    /// <summary>
    /// Play the video referenced in VideoClips on the main camera.
    /// </summary>
    /// <param name="clip">This is the video to be played.</param>
    /// <param name="func">Callback function to be executed at the end of the video.</param>
    public void CallPlayVideo(VideoClip clip, Action func) => StartCoroutine(PlayVideo(clip, func));
    /// <summary>
    /// Play the video referenced in VideoClips on the main camera.
    /// </summary>
    /// <param name="clip">This is the video to be played.</param>
    /// <param name="func">Callback function to be executed at the end of the video.</param>
    /// <param name="time">Delay the playback time by the argument.</param>
    public void CallPlayVideo(VideoClip clip, Action func, float time) => StartCoroutine(WaitForPlayVideo(clip, func, time));

    // Calls the PlayVideo coroutine with a delay as long as the time received as an argument.
    IEnumerator WaitForPlayVideo(VideoClip clip, Action func, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        StartCoroutine(PlayVideo(clip, func));
    }


    // VideoPlayer
    IEnumerator PlayVideo(VideoClip clip, Action func)
    {
        // if VideoPlayer tartget camera is not main camera Change videoPlayer target camera to main camera 
        if (videoPlayer.targetCamera != Camera.main) videoPlayer.targetCamera = Camera.main;

        // Change video clip
        videoPlayer.clip = clip;

        GameObject aimUI = GameObject.Find("AimUI");
        //Del & Add Aim UI 
        if (aimUI != null)
            aimUI.SetActive(false);
        GameObject fadeInOutPanel = GameObject.Find("FadeInOutPanel");
        //Del & Add fadeInOutPanel 
        if (fadeInOutPanel != null)
            fadeInOutPanel.SetActive(false);
        // Del AcquisitionNotificationSlider Before Play Video
        if (GameManager.Instance.GetUI().AcquisitionNotificationSlider.gameObject != null)
            GameManager.Instance.GetUI().AcquisitionNotificationSlider.gameObject.SetActive(false);
        // Del PostProcess Before Play Video
        GameObject postProcess = GameObject.Find("PostProcess");
        if (postProcess != null)
            postProcess.SetActive(false);

        // loopPointReached : last point of Video clip 
        videoPlayer.loopPointReached += (VideoPlayer vp) =>
        {
            videoPlayer.Stop();
            if (aimUI != null)
                aimUI.SetActive(true);
            if (GameManager.Instance.GetUI().AcquisitionNotificationSlider.gameObject != null)
                GameManager.Instance.GetUI().AcquisitionNotificationSlider.gameObject.SetActive(true);
            if (postProcess != null)
                postProcess.SetActive(true);
            TimeControl.Play();
            func();
        };
        // for skip
        videoFunc = func;

        // The video is prepared in advance to remove the video delay.
        videoPlayer.Prepare();

        // Wait until your video is ready.
        yield return new WaitUntil(() => videoPlayer.isPrepared == true);

        // timeScale is zero
        TimeControl.Pause();

        // play video
        videoPlayer.Play();
    }

    // A callback function to be called when the video ends.
    Action videoFunc = null;

    /// <summary>
    /// If it is not the first play and the video is playing, pressing the ESC button skips the video.
    /// </summary>
    public void SkipVideo()
    {
        if (!GameManager.Instance.isFirstPlay && Input.GetKeyDown(KeyCode.Escape) && videoPlayer.isPlaying)
        {
            StopCoroutine("PlayVideo");
            videoPlayer.Stop();
            if (Time.timeScale == 0) TimeControl.Play();
            videoFunc();
            videoFunc = null;
        }
    }


    /// <summary>
    /// Returns a video clip from a custom dictionary.
    /// </summary>
    /// <param name="category">An enum that defines chapters.</param>
    /// <param name="name">The name of the video for the chapter.Pass the argument without "chapter_" in front of the name</param>
    /// <returns></returns>
    public VideoClip GetClip(VideoCategory category, string name) => clips.GetValue(category.ToString() + "_" + name);
}
