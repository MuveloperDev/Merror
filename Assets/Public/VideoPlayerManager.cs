using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static VideoPlayerManager;
using MyLibrary;

public class VideoPlayerManager : MonoBehaviour
{
    [Serializable]
    public class VideoClips
    {
        [SerializeField] private Chapter1 chapter1 = new Chapter1();

        public Chapter1 getChapter1 { get { return chapter1; } }

        [Serializable]
        public class Chapter1
        {
            [SerializeField] private VideoClip op = null;
            [SerializeField] private VideoClip deathVideo = null;
            [SerializeField] private VideoClip endVideo = null;
            [SerializeField] private VideoClip isabelRoomVideo = null;
            [SerializeField] private VideoClip demoEndVideo = null;

            public VideoClip OP { get { return op; } }
            public VideoClip DeathVideo { get { return deathVideo; } }
            public VideoClip EndVideo { get { return endVideo; } }
            public VideoClip IsabelRoomVideo { get { return isabelRoomVideo; } }
            public VideoClip DemoEndVideo { get { return demoEndVideo; } }
        }
    }

    [SerializeField] private VideoClips videoClips = new VideoClips();
    public VideoClips getVideoClips { get { return videoClips; } }
    [SerializeField] private VideoPlayer videoPlayer = null;

    private void Start() => Init();
    void Init() => videoPlayer = GetComponent<VideoPlayer>();



    public void CallPlayVideo(VideoClip clip, Action func) => StartCoroutine(PlayVideo(clip, func));
    public void CallPlayVideo(VideoClip clip, Action func, float time) => StartCoroutine(WaitForPlayVideo(clip, func, time));

    IEnumerator WaitForPlayVideo(VideoClip clip, Action func, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        StartCoroutine(PlayVideo(clip, func));
    }

    Action videoFunc = null;
    IEnumerator PlayVideo(VideoClip clip, Action func)
    {
        if (videoPlayer.targetCamera != Camera.main) videoPlayer.targetCamera = Camera.main;
        videoPlayer.clip = clip;

        videoPlayer.loopPointReached += (VideoPlayer vp) =>
        {
            TimeControl.Play();
            videoPlayer.Stop();
            
            func();
        };
        videoFunc = func;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared == true);
        GameObject fadeInOutPanel = GameObject.Find("FadeInOutPanel");
        //Del & Add Aim UI 
        if (fadeInOutPanel != null)
            fadeInOutPanel.SetActive(false);
        TimeControl.Pause();
        videoPlayer.Play();
    }


    public void SkipVideo()
    {
        if (!GameManager.Instance.isFirstPlay && Input.GetKeyDown(KeyCode.Escape) && videoPlayer.isPlaying)
        {
            Debug.Log("SkipVideo");
            StopCoroutine("PlayVideo");
            TimeControl.Play();
            videoPlayer.Stop();
            videoFunc();
            videoFunc = null;
        }
    }
}
