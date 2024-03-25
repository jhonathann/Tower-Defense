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
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
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
     /// Wave with the information of the enemies
     /// </summary>
     private Wave wave;
     void Start()
     {
          wave = new Wave();//wave initialization
          NextWave += NextWaveCalled;
     }
     void Update()
     {
          if (!wave.waveFinished) return;//starts the timer when all the enemies of the previous wave have been defeated
          if (nextWaveTimer.IsDone)
          {
               nextWaveTimer.StartTimer(10, () => NextWave?.Invoke(), this);
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
          wave.waveFinished = false;
          wave.Mutate();
          StartCoroutine(CreateWave(wave));
     }
     /// <summary>
     /// Coroutine for the creation of the wave
     /// </summary>
     /// <returns> The coroutine</returns>
     IEnumerator CreateWave(Wave wave)
     {
          //While loop that stops when the dificulty is met
          foreach (WaveMember member in wave.members)
          {
               //Instantiates the enemy according to the type
               InstantiateAndConfigureEnemy(member);
               //Waits a second to generate the next enemy
               yield return new WaitForSeconds(1);
          }
     }
     /// <summary>
     /// Function that instantiates the enemy and sets his stats according to the type
     /// </summary>
     /// <param name="enemyType"></param>
     private void InstantiateAndConfigureEnemy(WaveMember member)
     {
          EnemyController enemy = Instantiate(Resources.Load<GameObject>("Bush"), this.transform.position, this.transform.rotation, this.transform).GetComponent<EnemyController>();
          enemy.InitializeEnemy(member);
     }
}
