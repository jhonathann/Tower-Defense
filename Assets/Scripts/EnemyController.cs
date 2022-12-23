using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the behaviour of the enemies
/// </summary>
public class EnemyController : MonoBehaviour
{
     /// <summary>
     /// Reference to the gameData Scriptable object
     /// </summary>
     public GameData gameData;
     /// <summary>
     /// Speed of the enemy
     /// </summary>
     [HideInInspector]
     public int healt;
     /// <summary>
     /// Used for the direction of the movement
     /// </summary>
     [HideInInspector]
     public float speed;
     /// <summary>
     /// Variable that defines the next checkpoint for parth traveling
     /// </summary>
     [HideInInspector]
     public int goalCheckPoint;
     /// <summary>
     /// Health of the enemy
     /// </summary>

     Vector3 direction;
     /// <summary>
     /// constant of the enemi height used to avoid clipping with the terrain
     /// </summary>
     private const float ENEMY_HEIGHT = 1f;
     void Start()
     {
          InitializeEnemy();
     }

     void Update()
     {
          TravelPath();
     }
     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          //Checks if it has reached the castle
          if (enteringObjectCollider.gameObject.name == "Castle")
          {
               Destroy(this.gameObject);
          }
          if (enteringObjectCollider.gameObject.name == "Capsule")
          {
               healt--;
               if (healt <= 0)
               {
                    Destroy(this.gameObject);
                    Part part = new Part();
                    Debug.Log($"{part.rarity} {part.type} {part.specificTypeInfo}");
               }
          }
     }
     /// <summary>
     /// Sets the starting conditions of the enemy
     /// </summary>
     void InitializeEnemy()
     {
          //Setting the correct name (used in the collider interactions)
          this.gameObject.name = "Enemy";
          //Setting the correct Hight so the enemy does not clip with the ground
          this.transform.position += Vector3.up * ENEMY_HEIGHT;
          //Set the starting position to one tile before the end of the path
          goalCheckPoint = gameData.path.Count - 2;
          //Set the starting mov ement direction
          direction = GetXZDirection();
     }

     /// <summary>
     /// Updates the logic that enables the travel of the path
     /// </summary>
     void TravelPath()
     {
          //Travels the path
          if (goalCheckPoint >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed, Space.World);
               if (CalculateXZDistance() < 1f)
               {
                    goalCheckPoint--;
                    direction = GetXZDirection();
               }
          }
     }
     /// <summary>
     /// Gets the direction in the XZ plane
     /// </summary>
     /// <returns>The direction that the enemi should follow/returns>
     Vector3 GetXZDirection()
     {
          Vector3 direction = Vector3.Normalize(gameData.path[goalCheckPoint].gameObject.transform.position - this.transform.position);
          direction.y = 0;
          return direction;
     }
     /// <summary>
     /// Calculate the distance to the checkpoint ignoring the y component (height)
     /// </summary>
     /// <returns>The distance beetween the 2 points</returns>
     float CalculateXZDistance()
     {
          Vector3 pointA = this.transform.position;
          Vector3 pointB = gameData.path[goalCheckPoint].gameObject.transform.position;
          pointA.y = 0;
          pointB.y = 0;
          return Vector3.Distance(pointA, pointB);
     }
}
