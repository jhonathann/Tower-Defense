using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
     public GameData gameData;
     float speed;
     int currentCheckPoint;
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
          //Set the starting position to the end of the path
          currentCheckPoint = gameData.path.Count - 1;
          this.transform.position = gameData.path[currentCheckPoint].gameObject.transform.position;
          //Sets the speed
          speed = 20;
          //Sets the initial direction vector
          direction = Vector3.Normalize(gameData.path[currentCheckPoint].gameObject.transform.position - this.transform.position);
     }
     void travelPath()
     {
          //Travels the path
          if (currentCheckPoint >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed);
               if (Vector3.Distance(this.transform.position, gameData.path[currentCheckPoint].gameObject.transform.position) < 1f)
               {
                    currentCheckPoint--;
                    //The try-catch block is just for the last case when index gets to -1
                    try
                    {
                         direction = Vector3.Normalize(gameData.path[currentCheckPoint].gameObject.transform.position - this.transform.position);
                    }
                    catch { }
               }
          }
     }

     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          if (enteringObjectCollider.gameObject.name == "Castle")
          {
               Destroy(this.gameObject);
          }
     }

}
