using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameManager : Singleton<GameManager>
{
    private Player MyPlayer = null;
    public Player GetPlayer() { return MyPlayer; }
    public void FindPlayer() { MyPlayer = FindObjectOfType<Player>(); }

    private CinemachineVirtualCamera PlayerCam = null;
    public CinemachineVirtualCamera GetPlayerCamera() { return PlayerCam; }
    public void FindPlayerCamera()
    {
        if (MyPlayer != null)
        {
            if(MyPlayer.gameObject.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
            {
                PlayerCam = cam;
                return;
            }
            else Debug.LogError("Find Player Camera : Failed to get component of player virtual camera");
            return;
        }
        Debug.Log("Find Player Camera : My Player is null ptr");
    }

    private void Start()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void OnSceneChanged(Scene previous, Scene current)
    {
        if (current.isLoaded)
        {
            switch(current.name)
            {
                default: break;
                case "Chapter1": { InitChapter(1); break; }
                case "Chapter2": { InitChapter(2); break; }
            }
        }
    }
    private void InitChapter(int chapterNum)
    {
        FindPlayer();
        FindPlayerCamera();
        Puzzle.Instance.InitPuzzle(chapterNum);
    }
}
