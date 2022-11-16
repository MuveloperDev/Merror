using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Trailer : MonoBehaviour
{
    public int camCount = 0;
    private CinemachineVirtualCamera[] virtualCam;
    private WaitForSeconds wait;
    private void Awake()
    {
        virtualCam = new CinemachineVirtualCamera[camCount];
        for(int i = 1; i <= camCount; i++)
        {
            virtualCam[i - 1] = GameObject.Find("CM vcam" + i).GetComponent<CinemachineVirtualCamera>();
        }   
    }
    private IEnumerator Cut1()
    {
        virtualCam[0].enabled = true;

        yield return null;
    }
    private IEnumerator Cut2()
    {
        yield return null;
    }
}
