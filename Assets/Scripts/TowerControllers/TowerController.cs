using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handels the behaviour of the towers
/// </summary>
public class TowerController : MonoBehaviour
{
     private TowerSelectionHandler towerSelectionHandler;
     private TowerAttackHandler towerAttackHandler;
     private TowerRotationHandler towerRotationHandler;
     private enum TowerState
     {
          Unplaced,
          Placed
     }
     private TowerState state = TowerState.Unplaced;
     /// <summary>
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
     //References to the parts of the tower
     public Part channeler;
     public Part structure;
     public Part source;
     /// <summary>
     /// reference to the hitzoneGameObjects that show the range of the tower
     /// </summary>
     private readonly List<GameObject> hitZoneGameObjects = new();

     private void Awake()
     {
          // Adds the components to the tower gameobject
          towerSelectionHandler = this.gameObject.AddComponent<TowerSelectionHandler>();
          towerAttackHandler = this.gameObject.AddComponent<TowerAttackHandler>();
          towerRotationHandler = this.gameObject.AddComponent<TowerRotationHandler>();
     }
     private void OnEnable()
     {
          //Subscribes to the TowerPlaced event
          TileController.TowerPlaced += OnTowerPlaced;
     }
     private void Start()
     {
          //Set the gameobject mask so it doesnt trigger the onmouse events (used while placing the tower so the colliders dont overlap)
          this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
          StoreParts();
          InstantiateModels();
          SetStructureStats();
          towerAttackHandler.Setup(this.channeler, this.source, this.gameData);
          towerSelectionHandler.Setup(AddSelectionCollider(), hitZoneGameObjects, gameData.towerUiData.GetWorldUIPrefab());
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
          Instantiate(gameData.towerData.GetChannelerPrefab(), this.transform.position + Vector3.up * TowerStats.CHANNELER_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetStructurePrefab(), this.transform.position + Vector3.up * TowerStats.STRUCTURE_HEIGHT, Quaternion.identity, this.transform);
          Instantiate(gameData.towerData.GetSourcePrefab(), this.transform.position + Vector3.up * TowerStats.SOURCE_HEIGHT, Quaternion.identity, this.transform);
     }

     private Collider AddSelectionCollider()
     {
          //Add the collider component
          BoxCollider selectionCollider = this.gameObject.AddComponent<BoxCollider>();
          //Sets the is trigger so it fires OnTrigger Events
          selectionCollider.isTrigger = true;
          //Properly adjust the center of the collider
          selectionCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
          //Properly adjust the size of the collider
          selectionCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
          return selectionCollider;
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
               beamCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, TowerStats.beamStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               beamCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.beamStructureStats[structure.rarity]);
          }
          void CreateCircleHitZoneCollider()
          {
               //Add the collider component
               SphereCollider circularCollider = this.gameObject.AddComponent<SphereCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               circularCollider.isTrigger = true;
               //Properly adjust the center of the collider
               circularCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT, 0);
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
               frontCrossCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               frontCrossCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Add Collider at the back
               //Add the collider component
               BoxCollider backCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               backCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               backCrossCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, -1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TowerStats.TOWER_WIDTH / 2);
               //Properly adjust the size of the collider
               backCrossCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Add collider to the right
               //Add the collider component
               BoxCollider rightCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               rightCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               rightCrossCollider.center = new Vector3(TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
               //Properly adjust the size of the collider
               rightCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
               //Add collider to the left
               //Add the collider component
               BoxCollider leftCrossCollider = this.gameObject.AddComponent<BoxCollider>();
               //Sets the is trigger so it fires OnTrigger Events
               leftCrossCollider.isTrigger = true;
               //Properly adjust the center of the collider
               leftCrossCollider.center = new Vector3(-1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TowerStats.TOWER_WIDTH / 2, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
               //Properly adjust the size of the collider
               leftCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity], TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
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
               hitZone.transform.position = this.transform.position + Vector3.forward * (TowerStats.beamStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2) + Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
               //Properly adjust the scale
               hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.beamStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               //Adds the gameobject to the hitzoneGameObjects list so it can be accesed
               hitZoneGameObjects.Add(hitZone);
          }
          void CreateCircleHitZoneMesh()
          {
               GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.up * TowerStats.STRUCTURE_HEIGHT;
               hitZone.transform.localScale = Vector3.one * TowerStats.circularStructureStats[structure.rarity] * 2;
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               hitZoneGameObjects.Add(hitZone);
          }
          void CreateCrossHitZoneMesh()
          {
               //Add front zone
               GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.forward * (TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2) + Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               hitZoneGameObjects.Add(hitZone);
               //Add back zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.back * (TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2) + Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.crossStructureStats[structure.rarity]);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               hitZoneGameObjects.Add(hitZone);
               //Add right zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.right * (TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2) + Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity], TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               hitZoneGameObjects.Add(hitZone);
               //Add left zone
               hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
               hitZone.transform.SetParent(this.transform);
               Destroy(hitZone.GetComponent<Collider>());
               hitZone.transform.position = this.transform.position + Vector3.left * (TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2) + Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
               hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity], TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
               //Sets the shader to the material
               hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shader Graphs/Blinking");
               hitZoneGameObjects.Add(hitZone);
          }
     }
     private void Update()
     {
          if (state == TowerState.Unplaced)
          {
               towerRotationHandler.RotateTower?.Invoke();
          }
          if (state == TowerState.Placed)
          {
               towerAttackHandler.CheckForAttack?.Invoke();
          };
     }

     void OnTowerPlaced()
     {
          this.state = TowerState.Placed;
          //Unsubscribes from the towerPlaced event 
          TileController.TowerPlaced -= OnTowerPlaced;
     }
}