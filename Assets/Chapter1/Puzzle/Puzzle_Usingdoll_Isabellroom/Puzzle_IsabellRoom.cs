using EPOOutline;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static CameraState;

public class Puzzle_IsabellRoom : MonoBehaviour
{
    #region Refefences
    [SerializeField] int index;

    [SerializeField] LineRenderer lr;

    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] GameObject nextObject;

    [SerializeField] GameObject frame;
    [SerializeField] GameObject[] allDolls;

    [SerializeField] public bool InteractableOK { get; set; } = false;
    #endregion
    
    private bool? puzzleFinish;

    // Player
    private GameObject player = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
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
        {
            for (int i = 0; i < allDolls.Length; i++)
            {
                Debug.Log("You are clicked wrong obj");
                if (allDolls[i].name == "3_Duck")
                {
                    allDolls[i].GetComponentInChildren<Puzzle_IsabellRoom>().RenderLine(false);
                    continue;
                }
                allDolls[i].SendMessage("RenderLine", false, SendMessageOptions.DontRequireReceiver);
            }
            return;
        }

        switch (index)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                RenderLine(true);
                nextObject.GetComponent<Puzzle_IsabellRoom>().InteractableOK = true;
                break;

            case 7:
                RenderLine(true);
                ActiveComponentsToFrame(true);
                frame.AddComponent<BoxCollider>();
                break;

            case 8:
                StartCoroutine(FinishPuzzle());
                break;
        }
    }

    private void RenderLine(bool active)
    {
        if(active == false)
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
            if(this.name != "1_RABBIT")
            {
                InteractableOK = false;
            }
        }
        else
            gameObject.GetComponent<LineRenderer>().enabled = true;

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

    IEnumerator Rotate()
    {
        yield return null;

        while (true)
        {

        }
    }

    IEnumerator FinishPuzzle()
    {
        for(int i = 0; i < allDolls.Length; i++)
        {
            if (allDolls[i].name == "3_Duck")
            {
                allDolls[i].GetComponentInChildren<LineRenderer>().enabled = false;
                Destroy(allDolls[i].GetComponentInChildren<Interactable>());
                continue;
            }
            allDolls[i].GetComponent<LineRenderer>().enabled = false;

            Destroy(allDolls[i].GetComponent<Interactable>());
        }
        frame.AddComponent<Rigidbody>();
        frame.GetComponent<Rigidbody>().AddForce(0,0,-10f, ForceMode.Impulse);
        ActiveComponentsToFrame(false);
        Destroy(frame.GetComponent<Interactable>());
        Destroy(frame.GetComponent<Outlinable>());
        StartCoroutine(Do_Eff());
        yield return null;
    }

   IEnumerator Do_Eff()
    {
        yield return new WaitForSeconds(2f);
        Destroy(frame);
        GameObject.Find("PostProcess").GetComponent<CameraState>().TurnOnState(CamState.FADEOUT);
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < allDolls.Length; i++)
        {
            allDolls[i].transform.LookAt(player.transform.position);
        }
    }
}
