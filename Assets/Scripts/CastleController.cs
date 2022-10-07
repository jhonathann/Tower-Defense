using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
     //Access to the gameData
     public GameData gameData;
     void Update()
     {
          checkHealth();
     }

     //Destroys the gameObject when the player health gets to 0
     void checkHealth()
     {
          if (gameData.health <= 0)
          {
               Destroy(this.gameObject);
          }
     }
     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          //Only decreases health if the entering collider belongs to an enemy (this is to considere other adjecent tiles that also have colliders)
          if (enteringObjectCollider.gameObject.name == "Enemy(Clone)")
          {
               gameData.health--;
          }
     }
}
