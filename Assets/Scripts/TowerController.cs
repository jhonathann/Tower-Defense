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
     private GameObject shotPrefab;
     private float fireRate;
     private float damage;
     private List<GameObject> enemiesInRange = new List<GameObject>();
     /// <summary>
     /// Variable that keeps track of the time since the last shot (initialized in maximum value so the tower shots immediatly when the first enemy enters)
     /// </summary>
     private float timeSinceLastShot = Mathf.Infinity;
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
     /// <summary>
     /// Sets the stats for the fireRate of the tower according to the channalizer
     /// </summary>
     private void SetChannalizerStats()
     {
          switch (channalizer.specificTypeInfo)
          {
               case ChannalizerType.Area:
                    SetChannalizerAreaVariables();
                    break;
               case ChannalizerType.Fast:
                    SetChannalizerFastVariables();
                    break;
               case ChannalizerType.Strong:
                    SetChannalizerStrongVariables();
                    break;
          }

          void SetChannalizerAreaVariables()
          {
               this.shotPrefab = this.gameData.towerData.AreaChannalizerBolt;
               this.fireRate = TowerStats.areaChannalizerStats[channalizer.rarity].fireRate;
               this.damage = TowerStats.areaChannalizerStats[channalizer.rarity].damage;
          }

          void SetChannalizerFastVariables()
          {
               this.shotPrefab = this.gameData.towerData.FastChannalizerBolt;
               this.fireRate = TowerStats.fastChannalizerStats[channalizer.rarity].fireRate;
               this.damage = TowerStats.fastChannalizerStats[channalizer.rarity].damage;
          }

          void SetChannalizerStrongVariables()
          {
               this.shotPrefab = this.gameData.towerData.StrongChannalizerBolt;
               this.fireRate = TowerStats.strongChannalizerStats[channalizer.rarity].fireRate;
               this.damage = TowerStats.strongChannalizerStats[channalizer.rarity].damage;
          }
     }
     /// <summary>
     /// Creates the hitZones of the tower based on the structureType
     /// </summary>
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
               hitZone.transform.localScale = Vector3.one * TowerStats.circularStructureStats[structure.rarity] * 2;
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
          //Filters Away the destroyed enemies
          enemiesInRange = enemiesInRange.Where(enemy => enemy != null).ToList();
          //Gets Ready To Shoot
          if (timeSinceLastShot >= 1 / fireRate)
          {
               if (enemiesInRange.Count == 0) return;
               GameObject target = SelectTarget();
               attack(target);
               timeSinceLastShot = 0;
          }
          timeSinceLastShot += Time.deltaTime;
     }
     /// <summary>
     /// Instantiates the shotPrefab Gameobject and sets its stats accordingly
     /// </summary>
     /// <param name="target">The target of the shot</param>
     private void attack(GameObject target)
     {
          GameObject shoot = Instantiate(this.shotPrefab, this.transform.position, this.transform.rotation);
          shoot.GetComponent<ShotController>().target = target;
          shoot.GetComponent<ShotController>().damage = this.damage;
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
     //Adds enemies to the list when they enter the collider
     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          if (enteringObjectCollider.GetComponent<EnemyController>() != null)
          {
               enemiesInRange.Add(enteringObjectCollider.gameObject);
          }
     }
     //Removes enemies from the list when they leave the collider
     void OnTriggerExit(Collider exitingObjectCollider)
     {
          if (exitingObjectCollider.GetComponent<EnemyController>() != null)
          {
               enemiesInRange.Remove(exitingObjectCollider.gameObject);
          }
     }
}
