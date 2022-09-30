using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
     public List<Tile> path;
     public int health;
     void OnEnable()
     {
          health = 10;
     }
}
