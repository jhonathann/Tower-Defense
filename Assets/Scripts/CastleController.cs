using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class to control the behaviour of the castle
/// </summary>
public class CastleController : MonoBehaviour, IDamageable
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
     void CheckHealth()
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
     void IDamageable.TakeDamage(float damageAmount, Func<EnemyController, IEnumerator> Effect, Element element)
     {
          gameData.health -= damageAmount;
          CheckHealth();
     }
}
