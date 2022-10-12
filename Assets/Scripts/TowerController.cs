using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerController : MonoBehaviour
{

     void Start()
     {
     }

     void Update()
     {
          GameObject target = SelectTarget();
     }

     GameObject SelectTarget()
     {
          GameObject target = null;
          GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
          enemies = enemies.Where(enemy => isInRange(enemy, 30) == true).OrderBy(enemy => enemy.GetComponent<EnemyController>().goalCheckPoint).ToArray();
          //Check if there are enemies in range
          if (enemies.Count() > 0)
          {
               target = enemies[0];
          }
          return target;
     }
     //Function to know if the enemy is within the range of the tower
     bool isInRange(GameObject objective, int alcance)
     {
          if (Vector3.Distance(this.transform.position, objective.transform.position) < alcance)
          {
               return true;
          }
          else
          {
               return false;
          }

     }
}
