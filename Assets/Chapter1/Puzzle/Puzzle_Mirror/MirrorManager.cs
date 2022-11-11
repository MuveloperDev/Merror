using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    private List<Mirror> Mirrors = new List<Mirror>();
    private void Awake()
    {
        Mirrors.AddRange(GameObject.FindObjectsOfType<Mirror>());
    }
    public void RemoveMirror(Mirror gameobject)
    {
        Mirrors.Remove(gameobject);
        CheckMirrorCount();
    }
    private void CheckMirrorCount()
    {
        if (Mirrors.Count == 0)
        {
            GameManager.Instance.GetPuzzle().SetClear("Mirror", true);
        }
    }
}
