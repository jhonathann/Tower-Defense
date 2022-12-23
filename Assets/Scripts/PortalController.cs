using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controlls the portal behaviour (creating of waves)
/// </summary>
public class PortalController : MonoBehaviour
{
     /// <summary>
     /// Controlls the dificulty of the level increasing its value each wave and creating more enemies accordingly
     /// </summary>
     private float difficulty;
     /// <summary>
     /// Action that handles the logic of when the nextWave is called
     /// </summary>
     public static Action NextWave;
     void Start()
     {
          difficulty = 10; //Sets the start difficulty
          NextWave += NextWaveCalled;
     }

     private void NextWaveCalled()
     {
          difficulty = difficulty * 1.2f;
          StartCoroutine(createWave());
     }
     /// <summary>
     /// Coroutine for the creation of the wave
     /// </summary>
     /// <returns> The coroutine</returns>
     IEnumerator createWave()
     {
          // Starting values to control the dificulty
          float difficultyCount = 0;
          int difficultyScore = 0;
          //While loop that stops when the dificulty is met
          while (difficultyCount + difficultyScore <= difficulty)
          {
               //Gets a new random enemytype
               EnemyType enemyType = UtilityEnum.GetRandomTypeFromAnEnum<EnemyType>();
               //Gets the difficultyScore of that type of enemy
               difficultyScore = EnemyStats.GetDifficultyScore(enemyType);
               //Updates the difficulty count
               difficultyCount += difficultyScore;
               //Instantiates the enemy according to the type
               InstantiateAndConfigureEnemy(enemyType);
               //Waits a second to generate the next enemy
               yield return new WaitForSeconds(1);
          }
     }
     /// <summary>
     /// Function that instantiates the enemy and sets his stats according to the type
     /// </summary>
     /// <param name="enemyType"></param>
     private void InstantiateAndConfigureEnemy(EnemyType enemyType)
     {
          GameObject enemyPrefab = EnemyStats.GetPrefab(enemyType);
          EnemyController enemy = Instantiate(enemyPrefab, this.transform.position, this.transform.rotation, this.transform).GetComponent<EnemyController>();
          enemy.healt = EnemyStats.GetHealth(enemyType);
          enemy.speed = EnemyStats.GetSpeed(enemyType);
     }
}
