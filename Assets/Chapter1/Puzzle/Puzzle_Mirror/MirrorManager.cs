using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    private List<Mirror> Mirrors = new List<Mirror>();
    [SerializeField] private int maxMirrorCount;
    [SerializeField] private GameObject janeText = null;
    private void Awake()
    {
        for(int i = 1; i <= maxMirrorCount; i++)
        {
            Mirrors.Add(GameObject.Find("Mirror" + i).GetComponent<Mirror>());
        }
    }
    public void RemoveMirror(Mirror mir)
    {
        if(CheckMirrorCount())
        {
            Instantiate(janeText, mir.transform.position, Quaternion.LookRotation(-mir.transform.right));
        }
        Mirrors.Remove(mir);
        if(Mirrors.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }
    private bool CheckMirrorCount()
    {
        if (Mirrors.Count == 1)
        {
            //GameManager.Instance.GetPuzzle().SetClear("Mirror", true);
            return true;
        }
        return false;
    }
}
