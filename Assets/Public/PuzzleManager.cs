using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using System.Linq;
using JetBrains.Annotations;

public class PuzzleManager : MonoBehaviour
{
    private Puzzle CurrentPuzzle = null;
    public Puzzle GetCurrentPuzzle()
    {
        return this.CurrentPuzzle;
    }
    public class Puzzle : PuzzleManager
    {
        public int ChapterNumber;
        public bool AllClear = false;
        public ChapterPuzzles[] chapterPuzzleDatas;
        public Puzzle() { Debug.Log("New Puzzle Class Instantiated."); }
    }

    /// <summary>
    /// Initialize memory for a certain chapter's puzzle.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    public void InitPuzzle(int chapterNum)
    {
        if (chapterNum <= 0 || chapterNum > 5)
        {
            Debug.Log("Chapter Number Error : Can't initialize puzzle class.");
            return;
        }
        if (CurrentPuzzle != null) CurrentPuzzle = null; // Delete current puzzle memory.
        CurrentPuzzle = new GameObject(typeof(Puzzle).ToString(), typeof(Puzzle)).GetComponent<Puzzle>();
        CurrentPuzzle.transform.SetParent(this.transform);

        Debug.Log(CurrentPuzzle);
        CurrentPuzzle.ChapterNumber = chapterNum;
        switch (CurrentPuzzle.ChapterNumber) 
        {
            case 1:
                CurrentPuzzle.chapterPuzzleDatas = new ChapterPuzzles[5];
                CurrentPuzzle.chapterPuzzleDatas[0] = new ChapterPuzzles("FreemasonCipher", false);
                CurrentPuzzle.chapterPuzzleDatas[1] = new ChapterPuzzles("Puzzle_IsabellRoom", false);
                CurrentPuzzle.chapterPuzzleDatas[2] = new ChapterPuzzles("Decryption_Puzzle", false);
                CurrentPuzzle.chapterPuzzleDatas[3] = new ChapterPuzzles("Mirror", false);
                CurrentPuzzle.chapterPuzzleDatas[4] = new ChapterPuzzles("PuzzleGuessWho", false);
                break;
        }


    }

    /// <summary>
    /// Set each puzzle's clear flag based on parameter chapter number, puzzle name, flag.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    /// <param name="puzzleName">Puzzle's name</param>
    /// <param name="isClear">Clear flag</param>
    public void SetClear(string puzzleName, bool isClear)
    {
        bool isContain = false;

        for (int i = 0; i < CurrentPuzzle.chapterPuzzleDatas.Length; i++)
        {
            if (CurrentPuzzle.chapterPuzzleDatas[i].puzzleName.Equals(puzzleName))
            {
                CurrentPuzzle.chapterPuzzleDatas[i].isCleared = isClear;
                isContain = true;
                Debug.Log(puzzleName + " puzzle's clear state changed : " + isClear);
                break;
            }
        }

        if (isContain == false) // If puzzle name doesn't exist in dictionary
        {
            Debug.LogError("Can't set puzzle clear state : Puzzle name error");
            return;
        }
        CheckAllPuzzleClear();
    }
    

    /// <summary>
    /// Check this chapter's puzzles are all cleared.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    private void CheckAllPuzzleClear()
    {
        for(int i = 0; i < CurrentPuzzle.chapterPuzzleDatas.Length - 2; i++)
        {
            if(CurrentPuzzle.chapterPuzzleDatas[i].isCleared == false)
            {
                CurrentPuzzle.AllClear = false;
                return;
            }
        }
        
        Debug.Log("Chapter " + CurrentPuzzle.ChapterNumber + " puzzles all clear.");
        CurrentPuzzle.AllClear = true;
    }

    public GameObject FindClearPuzzleObj(int chapterNum,string pzlName)
    {
        GameObject resObj = null;
        switch (chapterNum) 
        {
            case 1:
                switch (pzlName) 
                {
                    case "PuzzleGuessWho":
                        resObj = FindObjectOfType<PuzzleGuessWho>().gameObject;
                        break;
                    case "Puzzle_IsabellRoom":
                        resObj = FindObjectOfType<Puzzle_IsabellRoom>().gameObject;
                        break;
                    case "Mirror":
                        resObj = FindObjectOfType<Mirror>().gameObject;
                        break;
                    case "Decryption_Puzzle":
                        resObj = FindObjectOfType<Decryption_Puzzle>().gameObject;
                        break;
                    case "FreemasonCipher":
                        resObj = FindObjectOfType<FreemasonCipher>().gameObject;
                        break;
                    default:
                        resObj = null;
                        break;
                }
                break;
        }

        return resObj;
    }
}
