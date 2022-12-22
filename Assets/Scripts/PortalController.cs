using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
     public GameObject enemy;
     public GameData gameData;
     private int waveCounter;
     private int waveMultiplier;
     void Start()
     {
          waveCounter = 1;
          waveMultiplier = 5;
     }

     void Update()
     {
          if (gameData.sendNextWave)
          {
               StartCoroutine(createWave(enemy));
               gameData.sendNextWave = false;
               waveCounter++;
          }
     }

     IEnumerator createWave(GameObject enemy)
     {
          int numberOfEnemies = Random.Range(waveCounter * waveMultiplier / 2, waveCounter * waveMultiplier);

          for (int i = 0; i < numberOfEnemies; i++)
          {
               Instantiate(enemy, this.transform.position, this.transform.rotation, this.transform);
               i++;
               yield return new WaitForSeconds(1);
          }
     }
}
