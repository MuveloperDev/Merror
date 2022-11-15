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
    }
    private IEnumerator Cut1()
    {
        yield return null;
    }
    private IEnumerator Cut2()
    {
        yield return null;
    }
}
