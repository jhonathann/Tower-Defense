using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
     public List<Tile> path;
     float speed;
     int currentCheckPoint;
     Vector3 direction;
     void Start()
     {
          //Get the path to travel
          path = GameObject.Find("Terrain").GetComponent<TerrainController>().path;
          //Set the starting position to the end of the path
          currentCheckPoint = path.Count - 1;
          this.transform.position = path[currentCheckPoint].gameObject.transform.position;
          speed = 20;
          direction = Vector3.Normalize(path[currentCheckPoint].gameObject.transform.position - this.transform.position);
     }

     void Update()
     {
          if (currentCheckPoint >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed);
               if (Vector3.Distance(this.transform.position, path[currentCheckPoint].gameObject.transform.position) < 0.5f)
               {
                    currentCheckPoint--;
                    try
                    {
                         direction = Vector3.Normalize(path[currentCheckPoint].gameObject.transform.position - this.transform.position);
                    }
                    catch { }
               }
          }
          if (currentCheckPoint == 0)
          {
               Destroy(this.gameObject);
          }
     }
}
