using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    [SerializeField] GameObject blackBoard;
    [SerializeField] Material blackBoardMaterial = null;


    private void Awake()
    {
        blackBoardMaterial = blackBoard.GetComponent<MeshRenderer>().material;
    }
   
    public void CallChangeBlackBoardAlpha()
    {
        StartCoroutine(ChangeBlackBoardAlpha());
    }

    public IEnumerator ChangeBlackBoardAlpha()
    {
        if (blackBoard == null)
        {
            yield break;
        }

        float plusAlpha = 1f;

        while (plusAlpha > 0f)
        {
            plusAlpha -= 0.03f;

            blackBoardMaterial.color = new Color(1f, 1f, 1f, plusAlpha);

            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}
