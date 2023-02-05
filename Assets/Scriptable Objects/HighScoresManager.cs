using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Security.Cryptography;

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

     /// <summary>
     /// Creates a symetric encription algorithm acording to the Advance Encryption Standar (generates a new key and iv each session)
     /// </summary>
     private Aes advanceEncryptionStandard = Aes.Create();
     void OnEnable()
     {
          // Creates the highscores file path
          HIGH_SCORES_PATH = Application.persistentDataPath + "/HighScores.json";
          //Initializes the list
          highScores = new List<int>();
          //Try to load the highscores (different exeption may arise in the access to the file or in the encryption itself)
          try
          {
               LoadHighScores();
          }
          catch { }

          //Subscribes to the CheckForHigScore Func
          CheckForHighScore = OnCheckForHighScore;
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
          string jsonHighScores = JsonUtility.ToJson(new SerializableListWrapper<int>(highScores));
          EncryptAndSave(jsonHighScores);
          SaveKeyAndIV();

          //Saves the Key and the IV for the encryption using the PlayerPrefs class
          void SaveKeyAndIV()
          {
               string key = Convert.ToBase64String(advanceEncryptionStandard.Key);
               string iv = Convert.ToBase64String(advanceEncryptionStandard.IV);
               PlayerPrefs.SetString("key", key);
               PlayerPrefs.SetString("iv", iv);
          }
     }

     /// <summary>
     /// Loads the serialized file of the highscores
     /// </summary>
     private void LoadHighScores()
     {
          string jsonHighScores = LoadAndDecrypt();
          highScores = JsonUtility.FromJson<SerializableListWrapper<int>>(jsonHighScores).list;
     }

     /// <summary>
     /// Encrypts the data and saves it into the file
     /// </summary>
     /// <param name="text">json string of the data to be encrypted</param>
     private void EncryptAndSave(string text)
     {
          using (FileStream fileStream = new FileStream(HIGH_SCORES_PATH, FileMode.Create, FileAccess.Write))
          {
               using (CryptoStream cryptoStream = new CryptoStream(fileStream, advanceEncryptionStandard.CreateEncryptor(), CryptoStreamMode.Write))
               {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                         streamWriter.Write(text);
                    }
               }
          }
     }

     /// <summary>
     /// Reads the data and decrypts it
     /// </summary>
     /// <returns>The json string of the decrypted data</returns>
     private string LoadAndDecrypt()
     {
          byte[] key = Convert.FromBase64String(PlayerPrefs.GetString("key"));
          byte[] iv = Convert.FromBase64String(PlayerPrefs.GetString("iv"));

          using (FileStream fileStream = new FileStream(HIGH_SCORES_PATH, FileMode.Open, FileAccess.Read))
          {
               using (CryptoStream cryptoStream = new CryptoStream(fileStream, advanceEncryptionStandard.CreateDecryptor(key, iv), CryptoStreamMode.Read))
               {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                         return streamReader.ReadToEnd();
                    }
               }
          }
     }
     //Wrapper to enable serialization of the list(cause unity serialization doesnt work on lists but it does in classes)
     //Thanks to theonlysake in https://forum.unity.com/threads/jsonutilities-tojson-with-list-string-not-working-as-expected.722783/
     [Serializable]
     public class SerializableListWrapper<T>
     {
          public List<T> list;
          public SerializableListWrapper(List<T> list) => this.list = list;
     }
}
