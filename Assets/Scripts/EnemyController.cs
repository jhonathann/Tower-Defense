using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
     public GameObject terrain;
     List<Tile> path;
     float speed;
     int index;
     Vector3 direction;
     void Start()
     {
          path = terrain.GetComponent<TerrainController>().path;
          this.transform.position = path[path.Count - 1].gameObject.transform.position;
          speed = 20;
          index = path.Count - 1;
          direction = Vector3.Normalize(path[index].gameObject.transform.position - this.transform.position);
     }

     void Update()
     {
          if (index >= 0)
          {
               this.transform.Translate(direction * Time.deltaTime * speed);
               if (Vector3.Distance(this.transform.position, path[index].gameObject.transform.position) < 0.5f)
               {
                    path[index].gameObject.transform.position += Vector3.up;
                    index--;
                    try
                    {
                         direction = Vector3.Normalize(path[index].gameObject.transform.position - this.transform.position);
                    }
                    catch { }
               }
          }
     }
}
