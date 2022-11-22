using EPOOutline;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
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

    [SerializeField] bool interactableok = false;
    [SerializeField] GameObject Hint1 = null;
    [SerializeField] Interactable[] openDoors = null;
    [SerializeField] Transform PlayerPosAfterCutVideo = null;

    #endregion

    private bool? puzzleFinish = null;

    // Player
    private GameObject player = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        //Receive this puzzle is Finished from GameManager
        if(GameManager.Instance.isFirstPlay == false)
            puzzleFinish = GameManager.Instance.GetData().puzzles[1].isCleared;

        if (puzzleFinish == null)
            Debug.LogError("No Receive DataValue From GameManager");

        // Check this Puzzle is finished
        if (puzzleFinish == true)
        {
            Destroy(gameObject.GetComponent<Interactable>());
            Destroy(gameObject.GetComponent<Outlinable>());
            frame.SetActive(false);
        }
        if (gameObject.name == "1_RABBIT" || gameObject.name == "Frame_Finish") interactableok = true;
    }

    private void CheckDolls()
    {
        if (gameObject.GetComponent<Puzzle_IsabellRoom>().interactableok == false)
        {
            for(int i = 0; i < allDolls.Length; i++)
            {
                if (i == 2)
                {
                    allDolls[i].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().RenderLine(false);
                    allDolls[i].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().interactableok = false;
                    allDolls[i].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().RotateSM(allDolls[i].transform.GetChild(0).GetComponent<Puzzle_IsabellRoom>().nextObject.transform, 1f, false);
                    continue;
                }
                allDolls[i].GetComponent<Puzzle_IsabellRoom>().RenderLine(false);
                allDolls[i].GetComponent<Puzzle_IsabellRoom>().RotateSM(allDolls[i].GetComponent<Puzzle_IsabellRoom>().nextObject.transform, 1f, false);
                allDolls[i].GetComponent<Puzzle_IsabellRoom>().interactableok = false;
            }
            allDolls[0].GetComponent<Puzzle_IsabellRoom>().interactableok = true;
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
                StartCoroutine(Rotate(frame.transform, 1f, true));
                nextObject.GetComponent<Puzzle_IsabellRoom>().interactableok = true;
                break;

            case 7:
                RenderLine(true);
                ActiveComponentsToFrame(true);
                frame.AddComponent<BoxCollider>();
                break;

            case 8:
                InsertItemInventory();
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
        for (int i = 0; i < allDolls.Length; i++)
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
        Destroy(frame);
        GameManager.Instance.GetVideoPlayer().CallPlayVideo(GameManager.Instance.GetVideoPlayer().GetClip(VideoPlayerManager.VideoCategory.CHAPTER1, "ISABELROOM"),
            () =>
            {
                player.transform.position = PlayerPosAfterCutVideo.position;
                player.transform.rotation = PlayerPosAfterCutVideo.rotation;
                for (int i = 0; i < openDoors.Length; i++)
                {
                    openDoors[i].IsLocked = false;
                }
                GameManager.Instance.Save();
                Debug.Log("EndVideo");
            });
    }

    void RotateSM(Transform target, float time, bool delayRender)
    {
        StartCoroutine(Rotate(target, time, delayRender));
    }

    IEnumerator Rotate(Transform target, float time, bool delayRender)
    {
        Debug.Log(" Do Rotate : " + target.gameObject.name);
        Vector3 dir;
        while (time > 0)
        {
            if (gameObject.name == "3_DuckObj")
            {
                dir = target.transform.position - allDolls[2].transform.position;
                allDolls[2].transform.rotation = Quaternion.Lerp(allDolls[2].transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3f);
            }
            else
            {
                dir = target.transform.position - transform.position;
                this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3f);
            }

            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (delayRender == true)
            RenderLine(true);
    }

    private void InsertItemInventory()
    {
        GameManager.Instance.ClearPuzzle("Puzzle_IsabellRoom", Hint1, 7f);
        if (puzzleFinish == true)
        {
            return;
        }
        StartCoroutine(FinishPuzzle());
    }
}
