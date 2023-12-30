using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Class that controlls the portal behaviour (creating of waves)
/// </summary>
public class PortalController : MonoBehaviour
{
     /// <summary>
     /// List that contains the performance and the element of each enemy killed in that wave
     /// </summary>
     /// <param name="goalReached">The point reached</param>
     /// <param name="element">the element of the enemy</param>
     private static List<(int goalReached, Element element)> waveResult = new();
     /// <summary>
     /// Reference to the gameData
     /// </summary>
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
     /// <summary>
     /// Variable to know when the wave is being displayed (to avoid overlaping waves)
     /// </summary>
     private bool waveActive = false;
     /// <summary>
     /// Tuple that contains the probability of ocurrence of each element
     /// </summary>
     /// <param name="fire">the fire probability</param>
     /// <param name="water">the water probability</param>
     /// <param name="earth">the earth probability</param>
     /// <param name="thunder">the thunder probability</param>
     /// <returns></returns>
     private (float fire, float water, float earth, float thunder) elementProbabilities = (25.0f, 25.0f, 25.0f, 25.0f);

     void Start()
     {
          difficulty = 10; //Sets the start difficulty
          NextWave += NextWaveCalled;
     }
     void Update()
     {
          if (waveActive) return;
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
          // Alter the probabilities of the elments acording to the wave result (takes the 10 best and worst and selects the element that is the mode between those 10)
          if (waveResult.Count != 0)
          {
               Element bestElement = waveResult
                    .OrderBy(enemy => enemy.goalReached)
                    .Take(10)
                    .GroupBy(enemy => enemy.element)
                    .OrderByDescending(group => group.Count())
                    .First()
                    .Key;
               Element worstElement = waveResult
                    .OrderByDescending(enemy => enemy.goalReached)
                    .Take(10)
                    .GroupBy(enemy => enemy.element)
                    .OrderByDescending(group => group.Count())
                    .First()
                    .Key;
               switch (bestElement)
               {
                    case Element.Water:
                         elementProbabilities.water++;
                         break;
                    case Element.Fire:
                         elementProbabilities.fire++;
                         break;
                    case Element.Thunder:
                         elementProbabilities.thunder++;
                         break;
                    case Element.Earth:
                         elementProbabilities.earth++;
                         break;
               }
               switch (worstElement)
               {
                    case Element.Water:
                         elementProbabilities.water--;
                         break;
                    case Element.Fire:
                         elementProbabilities.fire--;
                         break;
                    case Element.Thunder:
                         elementProbabilities.thunder--;
                         break;
                    case Element.Earth:
                         elementProbabilities.earth--;
                         break;
               }
               waveResult.Clear();
          }
          StartCoroutine(CreateWave());
     }
     /// <summary>
     /// Coroutine for the creation of the wave
     /// </summary>
     /// <returns> The coroutine</returns>
     IEnumerator CreateWave()
     {
          waveActive = true;
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
          waveActive = false;
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
          enemy.Element = assignElement();
          assignElement();

          Element assignElement()
          {
               int chance = UnityEngine.Random.Range(0, 100);
               if (chance < elementProbabilities.fire) return Element.Fire;
               if (chance < elementProbabilities.fire + elementProbabilities.water) return Element.Water;
               if (chance < elementProbabilities.fire + elementProbabilities.water + elementProbabilities.earth) return Element.Earth;
               return Element.Thunder;
          }
     }
     /// <summary>
     /// Method that allows enemies to add themselves to the waveResult when they die
     /// </summary>
     /// <param name="deadEnemy">the deadEnemy</param>
     public static void UpdateWaveResult(EnemyController deadEnemy)
     {
          waveResult.Add((deadEnemy.goalCheckPoint, deadEnemy.Element));
     }
}
