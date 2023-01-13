using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class to control the behaviour of the castle
/// </summary>
public class CastleController : MonoBehaviour, IDamagable
{
     /// <summary>
     /// Access to the gameData
     /// </summary>
     public GameData gameData;
     /// <summary>
     /// Events that triggers when the castle is destroyed
     /// </summary>
     public static event Action CastleDestroyed;
     //Destroys the gameObject when the player health gets to 0 and triggers the CastleDestroyed event
     void checkHealth()
     {
          if (gameData.health <= 0)
          {
               Destroy(this.gameObject);
               CastleDestroyed?.Invoke();
          }
     }
     /// <summary>
     /// Function that destribes how the object takes damage
     /// </summary>
     /// <param name="damageAmount">The amount of damage taken</param>
     void IDamagable.TakeDamage(float damageAmount, Func<EnemyController, IEnumerator> Effect)
     {
          gameData.health = gameData.health - damageAmount;
          checkHealth();
     }
}
