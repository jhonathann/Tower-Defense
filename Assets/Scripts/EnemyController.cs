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
     void Start()
     {
          initializeEnemy();
     }

     void Update()
     {
          travelPath();
     }
     void initializeEnemy()
     {
          //Set the starting position to one tile before the end of the path
          goalCheckPoint = gameData.path.Count - 2;
          //Set the speed
          speed = 20;
          //Set the starting mov ement direction
          direction = Vector3.Normalize(gameData.path[goalCheckPoint].gameObject.transform.position - this.transform.position);
     }
     void travelPath()
     {
          //Travels the path
          if (goalCheckPoint >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed, Space.World);
               if (Vector3.Distance(this.transform.position, gameData.path[goalCheckPoint].gameObject.transform.position) < 1f)
               {
                    goalCheckPoint--;
                    direction = Vector3.Normalize(gameData.path[goalCheckPoint].gameObject.transform.position - this.transform.position);
               }
          }
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


}
