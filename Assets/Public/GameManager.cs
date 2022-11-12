using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering;
using EPOOutline;
using UnityEditor;
using MyLibrary;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Video;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        InitAudioManager();
        InitUIManager();
        InitPuzzleManager();
        //InitCutScenePlayer();
        SceneManager.activeSceneChanged -= OnSceneChanged;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void Update()
    {
        GameInput.UpdateKey();
    }
    #region Check current scene and initialize something work
    private void OnSceneChanged(Scene previous, Scene current) // Called when scene changed
    {
        if (current.isLoaded)
        {
            switch (current.name)
            {
                default: { ToggleCursor(false); break; }
                case "Chapter1": { InitChapter(1); break; }
                case "Chapter2": { InitChapter(2); break; }
                case "Chapter1_KYM": { InitChapter(1); break; } // test
            }
        }
    }
    /// <summary>
    /// Initialize chapter information by chapter number
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    private void InitChapter(int chapterNum)
    {
        ToggleCursor(true); // Lock cursor
        FindPlayer(); // Find my player
        FindPlayerCamera(); // Find camera
        InitInventory(); // Init inventory
        _Puzzle.InitPuzzle(chapterNum); // Init puzzles
    }
    #endregion
    #region Player & Camera Management
    private Player MyPlayer = null;
    public Player GetPlayer() { return MyPlayer; }
    public void FindPlayer() { MyPlayer = FindObjectOfType<Player>(); }

    private CinemachineVirtualCamera PlayerCam = null;
    public CinemachineVirtualCamera GetPlayerCamera() { return PlayerCam; }
    public void FindPlayerCamera()
    {
        if (MyPlayer != null)
        {
            if (MyPlayer.transform.GetChild(0).TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
            {
                PlayerCam = cam;
                return;
            }
            else Debug.LogError("Find Player Camera : Failed to get component of player virtual camera");
            return;
        }
        Debug.Log("Find Player Camera : My Player is null ptr");
    }
    #endregion // Find my player and save reference and camera.
    #region Inventory Management
    private bool ShowInven = false;
    private Inventory MyInventory = null;
    public Inventory GetInventory() { return MyInventory; }
    private Canvas InventoryCanvas = null;
    private TextMeshProUGUI Notice = null;
    /// <summary>
    /// Initialize inventory function. Get notice TMP object and inventory canvas.
    /// </summary>
    public void InitInventory()
    {
        TryGetComponent<Inventory>(out Inventory inventory);
        MyInventory = inventory == null ? null : inventory;
        MyInventory ??= this.AddComponent<Inventory>();
        MyInventory.InitInventory();
    }
    /// <summary>
    /// Input key 'I', player can toggle inventory UI.
    /// </summary>
    public void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInven = !ShowInven;
            if (ShowInven)
            {
                MyInventory.ShowInventory();
                ToggleCursor(false);
            }
            else
            {
                MyInventory.HideInventory();
                ToggleCursor(true);
            }
        }
        if (ShowInven)
        {
            if (MyInventory.GetItemCount > 0)
            {
                MyInventory.RotationItem();
            }
        }
    }
    #endregion // Set inventory
    #region Audio Management
    private AudioManager _AudioManager = null;
    private void InitAudioManager() => _AudioManager = GetComponent<AudioManager>();
    public AudioManager GetAudio() => _AudioManager;
    #endregion
    #region UI Management
    private UIManager _UIManager = null;
    private void InitUIManager() => _UIManager = GetComponent<UIManager>();
    public UIManager GetUI() => _UIManager;
    #endregion
    #region Puzzle Management
    private PuzzleManager _Puzzle = null;
    private void InitPuzzleManager() => _Puzzle = GetComponent<PuzzleManager>();
    public PuzzleManager GetPuzzle() => _Puzzle;
    #endregion
    #region Cut Scene
    private VideoPlayer CutScenePlayer = null;
    public VideoClip CutSceneClip = null;
    private void InitCutScenePlayer()
    {
        CutScenePlayer = GetComponent<VideoPlayer>();
        CutScenePlayer.enabled = false;
    }
    public void PlayCutScene(int CutSceneIndex)
    {
        // GetMyClip from data
        // CutSceneClip = GetClip();
        CutScenePlayer.enabled = true;
        CutScenePlayer.targetCamera = Camera.main;
        CutScenePlayer.clip = CutSceneClip;
        if(CutSceneClip == null) { Debug.Log("There's no cut scene clip."); return; }
        CutScenePlayer.Play();
        StartCoroutine(WhenCutSceneEnd());
    }
    private IEnumerator WhenCutSceneEnd()
    {
        yield return new WaitForSecondsRealtime(1f);
        while (true)
        {
            if (CutScenePlayer.isPlaying == false)
            {
                Debug.Log("Cut scene playing finished.");
                CutScenePlayer.clip = null;
                CutScenePlayer.enabled = false;
                yield break;
            }
            yield return null;
        }
    }
    #endregion
    /// <summary>
    /// Toggle cursor lock state.
    /// </summary>
    /// <param name="value">Is cursor locked?</param>
    public void ToggleCursor(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
