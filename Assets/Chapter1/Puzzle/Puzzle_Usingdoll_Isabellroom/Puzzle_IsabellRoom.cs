using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Puzzle_IsabellRoom : MonoBehaviour
{
    [SerializeField] int index;

    [SerializeField] LineRenderer lr;

    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] GameObject nextObject;

    [SerializeField] GameObject frame;
    [SerializeField] GameObject[] allDolls;

    private bool? puzzleFinish;
    [SerializeField] public bool InteractableOK { get; set; } = false;

    private void Awake()
    {
        // Receive this puzzle is Finished from GameManager
        // puzzleFinish = GameManager.Inst.~~~;

        //if (puzzleFinish == null)
        //    Debug.LogError("Failed To Receive Value From GameManager");

        // Check this Puzzle is finished
        if (puzzleFinish == true)
        {
            Destroy(gameObject.GetComponent<Interactable>());
            Destroy(gameObject.GetComponent<Puzzle_IsabellRoom>());

            // Set Position of frame
            // ~~~
        }

        if (gameObject.name == "1_RABBIT" || gameObject.name == "Frame_Finish") InteractableOK = true;
    }

    private void CheckDolls()
    {
        if (gameObject.GetComponent<Puzzle_IsabellRoom>().InteractableOK == false)
            return;

        switch (index)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                RenderLine();
                nextObject.GetComponent<Puzzle_IsabellRoom>().InteractableOK = true;
                break;

            case 7:
                RenderLine();
                ActiveComponentsToFrame(true);
                frame.AddComponent<BoxCollider>();
                break;

            case 8:
                StartCoroutine(FinishPuzzle());
                break;
        }
    }

    private void RenderLine()
    {
        lr.startColor = new Color(1, 0, 0, 0.7f);
        lr.endColor = new Color(1, 0, 0, 0.7f);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, startPos.position);
        lr.SetPosition(1, endPos.position);
    }

    private void ActiveComponentsToFrame(bool active)
    {
        frame.GetComponentInChildren<Interactable>().enabled = active;
        frame.GetComponentInChildren<Outlinable>().enabled = active;
    }

    IEnumerator FinishPuzzle()
    {
        for(int i = 0; i < allDolls.Length; i++)
        {
            allDolls[i].GetComponent<LineRenderer>().enabled = false;
            Destroy(allDolls[i].GetComponent<Interactable>());
        }
        frame.AddComponent<Rigidbody>();
        Invoke("DelayDestroy", 2f);
        yield return null;
    }

    private void DelayDestroy()
    {
        Destroy(frame.GetComponent<Interactable>());
        ActiveComponentsToFrame(false);
    }
}
