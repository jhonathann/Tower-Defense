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
                    CreateBeamHitZoneCollider();
                    CreateBeamHitZoneMesh();
                    break;
               case StructureType.Circular:
                    CreateCircleHitZoneCollider();
                    CreateCircleHitZoneMesh();
                    break;
               case StructureType.Cross:
                    CreateCrossHitZoneCollider();
                    CreateCrossHitZoneMesh();
                    break;
          }
          void CreateBeamHitZoneCollider()
          {
               //Add the collider component
               BoxCollider beamCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               beamCollider.isTrigger = true;
               //Properly adjust the center of the collider
               beamCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, TowerStats.beamStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               beamCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.beamStructureStats[structure.rarity]);
          }
          void CreateCircleHitZoneCollider()
          {
               //Add the collider component
               SphereCollider circularCollider = this.gameObject.AddComponent<SphereCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               circularCollider.isTrigger = true;
               //Properly adjust the center of the collider
               circularCollider.center = new Vector3(0, STRUCTURE_HEIGHT, 0);
               //Properly adjust the radius of the collider
               circularCollider.radius = TowerStats.circularStructureStats[structure.rarity];
          }
          void CreateCrossHitZoneCollider()
          {
               //Add Collider at the front
               //Add the collider component
               BoxCollider frontCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               frontCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               frontCrossCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               frontCrossCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Add Collider at the back
               //Add the collider component
               BoxCollider backCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               backCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               backCrossCollider.center = new Vector3(0, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, -1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               backCrossCollider.size = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Add collider to the right
               //Add the collider component
               BoxCollider rightCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               rightCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               rightCrossCollider.center = new Vector3(TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, 0);
               //Properly adjust the size of the collider
               rightCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
               //Add collider to the left
               //Add the collider component
               BoxCollider leftCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               leftCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               leftCrossCollider.center = new Vector3(-1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TOWER_WIDTH / 2, STRUCTURE_HEIGHT + TOWER_HEIGHT / 2, 0);
               //Properly adjust the size of the collider
               leftCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
          }
          void CreateBeamHitZoneMesh()
          {
               //Creates the primitive
               GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               //Sets the primitive as a child of the tower
               hitZone.transform.SetParent(this.transform);
               //Destroy the primitive's collider (created by default)
               Destroy(hitZone.GetComponent<Collider>());
               //Properly adjust the position
               hitZone.transform.position = this.transform.position + Vector3.forward * (TowerStats.beamStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2) + Vector3.up * (STRUCTURE_HEIGHT + TOWER_HEIGHT / 2);
               //Properly adjust the scale
               hitZone.transform.localScale = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.beamStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
          }
          void CreateCircleHitZoneMesh()
          {
               GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.up * STRUCTURE_HEIGHT;
               hitZone.transform.localScale = Vector3.one * TowerStats.circularStructureStats[structure.rarity];
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
          }
          void CreateCrossHitZoneMesh()
          {
               //Add front zone
               GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.forward * (TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2) + Vector3.up * (STRUCTURE_HEIGHT + TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               //Add back zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.back * (TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2) + Vector3.up * (STRUCTURE_HEIGHT + TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TOWER_WIDTH, TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               //Add right zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.right * (TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2) + Vector3.up * (STRUCTURE_HEIGHT + TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               //Add left zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.left * (TowerStats.crossStructureStats[structure.rarity] / 2 + TOWER_WIDTH / 2) + Vector3.up * (STRUCTURE_HEIGHT + TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity], TOWER_HEIGHT, TOWER_WIDTH);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
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
     void OnTriggerEnter(Collider collider)
     {
          Debug.Log("Works");
     }
}
