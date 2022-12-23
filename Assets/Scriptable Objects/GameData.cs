using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ScriptableObject that contains relevant game informatcion
/// </summary>
[CreateAssetMenu]
public class GameData : ScriptableObject
{
     /// <summary>
     /// Reference to the generated path
     /// </summary>
     public List<Tile> path;
     /// <summary>
     /// The health of the player
     /// </summary>
     public int health;
     /// <summary>
     /// This enables the next wave
     /// </summary>
     public bool sendNextWave;
     void OnEnable()
     {
          health = 10;
          sendNextWave = false;
     }
}
