using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Script that handles the tower attack behaviour
/// </summary>
public class TowerAttackHandler : MonoBehaviour
{
     /// <summary>
     /// Action used to take the check for attack logic
     /// </summary>
     public Action CheckForAttack;
     //Prefab for the instantiation of the shots
     private GameObject shotPrefab;
     private float fireRate;
     private float damage;
     private SourceType shotType;
     /// <summary>
     /// Action that references the correct function for the tower attack type
     /// </summary>
     private Action<GameObject> Attack;
     /// <summary>
     /// Function that references the effect that will be added to a hitted enemy
     /// </summary>
     private Func<EnemyController, IEnumerator> Effect;
     private List<GameObject> enemiesInRange = new List<GameObject>();
     /// <summary>
     /// Variable that keeps track of the time since the last shot (initialized in maximum value so the tower shots immediatly when the first enemy enters)
     /// </summary>
     private float timeSinceLastShot = Mathf.Infinity;

     private void OnEnable()
     {
          this.CheckForAttack += OnCheckForAttack;
     }
     //Adds enemies to the list when they enter the collider
     private void OnTriggerEnter(Collider enteringObjectCollider)
     {
          if (enteringObjectCollider.GetComponent<EnemyController>() != null)
          {
               enemiesInRange.Add(enteringObjectCollider.gameObject);
          }
     }

     //Removes enemies from the list when they leave the collider
     private void OnTriggerExit(Collider exitingObjectCollider)
     {
          if (exitingObjectCollider.GetComponent<EnemyController>() != null)
          {
               enemiesInRange.Remove(exitingObjectCollider.gameObject);
          }
     }

     public void Setup(Part channeler, Part source, GameData gameData)
     {
          SetChannelerStats(channeler, gameData);
          SetSourceStats(source);
     }
     /// <summary>
     /// Sets the stats for the fireRate of the tower according to the channeler
     /// </summary>
     private void SetChannelerStats(Part channeler, GameData gameData)
     {
          switch (channeler.specificTypeInfo)
          {
               case ChannelerType.Area:
                    SetChannelerAreaVariables(gameData);
                    break;
               case ChannelerType.Fast:
                    SetChannelerFastVariables(gameData);
                    break;
               case ChannelerType.Strong:
                    SetChannelerStrongVariables(gameData);
                    break;
          }

          void SetChannelerAreaVariables(GameData gameData)
          {
               this.shotPrefab = gameData.towerData.AreaChannelerBolt;
               this.fireRate = TowerStats.areaChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.areaChannelerStats[channeler.rarity].damage;
               Attack += AttackArea;
          }

          void SetChannelerFastVariables(GameData gameData)
          {
               this.shotPrefab = gameData.towerData.FastChannelerBolt;
               this.fireRate = TowerStats.fastChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.fastChannelerStats[channeler.rarity].damage;
               Attack += AttackOne;
          }

          void SetChannelerStrongVariables(GameData gameData)
          {
               this.shotPrefab = gameData.towerData.StrongChannelerBolt;
               this.fireRate = TowerStats.strongChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.strongChannelerStats[channeler.rarity].damage;
               Attack += AttackOne;
          }
     }
     /// <summary>
     /// Sets the effect and the shotType according with the source
     /// </summary>
     /// <param name="source">the source part</param>
     private void SetSourceStats(Part source)
     {
          switch (source.specificTypeInfo)
          {
               case SourceType.Earth:
                    Effect = TowerStats.earthSourceStats[source.rarity];
                    this.shotType = SourceType.Earth;
                    break;
               case SourceType.Fire:
                    Effect = TowerStats.fireSourceStats[source.rarity];
                    this.shotType = SourceType.Fire;
                    break;
               case SourceType.Thunder:
                    Effect = TowerStats.thunderSourceStats[source.rarity];
                    this.shotType = SourceType.Thunder;
                    break;
               case SourceType.Water:
                    Effect = TowerStats.waterSourceStats[source.rarity];
                    this.shotType = SourceType.Water;
                    break;
          }
     }
     private void OnCheckForAttack()
     {
          //Filters Away the destroyed enemies
          enemiesInRange = enemiesInRange.Where(enemy => enemy != null).ToList();
          //Gets Ready To Shoot
          if (timeSinceLastShot >= 1 / fireRate)
          {
               if (enemiesInRange.Count == 0) return;
               GameObject target = SelectTarget();
               Attack(target);
               timeSinceLastShot = 0;
          }
          timeSinceLastShot += Time.deltaTime;
     }
     /// <summary>
     /// Instantiates the shotPrefab Gameobject and sets its stats accordingly
     /// </summary>
     /// <param name="target">The target of the shot</param>
     private void AttackOne(GameObject target)
     {
          ShotController shot = Instantiate(this.shotPrefab, this.transform.position, this.transform.rotation).GetComponent<ShotController>();
          ShotAppearance shotAppearance = shot.gameObject.GetComponent<ShotAppearance>();
          shotAppearance.type = this.shotType;
          shot.target = target;
          shot.damage = this.damage;
          shot.Effect = this.Effect;
     }
     /// <summary>
     /// Used when the tower has the area channeler
     /// </summary>
     /// <param name="target">Just used to match the signature of the action (does nothing)</param>
     private void AttackArea(GameObject target = null)
     {
          foreach (GameObject enemy in enemiesInRange)
          {
               AttackOne(enemy);
          }
     }
     /// <summary>
     /// Function that determines the target of the tower
     /// </summary>
     /// <returns>The target of the tower</returns>
     private GameObject SelectTarget()
     {
          GameObject target;
          //Filter the enemies in range, ordera them by the progress toward the castle and returns the first
          target = enemiesInRange.OrderBy(enemy => enemy.GetComponent<EnemyController>().goalCheckPoint).First();
          return target;
     }

}
