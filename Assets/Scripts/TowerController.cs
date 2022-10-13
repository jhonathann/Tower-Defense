using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerController : MonoBehaviour
{
     public GameObject magicBolt;
     float timeSince;
     void Start()
     {
     }

     void Update()
     {
          GameObject target = SelectTarget();
          if (timeSince >= 0.5)
          {
               attack(target);
               timeSince = 0;
          }
          timeSince += Time.deltaTime;
     }
     void attack(GameObject target)
     {
          if (target == null)
          {
               return;
          }
          GameObject shoot = Instantiate(magicBolt, this.transform.position, this.transform.rotation);
          shoot.GetComponent<MagicBoltController>().target = target;
     }
     GameObject SelectTarget()
     {
          GameObject target = null;
          //Get All the enemies
          GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
          //Filter the enemies in range and order them by the progress toward the castle
          enemies = enemies.Where(enemy => isInRange(enemy, 30) == true).OrderBy(enemy => enemy.GetComponent<EnemyController>().goalCheckPoint).ToArray();
          //Check if there are enemies in range
          if (enemies.Count() > 0)
          {
               //Select the enemy that has advanced the most
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
