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

    //public bool InteractableOK { get { return interactableok; } set { interactableok = InteractableOK; } }
    [SerializeField] bool interactableok = false;
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

        if (gameObject.name == "1_RABBIT" || gameObject.name == "Frame_Finish") interactableok = true;
    }

    private void CheckDolls()
    {
        if (gameObject.GetComponent<Puzzle_IsabellRoom>().interactableok == false)
        {
            for(int i = 0; i < allDolls.Length; i++)
            {
                if(i == 2)
                {
                    allDolls[2].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().interactableok = false;
                    allDolls[2].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().StartCoroutine(Rotate(allDolls[2].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().nextObject.transform, 2f));
                    allDolls[2].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().RenderLine(false);
                }
                else
                {
                    allDolls[i].GetComponent<Puzzle_IsabellRoom>().interactableok = false;
                    allDolls[i].GetComponent<Puzzle_IsabellRoom>().StartCoroutine(Rotate(allDolls[i].GetComponent<Puzzle_IsabellRoom>().nextObject.transform, 2f));
                    allDolls[i].GetComponent<Puzzle_IsabellRoom>().RenderLine(false);
                }
       
            }
            allDolls[1].GetComponent<Puzzle_IsabellRoom>().interactableok = true;

            return;
        }


        Debug.Log("Correct next process on");
        switch (index)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                StartCoroutine(Rotate(frame.transform, 2f));
                nextObject.GetComponent<Puzzle_IsabellRoom>().interactableok = true;
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
        for (int i = 0; i < allDolls.Length; i++)
        {
            allDolls[i].transform.LookAt(player.transform.position);
        }
    }

    IEnumerator Rotate(Transform target, float time)
    {
        Debug.Log("Do Rotate : " + target.gameObject.name);
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

        RenderLine(true);
    }
}
