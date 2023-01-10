using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class handels the behaviour of the towers
/// </summary>
public class TowerController : MonoBehaviour
{
     /// <summary>
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
     //References to the parts of the tower
     private Part channalizer;
     private Part structure;
     private Part source;
     //Constant for the correct instantiation of the prefabs
     private const float CHANNALIZER_HEIGHT = 18.0f;
     private const float SOURCE_HEIGHT = 8.0f;
     private const float STRUCTURE_HEIGHT = 1.0f;
     private const float TOWER_HEIGHT = 20.0f;
     private const float TOWER_WIDTH = 10.0f;
     public GameObject magicBolt;
     private float timeSince;
     private void Start()
     {
          //Set the gameobject mask so it doesnt trigger the onmouse events
          this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
          StoreParts();
          InstantiateModels();
          SetChannalizerStats();
          SetStructureStats();
          SetSourceStats();
     }
     /// <summary>
     /// Stores the references to the parts of the tower
     /// </summary>
     private void StoreParts()
     {
          this.channalizer = gameData.towerData.channalizer;
          this.structure = gameData.towerData.structure;
          this.source = gameData.towerData.source;
     }
     /// <summary>
     /// Instantiate the models of the parts
     /// </summary>
     private void InstantiateModels()
     {
          Instantiate(gameData.towerData.GetChannalizerPrefab(), this.transform.position + Vector3.up * CHANNALIZER_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetStructurePrefab(), this.transform.position + Vector3.up * STRUCTURE_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetSourcePrefab(), this.transform.position + Vector3.up * SOURCE_HEIGHT, Quaternion.identity, this.transform);
     }
     private void SetChannalizerStats()
     {
          switch (channalizer.specificTypeInfo)
          {
               case ChannalizerType.Area: break;
               case ChannalizerType.Fast: break;
               case ChannalizerType.Strong: break;
          }
     }
     private void SetStructureStats()
     {
          switch (structure.specificTypeInfo)
          {
               case StructureType.Beam:
                    BoxCollider beamCollider = this.gameObject.AddComponent<BoxCollider>();
                    beamCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, TowerStats.beamStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2);
                    beamCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.beamStructureStats[structure.rarity]);
                    break;
               case StructureType.Circular:
                    SphereCollider circularCollider = this.gameObject.AddComponent<SphereCollider>();
                    circularCollider.center = new Vector3(0, STRUCTURE_HEIGHT, 0);
                    circularCollider.radius = TowerStats.circularStructureStats[structure.rarity];
                    break;
               case StructureType.Cross:
                    //Add Collider at the front
                    BoxCollider frontCrossCollider = this.gameObject.AddComponent<BoxCollider>();
                    frontCrossCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2);
                    frontCrossCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
                    //Add Collider at the back
                    BoxCollider backCrossCollider = this.gameObject.AddComponent<BoxCollider>();
                    backCrossCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, -1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TOWER_WIDTH / 2);
                    backCrossCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
                    //Add collider to the right
                    BoxCollider rightCrossCollider = this.gameObject.AddComponent<BoxCollider>();
                    rightCrossCollider.center = new Vector3(TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, 0);
                    rightCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
                    //Add collider to the left
                    BoxCollider leftCrossCollider = this.gameObject.AddComponent<BoxCollider>();
                    leftCrossCollider.center = new Vector3(-1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TOWER_WIDTH / 2, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, 0);
                    leftCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
                    break;
          }
     }
     private void SetSourceStats()
     {
          switch (source.specificTypeInfo)
          {
               case SourceType.Earth: break;
               case SourceType.Fire: break;
               case SourceType.Thunder: break;
               case SourceType.Water: break;
          }
     }
     private void Update()
     {
          GameObject target = SelectTarget();
          if (timeSince >= 0.5)
          {
               attack(target);
               timeSince = 0;
          }
          timeSince += Time.deltaTime;
     }
     private void attack(GameObject target)
     {
          if (target == null)
          {
               return;
          }
          GameObject shoot = Instantiate(magicBolt, this.transform.position, this.transform.rotation);
          shoot.GetComponent<MagicBoltController>().target = target;
     }
     private GameObject SelectTarget()
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
     private bool isInRange(GameObject objective, int alcance)
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
