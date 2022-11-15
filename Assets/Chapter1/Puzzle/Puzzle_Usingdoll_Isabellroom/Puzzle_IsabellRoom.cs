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
                if (i == 2)
                {
                    allDolls[2].GetComponentInChildren<Puzzle_IsabellRoom>().StartCoroutine(Rotate(nextObject.transform, 2f, false));
                    continue;
                }
                else
                    allDolls[i].GetComponent<Puzzle_IsabellRoom>().StartCoroutine(Rotate(nextObject.transform, 2f, false));
            }
            allDolls[0].GetComponent<Puzzle_IsabellRoom>().InteractableOK = true;
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
                StartCoroutine(Rotate(frame.transform, 2f, true));
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
            gameObject.GetComponent<LineRenderer>().enabled = false;
        else
            gameObject.GetComponent<LineRenderer>().enabled = true;

        lr.startColor = new Color(1, 0, 0, 0.7f);
        lr.endColor = new Color(1, 0, 0, 0.7f);
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
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
        yield return new WaitForFixedUpdate();
    }

   IEnumerator Do_Eff()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("PostProcess").GetComponent<CameraState>().TurnOnState(CamState.LIGHTOUT);
        yield return new WaitForSeconds(2f);
        Destroy(frame);
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < allDolls.Length; i++)
        {
            allDolls[i].transform.LookAt(player.transform.position);
        }
    }

    IEnumerator Rotate(Transform target, float time, bool correct)
    {
        Vector3 dir;
        while (time > 0)
        {
            if (gameObject.name == "3_DuckObj")
            {
                dir = target.transform.position - allDolls[2].transform.position;
                allDolls[2].transform.rotation = Quaternion.Lerp(allDolls[2].transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
            }
            else
            {
                dir = target.transform.position - transform.position;
                this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
            }

            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(time);
        RenderLine(correct);
    }
}
