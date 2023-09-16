using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controlls the portal behaviour (creating of waves)
/// </summary>
public class PortalController : MonoBehaviour
{
     public GameData gameData;
     /// <summary>
     /// Controlls the dificulty of the level increasing its value each wave and creating more enemies accordingly
     /// </summary>
     private float difficulty;
     /// <summary>
     /// Action that handles the logic of when the nextWave is called
     /// </summary>
     public static Action NextWave;
     /// <summary>
     /// Timer that handels the countdown timer for the waves
     /// </summary>
     /// <returns></returns>
     private readonly Timer nextWaveTimer = new();
     void Start()
     {
          difficulty = 10; //Sets the start difficulty
          NextWave += NextWaveCalled;
     }
     void Update()
     {
          if (nextWaveTimer.IsDone)
          {
               nextWaveTimer.StartTimer(30, () => NextWave?.Invoke(), this);
          }
          gameData.timerString = nextWaveTimer.remainingTimeAsString;
     }
     /// <summary>
     /// Removes the subscription to the static Action when the gameObject is destroyed (when the scene is reloaded)
     /// </summary>
     void OnDestroy()
     {
          NextWave -= NextWaveCalled;
     }
     private void NextWaveCalled()
     {
          difficulty *= 1.2f;
          StartCoroutine(CreateWave());
     }
     /// <summary>
     /// Coroutine for the creation of the wave
     /// </summary>
     /// <returns> The coroutine</returns>
     IEnumerator CreateWave()
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
          enemy.type = enemyType;
     }
}
