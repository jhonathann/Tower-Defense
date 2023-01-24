using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// This class handels the behaviour of the towers
/// </summary>
public class TowerController : MonoBehaviour
{
     private enum TowerState
     {
          Unplaced,
          Placed,
     }
     private TowerState state = TowerState.Unplaced;
     public static Quaternion towerRotation = Quaternion.identity;
     /// <summary>
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
     //References to the parts of the tower
     private Part channeler;
     private Part structure;
     private Part source;
     //Constant for the correct instantiation of the prefabs
     private const float CHANNELER_HEIGHT = 18.0f;
     private const float SOURCE_HEIGHT = 8.0f;
     private const float STRUCTURE_HEIGHT = 1.0f;
     private const float TOWER_HEIGHT = 20.0f;
     private const float TOWER_WIDTH = 10.0f;
     //Prefab for the instantiation of the shots
     private GameObject shotPrefab;
     private float fireRate;
     private float damage;
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
          //Subscribes to the TowerPlaced event
          TileController.TowerPlaced += OnTowerPlaced;
     }
     private void Start()
     {
          //Set the gameobject mask so it doesnt trigger the onmouse events
          this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
          StoreParts();
          InstantiateModels();
          SetChannelerStats();
          SetStructureStats();
          SetSourceStats();
          //Sets the rotation to the saved rotation (must be done last so the hitZones rotate adequately)
          this.transform.rotation = towerRotation;
     }
     /// <summary>
     /// Stores the references to the parts of the tower
     /// </summary>
     private void StoreParts()
     {
          this.channeler = gameData.towerData.channeler;
          this.structure = gameData.towerData.structure;
          this.source = gameData.towerData.source;
     }
     /// <summary>
     /// Instantiate the models of the parts
     /// </summary>
     private void InstantiateModels()
     {
          Instantiate(gameData.towerData.GetChannelerPrefab(), this.transform.position + Vector3.up * CHANNELER_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetStructurePrefab(), this.transform.position + Vector3.up * STRUCTURE_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetSourcePrefab(), this.transform.position + Vector3.up * SOURCE_HEIGHT, Quaternion.identity, this.transform);
     }
     /// <summary>
     /// Sets the stats for the fireRate of the tower according to the channeler
     /// </summary>
     private void SetChannelerStats()
     {
          switch (channeler.specificTypeInfo)
          {
               case ChannelerType.Area:
                    SetChannelerAreaVariables();
                    break;
               case ChannelerType.Fast:
                    SetChannelerFastVariables();
                    break;
               case ChannelerType.Strong:
                    SetChannelerStrongVariables();
                    break;
          }

          void SetChannelerAreaVariables()
          {
               this.shotPrefab = this.gameData.towerData.AreaChannelerBolt;
               this.fireRate = TowerStats.areaChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.areaChannelerStats[channeler.rarity].damage;
               Attack += AttackArea;
          }

          void SetChannelerFastVariables()
          {
               this.shotPrefab = this.gameData.towerData.FastChannelerBolt;
               this.fireRate = TowerStats.fastChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.fastChannelerStats[channeler.rarity].damage;
               Attack += AttackOne;
          }

          void SetChannelerStrongVariables()
          {
               this.shotPrefab = this.gameData.towerData.StrongChannelerBolt;
               this.fireRate = TowerStats.strongChannelerStats[channeler.rarity].fireRate;
               this.damage = TowerStats.strongChannelerStats[channeler.rarity].damage;
               Attack += AttackOne;
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
               case SourceType.Earth:
                    Effect = TowerStats.earthSourceStats[source.rarity];
                    break;
               case SourceType.Fire:
                    Effect = TowerStats.fireSourceStats[source.rarity];
                    break;
               case SourceType.Thunder:
                    Effect = TowerStats.thunderSourceStats[source.rarity];
                    break;
               case SourceType.Water:
                    Effect = TowerStats.waterSourceStats[source.rarity];
                    break;
          }
     }
     private void Update()
     {
          if (state == TowerState.Unplaced)
          {
               TowerUnplacedUpdate();
          }
          if (state == TowerState.Placed)
          {
               TowerPlacedUpdate();
          };
     }
     private void TowerUnplacedUpdate()
     {
          if (Input.GetKeyDown(KeyCode.R))
          {
               //Rotates the tower
               this.transform.Rotate(new Vector3(0, 90, 0));
               //Saves the new Rotation so the next instantiated tower is instantiated with that rotation
               towerRotation = this.transform.rotation;
          }
     }
     private void TowerPlacedUpdate()
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

     void OnTowerPlaced()
     {
          this.state = TowerState.Placed;
     }
}
