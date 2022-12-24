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

     [HideInInspector]
     public EnemyType type;
     /// <summary>
     /// Health of the enemy
     /// </summary>
     private int healt;
     /// /// <summary>
     /// Speed of the enemy
     /// </summary>
     private float speed;
     /// <summary>
     /// Chance of the enemy generating a part
     /// </summary>
     private int dropRate;
     /// <summary>
     /// Variable that defines the next checkpoint for parth traveling
     /// </summary>
     [HideInInspector]
     public int goalCheckPoint;
     /// <summary>
     /// Used for the direction of the movement
     /// </summary>
     Vector3 direction;
     /// <summary>
     /// constant of the enemy height used to avoid clipping with the terrain
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
                    TryGeneratePart();
               }
          }
     }
     /// <summary>
     /// Sets the starting conditions of the enemy
     /// </summary>
     void InitializeEnemy()
     {
          //Set the stats of the enemy
          this.healt = EnemyStats.GetHealth(type);
          this.speed = EnemyStats.GetSpeed(type);
          this.dropRate = EnemyStats.GetDropRate(type);
          //Setting the correct name (used in the collider interactions)
          this.gameObject.name = "Enemy";
          //Setting the correct Hight so the enemy does not clip with the ground
          this.transform.position += Vector3.up * ENEMY_HEIGHT;
          //Set the starting position to one tile before the end of the path
          goalCheckPoint = gameData.path.Count - 2;
          //Set the starting mov ement direction
          direction = GetXZDirection();
     }

     void TryGeneratePart()
     {
          int chance = Random.Range(1, 100);
          if (chance > dropRate) return;
          Part part = new Part();
          Debug.Log($"{part.rarity} {part.type} {part.specificTypeInfo}");
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
