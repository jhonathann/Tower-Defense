using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
/// <summary>
/// Scriptable object that controlls the logic to manage the highscores
/// </summary>
[CreateAssetMenu]
public class HighScoresManager : ScriptableObject

{
     public static Func<int, bool> CheckForHighScore;
     public List<int> highScores;
     private const int MAX_NUMBER_OF_HIGHSCORES = 5;

     /// <summary>
     /// Path where the highscores file will be saved
     /// </summary>
     private string HIGH_SCORES_PATH;
     void OnEnable()
     {
          // Creates the highscores file path
          HIGH_SCORES_PATH = Application.persistentDataPath + "/HighScores";
          //Initializes the list
          highScores = new List<int>();
          //Loads the highscores(if there is a highscore file)
          LoadHighScores();
          //Subscribes to the CheckForHigScore Func
          CheckForHighScore += OnCheckForHighScore;
     }

     private bool OnCheckForHighScore(int score)
     {
          if (IsBelowMaxHighScores() || IsAHighScore(score))
          {
               AddHighScore(score);
               SaveHighScores();
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
               //Sortss the elements from the highest to the lowest
               highScores.Sort((a, b) => b.CompareTo(a));
               highScores = highScores.Take(MAX_NUMBER_OF_HIGHSCORES).ToList();
          }
     }

     /// <summary>
     /// Saves the list of highscores
     /// </summary>
     private void SaveHighScores()
     {
          string jsonHighScores = JsonUtility.ToJson(new JsonableListWrapper<int>(highScores));
          File.WriteAllText(HIGH_SCORES_PATH, jsonHighScores);
     }

     /// <summary>
     /// Loads the serialized file of the highscores (if there is one)
     /// </summary>
     private void LoadHighScores()
     {
          if (ThereIsNoFile()) return;
          string jsonHighScores = File.ReadAllText(HIGH_SCORES_PATH);
          highScores = JsonUtility.FromJson<JsonableListWrapper<int>>(jsonHighScores).list;

          bool ThereIsNoFile()
          {
               return !File.Exists(HIGH_SCORES_PATH);
          }
     }

     //Wrapper to enable serialization of the list(cause unity serialization doesnt work on lists but it does in classes)
     //Thanks to theonlysake in https://forum.unity.com/threads/jsonutilities-tojson-with-list-string-not-working-as-expected.722783/
     [Serializable]
     public class JsonableListWrapper<T>
     {
          public List<T> list;
          public JsonableListWrapper(List<T> list) => this.list = list;
     }
}
