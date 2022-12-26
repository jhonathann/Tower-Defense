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
     /// The list of parts that the player currently has
     /// </summary>
     /// <typeparam name="Part">The Part Class</typeparam>
     /// <returns></returns>
     public List<Part> parts = new List<Part>();
     void OnEnable()
     {
          health = 10;
          AddStartingParts(6);
     }
     /// <summary>
     /// Adds the starting parts to the gameobject
     /// </summary>
     /// <param name="numberOfParts">The number of starting parts</param>
     void AddStartingParts(int numberOfParts)
     {
          for (int i = 0; i < numberOfParts; i++)
          {
               parts.Add(new Part());
          }
     }
}
