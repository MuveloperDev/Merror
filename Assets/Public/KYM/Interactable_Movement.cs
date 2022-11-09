using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Interactable : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private bool Moveable = false;
    [SerializeField] private Axis Movement_Axis = Axis.X;
    [SerializeField] private float Target_Movement = 5f;
    [SerializeField] private float Movement_Speed = 1f;
    [SerializeField] protected bool InvertMovement = false;

    private Coroutine MoveCoroutine = null;
    private void Do_Movement()
    {
        if (Moveable == false) return;
        MoveCoroutine ??= StartCoroutine(MoveObj());
    }
    private IEnumerator MoveObj()
    {
        float deltaMovement = 0f;
        float framePerMovement = 0f;
        while (true)
        {
            if (deltaMovement >= Target_Movement)
            {
                InvertMovement = !InvertMovement;
                MoveCoroutine = null;
                yield break;
            }
            framePerMovement = Time.deltaTime * Movement_Speed;
            framePerMovement = InvertMovement == true ? -framePerMovement : framePerMovement;
            deltaMovement += Mathf.Abs(framePerMovement);
            switch(Movement_Axis)
            {
                case Axis.X: { transform.position += new Vector3(framePerMovement, 0f, 0f); break; }
                case Axis.Y: { transform.position += new Vector3(0f, framePerMovement, 0f); break; }
                case Axis.Z: { transform.position += new Vector3(0f, 0f, framePerMovement); break; }
            }
            yield return null;
        }
    }
}
