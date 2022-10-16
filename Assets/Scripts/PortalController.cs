using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
     public GameObject enemy;
     public GameData gameData;
     void Start()
     {

     }

     void Update()
     {
          if (gameData.sendNextWave)
          {
               StartCoroutine(createWave(enemy));
               gameData.sendNextWave = false;
          }
     }

     IEnumerator createWave(GameObject enemy)
     {
          int numberOfEnemies = 0;
          while (numberOfEnemies < 10)
          {
               Instantiate(enemy, this.transform.position, this.transform.rotation, this.transform);
               numberOfEnemies++;
               yield return new WaitForSeconds(1);
          }
     }
}
