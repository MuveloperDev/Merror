using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using System.Linq;
using JetBrains.Annotations;

public class PuzzleManager : MonoBehaviour
{
    private Puzzle CurrentPuzzle = null;
    public Puzzle GetCurrentPuzzle() => CurrentPuzzle;
    public class Puzzle : PuzzleManager
    {
        public int ChapterNumber;
        public Dictionary<string, bool> PuzzleList = null;
        public bool AllClear = false;
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
        if(CurrentPuzzle != null) CurrentPuzzle = null; // Delete current puzzle memory.
        CurrentPuzzle = new Puzzle(); // Create new puzzle class
        CurrentPuzzle.ChapterNumber = chapterNum;
        CurrentPuzzle.PuzzleList = new Dictionary<string, bool>();
        switch (CurrentPuzzle.ChapterNumber) // Init puzzle dictionary
        {
            case 1:
                {
                    CurrentPuzzle.PuzzleList.Add("GuessWho", false);
                    CurrentPuzzle.PuzzleList.Add("Keyboard", false);
                    CurrentPuzzle.PuzzleList.Add("Doll", false);
                    CurrentPuzzle.PuzzleList.Add("Mirror", false);
                    break;
                }
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
        foreach (var puzzle in CurrentPuzzle.PuzzleList)
        {
            if (puzzle.Key == puzzleName)
            {
                CurrentPuzzle.PuzzleList.Remove(puzzle.Key);
                CurrentPuzzle.PuzzleList.Add(puzzleName, isClear); // Change state.
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
        foreach (var puzzle in CurrentPuzzle.PuzzleList)
        {
            if (puzzle.Value == false)
            {
                Debug.Log("All puzzles not clear yet.");
                return;
            }
        }
        Debug.Log("Chapter " + CurrentPuzzle.ChapterNumber + " puzzles all clear.");
        CurrentPuzzle.AllClear = true;
    }
}
