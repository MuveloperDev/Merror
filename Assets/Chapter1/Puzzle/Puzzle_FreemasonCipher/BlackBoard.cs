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
        Debug.Log("코루틴 호출 준비");
        StartCoroutine(ChangeBlackBoardAlpha());
    }

    public IEnumerator ChangeBlackBoardAlpha()
    {
        if (blackBoard == null)
        {
            yield break;
        }

        float plusAlpha = 1f;

        Debug.Log("DestroyInteractable 들어옴");

        while (plusAlpha > 0f)
        {
            Debug.Log("DestroyInteractable : " + plusAlpha);
            plusAlpha -= 0.03f;

            blackBoardMaterial.color = new Color(1f, 1f, 1f, plusAlpha);

            Debug.Log("DestroyInteractable2 : " + blackBoardMaterial.color.a);

            yield return new WaitForSeconds(0.05f);
            Debug.Log("DestroyInteractable3 : " + plusAlpha);
        }
        yield break;
    }
}
