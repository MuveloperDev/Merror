using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using System.Linq;

public class PuzzleManager : MonoBehaviour
{
    private Chapter1Puzzle chapter1Puzzle = null;
    public Chapter1Puzzle GetChapter1Puzzle() => chapter1Puzzle;
    public class Chapter1Puzzle
    {
        public List<GameObject> Doors = new List<GameObject>();
        public Dictionary<string, bool> Puzzle = new Dictionary<string, bool>();
        public bool AllClear = false;
        public Chapter1Puzzle()
        {
            //if(Get saved data == true)
            // else
            Puzzle.Add("GuessWho", false);
            Puzzle.Add("Keyboard", false);
            Puzzle.Add("Doll", false);
            Puzzle.Add("Mirror", false);
        }
        public void GetDoors()
        {
            Doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
        }
    }
    private Chapter2Puzzle chapter2Puzzle = null;
    public Chapter2Puzzle GetChapter2Puzzle() => chapter2Puzzle;
    public class Chapter2Puzzle
    {
        public Dictionary<string, bool> Puzzle = new Dictionary<string, bool>();
        public Chapter2Puzzle()
        { 
            
        }
    }
    /// <summary>
    /// Initialize memory for a certain chapter's puzzle.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    public void InitPuzzle(int chapterNum)
    {
        switch (chapterNum)
        {
            case 1: { chapter1Puzzle = new Chapter1Puzzle(); break; }
            case 2: { chapter2Puzzle = new Chapter2Puzzle(); break; }
        }
    }
    /// <summary>
    /// Set each puzzle's clear flag based on parameter chapter number, puzzle name, flag.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    /// <param name="puzzleName">Puzzle's name</param>
    /// <param name="isClear">Clear flag</param>
    public void SetClear(int chapterNum, string puzzleName, bool isClear)
    {
        Dictionary<string, bool> dic = null;
        switch (chapterNum)
        {
            default: { Debug.LogError("Puzzle chapter index error."); return; }
            case 1: { if(chapter1Puzzle != null) dic = chapter1Puzzle.Puzzle; break; }
            case 2: { if (chapter2Puzzle != null) dic = chapter2Puzzle.Puzzle; break; }
        }

        foreach (var puzzle in dic)
        {
            if (puzzle.Key == puzzleName)
            {
                dic.Remove(puzzle.Key);
                dic.Add(puzzleName, isClear);
                Debug.Log(puzzleName + " puzzle's clear state changed : " + isClear);
                break;
            }
        }
        CheckAllPuzzleClear(chapterNum);
    }
    /// <summary>
    /// Check this chapter's puzzles are all cleared.
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>
    private void CheckAllPuzzleClear(int chapterNum)
    {
        Dictionary<string, bool> dic = null;
        switch (chapterNum)
        {
            default: { Debug.LogError("Puzzle chapter index error."); return; }
            case 1: { if (chapter1Puzzle != null) dic = chapter1Puzzle.Puzzle; break; }
            case 2: { if (chapter2Puzzle != null) dic = chapter2Puzzle.Puzzle; break; }
        }
        foreach (var puzzle in dic)
        {
            if (puzzle.Value == false)
            {
                Debug.Log("All puzzles not clear yet.");
                return;
            }
        }
        Debug.Log("Chapter " + chapterNum + " puzzles all clear.");
        chapter1Puzzle.AllClear = true;
    }
}
