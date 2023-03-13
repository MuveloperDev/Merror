using EPOOutline;
using System.Collections;
using UnityEngine;

public class Puzzle_IsabellRoom : Interactable, ISpecial
{
    #region CachingComponents
    [SerializeField] LineRenderer lr;
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] Transform PlayerPosAfterCutVideo = null;
    [SerializeField] GameObject nextObject;
    [SerializeField] GameObject frame;
    [SerializeField] GameObject Hint1 = null;
    [SerializeField] GameObject[] allDolls;
    [SerializeField] Interactable[] openDoors = null;
    [SerializeField] LineRenderer myRenderLine = null;
    [SerializeField] Interactable myInteractable = null;
    [SerializeField] Outlinable myOutlinable = null;
    [SerializeField] Puzzle_IsabellRoom myPuzzle = null;
    [SerializeField] Puzzle_IsabellRoom nextObjPuzzle = null;
    #endregion
    #region memberVariable
    [SerializeField] int index;
    [SerializeField] bool interactableok = false;
    private bool? puzzleFinish = null;
    #endregion
    // Player
    private GameObject player = null;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected new void Start()
    {
        //Receive this puzzle is Finished from GameManager
        if(GameManager.Instance.isFirstPlay == false)
            puzzleFinish = GameManager.Instance.GetData().puzzles[1].isCleared;

        if (puzzleFinish == null)
            Debug.LogError("No Receive DataValue From GameManager");

        if(nextObject != null)
            RotateSM(nextObject.transform, 1f, false);

        // Check this Puzzle is finished
        if (puzzleFinish == true)
        {
            Destroy(myInteractable);
            Destroy(myOutlinable);
            frame.SetActive(false);
            Destroy(this, 0.5f);
            return;
        }
        if (gameObject.name == "1_RABBIT" || gameObject.name == "Frame_Finish") interactableok = true;
    }

    public override void Do_Interact()
    {
        MySpecial();
    }

    public void MySpecial()
    {
        if (interactableok == false)
        {
            for (int i = 0; i < allDolls.Length; i++)
            {
                if (i == 2)
                {
                    allDolls[i].transform.GetChild(0).SendMessage("WrongAnswer", SendMessageOptions.DontRequireReceiver);
                    continue;
                }
                allDolls[i].SendMessage("WrongAnswer", SendMessageOptions.DontRequireReceiver);
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
                StartCoroutine(Rotate(frame.transform, 1f, true));
                nextObjPuzzle.interactableok = true;
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

    private void WrongAnswer()
    {
        RenderLine(false);
        RotateSM(nextObject.transform, 1f, false);
        interactableok = false;

        if (gameObject.name == "1_RABBIT")
            interactableok = true;
    }

    private void RenderLine(bool active)
    {
        if(active == false)
            myRenderLine.enabled = false;
        else
            myRenderLine.enabled = true;

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
            if(i == 2)
            {
                allDolls[i].transform.GetChild(0).SendMessage("RemoveAll", SendMessageOptions.DontRequireReceiver);
                continue;
            }
            allDolls[i].SendMessage("RemoveAll", SendMessageOptions.DontRequireReceiver);
        }
        frame.AddComponent<Rigidbody>();
        frame.GetComponent<Rigidbody>().AddForce(0,0,-10f, ForceMode.Impulse);
        ActiveComponentsToFrame(false);
        Destroy(frame.GetComponent<Interactable>());
        Destroy(frame.GetComponent<Outlinable>());
        StartCoroutine(Do_Eff());
        yield return new WaitForFixedUpdate();
    }

    private void RemoveAll()
    {
        myRenderLine.enabled = false;
        Destroy(myRenderLine);
        Destroy(myInteractable);
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
            });
    }

    void RotateSM(Transform target, float time, bool delayRender)
    {
        StartCoroutine(Rotate(target, time, delayRender));
    }

    IEnumerator Rotate(Transform target, float time, bool delayRender)
    {
        Vector3 dir;
        float fixedDeltaTime = Time.fixedDeltaTime;
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

            time -= fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (delayRender == true)
            RenderLine(true);
    }

    private void InsertItemInventory()
    {
        if(gameObject.name == "Frame_Finish")
            GameManager.Instance.ClearPuzzle("Puzzle_IsabellRoom", Hint1, 7f);

        if (puzzleFinish == true)
        {
            return;
        }
        StartCoroutine(FinishPuzzle());
    }
}
