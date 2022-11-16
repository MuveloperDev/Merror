using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementGuessWho : MonoBehaviour
{
    [SerializeField] private GameObject GuessWho;

    private void Awake()
    {
        GuessWho.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(WatchAllClear());
    }

    private IEnumerator WatchAllClear()
    {
        while (true)
        {
            if(GameManager.Instance.GetPuzzle() != null)
            {
                if (GameManager.Instance.GetPuzzle().GetCurrentPuzzle() != null)
                    break;
            }

            yield return null;
        }

        yield return new WaitUntil(() => GameManager.Instance.GetPuzzle().GetCurrentPuzzle().AllClear == true);
        GuessWho.SetActive(true);
    }
}
