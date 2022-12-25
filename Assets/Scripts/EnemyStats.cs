using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that contains the information related to the stats of the enemies
/// </summary>
public class EnemyStats : MonoBehaviour
{
     //reference to the prefabs of the enemies
     private static GameObject BushPrefab = Resources.Load<GameObject>("Bush");
     private static GameObject TreePrefab = Resources.Load<GameObject>("Tree");

     /// <summary> 
     /// Dictionary containing all the information of the enemies
     /// </summary>
     /// <param name="health">Health of the enemy</param>
     /// <param name="speed">Speed of the enemy</param>
     /// <param name="difficultyScore">Enemy's score of dificulty for the dificulty calculations</param>
     /// <param name="prefab">Prefab associated with the enemy</param>
     /// /// <param name="dropRate">integer that represents the chance (in porcentage) of the enemy dropping a part</param>
     private static readonly Dictionary<EnemyType, (int health, int speed, int difficultyScore, GameObject prefab, int dropRate)> enemyAttributes = new Dictionary<EnemyType, (int health, int speed, int difficultyScore, GameObject prefab, int dropRate)> {
        {EnemyType.Bush,(health: 5,speed: 20, difficultyScore: 1, prefab: BushPrefab, dropRate: 10)},
        {EnemyType.Tree,(health: 10, speed: 10, difficultyScore: 2, prefab: TreePrefab, dropRate: 20)}
     };
     /// <summary>
     /// Function to get an enemy's health
     /// </summary>
     /// <param name="enemyType">Type of the enemy</param>
     /// <returns>The health of the enemy</returns>
     public static int GetHealth(EnemyType enemyType)
     {
          return enemyAttributes[enemyType].health;
     }
     /// <summary>
     /// Function to get an enemy's speed
     /// </summary>
     /// <param name="enemyType">Type of the enemy</param>
     /// <returns>The speed of the enemy</returns>
     public static int GetSpeed(EnemyType enemyType)
     {
          return enemyAttributes[enemyType].speed;
     }
     /// <summary>
     /// Function to get an enemy's difficultyScore
     /// </summary>
     /// <param name="enemyType">Type of the enemy</param>
     /// <returns>The difficultyScore of the enemy</returns>
     public static int GetDifficultyScore(EnemyType enemyType)
     {
          return enemyAttributes[enemyType].difficultyScore;
     }
     /// <summary>
     /// Function to get an enemy's Prefab
     /// </summary>
     /// <param name="enemyType">Type of the enemy</param>
     /// <returns>The prefab associated to the enemy</returns>
     public static GameObject GetPrefab(EnemyType enemyType)
     {
          return enemyAttributes[enemyType].prefab;
     }
     /// <summary>
     /// Function to get an enemy's droprate
     /// </summary>
     /// <param name="enemyType">Type of the enemy</param>
     /// <returns>The dropRate of the enemy</returns>
     public static int GetDropRate(EnemyType enemyType)
     {
          return enemyAttributes[enemyType].dropRate;
     }
}

/// <summary>
/// Enum for the different types of enemies that exist
/// </summary>
public enum EnemyType
{
     Bush,
     Tree
}
