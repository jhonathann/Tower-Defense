using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
     public GameObject enemy;
     public List<Tile> path;
     void Start()
     {
          StartCoroutine(createWave(enemy));
     }

     void Update()
     {

     }

     IEnumerator createWave(GameObject enemy)
     {
          int numberOfEnemies = 0;
          while (numberOfEnemies < 10)
          {
               Instantiate(enemy, this.transform);
               numberOfEnemies++;
               yield return new WaitForSeconds(1);
          }
     }
}
