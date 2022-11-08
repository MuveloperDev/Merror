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

public class GameManager : Singleton<GameManager>
{
    private Player MyPlayer = null;
    public Player GetPlayer() { return MyPlayer; }
    public void FindPlayer() { MyPlayer = FindObjectOfType<Player>(); }

    private CinemachineVirtualCamera PlayerCam = null;
    public CinemachineVirtualCamera GetPlayerCamera() { return PlayerCam; }

    private bool ShowInven = false;
    private Inventory MyInventory = null;
    public Inventory GetInventory() { return MyInventory; }
    [SerializeField] private Canvas InventoryCanvas = null;
    [SerializeField] private TextMeshProUGUI Notice = null;
    public void InitInventory()
    { 
        MyInventory = new Inventory(Notice, InventoryCanvas);
    }
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
        InitInventory();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInven = !ShowInven;
            if (ShowInven)
            {
                MyInventory.ShowInventory();
            }
            else MyInventory.HideInventory();
        }
        if (ShowInven)
        {
            if (MyInventory.GetItemCount > 0)
            {
                MyInventory.RotationItem();
            }
        }
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
