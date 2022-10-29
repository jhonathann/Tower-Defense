using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
     public GameData gameData;
     float speed;
     public int goalCheckPoint;
     int healt = 5;
     Vector3 direction;
     private const float ENEMY_HEIGHT = 1f;
     void Start()
     {
          initializeEnemy();
     }

     void Update()
     {
          travelPath();
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
               }
          }
     }
     void initializeEnemy()
     {
          //Setting the correct name (used in the collider interactions)
          this.gameObject.name = "Enemy";
          //Setting the correct Hight so the enemy does not clip with the ground
          this.transform.position += Vector3.up * ENEMY_HEIGHT;
          //Set the starting position to one tile before the end of the path
          goalCheckPoint = gameData.path.Count - 2;
          //Set the speed
          speed = 20;
          //Set the starting mov ement direction
          direction = getXZDirection();
     }
     void travelPath()
     {
          //Travels the path
          if (goalCheckPoint >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed, Space.World);
               if (calculateXZDistance() < 1f)
               {
                    goalCheckPoint--;
                    direction = getXZDirection();
               }
          }
     }

     // gets the direction in the XZ plane
     Vector3 getXZDirection()
     {
          Vector3 direction = Vector3.Normalize(gameData.path[goalCheckPoint].gameObject.transform.position - this.transform.position);
          direction.y = 0;
          return direction;
     }

     //calculate the distance to the checkpoint ignoring the y component (height);
     float calculateXZDistance()
     {
          Vector3 pointA = this.transform.position;
          Vector3 pointB = gameData.path[goalCheckPoint].gameObject.transform.position;
          pointA.y = 0;
          pointB.y = 0;
          return Vector3.Distance(pointA, pointB);
     }
}
