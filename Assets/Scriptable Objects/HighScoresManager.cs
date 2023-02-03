using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Scriptable object that controlls the logic to manage the highscores
/// </summary>
[CreateAssetMenu]
public class HighScoresManager : ScriptableObject

{
     public static Func<int, bool> CheckForHighScore;
     public List<int> highScores = new List<int>();
     private const int MAX_NUMBER_OF_HIGHSCORES = 5;
     void OnEnable()
     {
          CheckForHighScore += OnCheckForHighScore;
     }

     private bool OnCheckForHighScore(int score)
     {
          if (IsBelowMaxHighScores() || IsAHighScore(score))
          {
               AddHighScore(score);
               return true;
          }
          return false;

          bool IsBelowMaxHighScores()
          {
               return highScores.Count < MAX_NUMBER_OF_HIGHSCORES;
          }
          bool IsAHighScore(int score)
          {
               return score > highScores[MAX_NUMBER_OF_HIGHSCORES - 1];
          }
          void AddHighScore(int score)
          {
               highScores.Add(score);
               highScores.Sort();
               highScores = highScores.Take(MAX_NUMBER_OF_HIGHSCORES).ToList();
          }
     }
}
