using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public partial class Interactable : MonoBehaviour
{
    [Header("More Function")]
    [SerializeField] private bool Gettable = false;
    [SerializeField] private float Inventory_Scale = 1f;
    [SerializeField] private bool Special = false;
    [SerializeField] private Light _Light = null;

    /// <summary>
    /// Do something special fuction.
    /// </summary>
    public virtual void Do_Special()
    {
        switch (myType)
        {
            case ObjectType.Toilet_Flush:
                {
                    Do_Flush();
                    break;
                }
            case ObjectType.Switch:
                {
                    Do_Light();
                    break;
                }
            case ObjectType.PuzzleGuessWho:
                {
                    Do_Puzzle_GuessWho();
                    break;
                }
            case ObjectType.Mirror:
                {
                    Do_Break_Mirror();
                    Debug.Log("Added");
                    break;
                }
        }
    }
    /// <summary>
    /// Flush Toilet water.
    /// </summary>
    protected void Do_Flush()
    {
        if (TryGetComponent<Toilet>(out Toilet toilet))
        {
            StartCoroutine(toilet.Flush());
        }
        else
        {
            Debug.Log("This Object not have Flush Script.");
        }
    }
    /// <summary>
    /// Turn on / off light if this object is switch type.
    /// </summary>
    protected void Do_Light()
    {
        if (_Light != null)
        {
            _Light.enabled = !_Light.enabled;
        }
    }

    public virtual void Do_Puzzle_GuessWho()
    {
        if (TryGetComponent<PuzzleGuessWho>(out PuzzleGuessWho puzzleGuessWho))
        {
            puzzleGuessWho.SendMessage("CheckAnswer",SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This Object not have PuzzleGuessWho Script.");
        }
    }
    public void Do_Break_Mirror()
    {
        if (TryGetComponent<Mirror>(out Mirror mirror))
        {
            Debug.Log("GetMirror");
            mirror.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This Object not have Mirror Script.");
        }
    }
}
