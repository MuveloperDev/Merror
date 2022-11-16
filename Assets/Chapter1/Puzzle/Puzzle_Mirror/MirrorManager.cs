using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    private List<Mirror> Mirrors = new List<Mirror>();
    [SerializeField] private int maxMirrorCount;
    [SerializeField] private GameObject janeText = null;
    private int count;
    public int GetMirrorsCount() => Mirrors.Count;
    public bool GetMirrorsIsBroken(int index) => Mirrors[index].IsBroken;
    
    private void Awake()
    {
        for(int i = 1; i <= maxMirrorCount; i++)
        {
            Mirrors.Add(GameObject.Find("Mirror" + i).GetComponent<Mirror>());
        }
        count = Mirrors.Count;
    }

    public void RemoveMirror(Mirror mir)
    {
        count--;
        GameManager.Instance.Save();
        if (count == 0)
        {
            Instantiate(janeText, mir.transform.position, Quaternion.LookRotation(-mir.transform.right));
        }
    }

    public void SetMirrorsIsBroken(int index,bool isBroken)
    {
        count--;
        Mirrors[index].IsBroken = isBroken;
        Mirrors[index].gameObject.SetActive(false);
    }
}
