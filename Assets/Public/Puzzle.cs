using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;

public class Puzzle : Singleton<Puzzle>
{
    public enum Puzzles
    {
        GuessWho,
        IsabelDoll,
        Mirror,
    }
    public class Chapter1Puzzle
    {
        public bool Guess_Who;
        public bool Isabel_Doll;
        public bool Break_Mirror;
        public Chapter1Puzzle() { Guess_Who = false; Isabel_Doll = false; Break_Mirror = false; }
        public Chapter1Puzzle(bool LoadData)
        {
            if(LoadData)
            {
                
            }
        }
    }
    private Chapter1Puzzle chapter1Puzzle = null;

    public class Chapter2Puzzle
    {
        public Chapter2Puzzle() { }
    }
    private Chapter2Puzzle chapter2Puzzle = null;
    public void InitPuzzle(int chapterNum)
    {
        switch (chapterNum)
        {
            case 1: { chapter1Puzzle = new Chapter1Puzzle(); break; }
            case 2: { chapter2Puzzle = new Chapter2Puzzle(); break; }
        }
    }
    public void SetClear(Puzzles puzzleName, bool isClear)
    {
        switch (puzzleName)
        {
            case Puzzles.GuessWho: { chapter1Puzzle.Guess_Who = isClear; break; }
            case Puzzles.IsabelDoll: { chapter1Puzzle.Isabel_Doll = isClear; break; }
            case Puzzles.Mirror: { chapter1Puzzle.Break_Mirror = isClear; break; }
        }
    }
}
