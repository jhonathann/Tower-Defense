using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the behaviour of the castle
/// </summary>
public class CastleController : MonoBehaviour, IDamagable
{
     //Access to the gameData
     public GameData gameData;
     //Destroys the gameObject when the player health gets to 0
     void checkHealth()
     {
          if (gameData.health <= 0)
          {
               Destroy(this.gameObject);
          }
     }
     /// <summary>
     /// Function that destribes how the object takes damage
     /// </summary>
     /// <param name="damageAmount">The amount of damage taken</param>
     void IDamagable.TakeDamage(float damageAmount)
     {
          gameData.health = gameData.health - damageAmount;
          checkHealth();
     }
}
