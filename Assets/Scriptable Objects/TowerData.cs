using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Scriptable Object to hold the reference of the tower to be created and the references to the prefabs of the parts
/// </summary>
[CreateAssetMenu]
public class TowerData : ScriptableObject
{
     [SerializeField]
     private GameData gameData;
     public GameObject tower;
     public Part channeler;
     public Part structure;
     public Part source;
     [SerializeField]
     private GameObject areaChannelerPrefab;
     [SerializeField]
     private GameObject fastChannelerPrefab;
     [SerializeField]
     private GameObject strongChannelerPrefab;
     [SerializeField]
     private GameObject beamStructurePrefab;
     [SerializeField]
     private GameObject circularStructurePrefab;
     [SerializeField]
     private GameObject crossStructurePrefab;
     [SerializeField]
     private GameObject earthSourcePrefab;
     [SerializeField]
     private GameObject fireSourcePrefab;
     [SerializeField]
     private GameObject waterSourcePrefab;
     [SerializeField]
     private GameObject thunderSourcePrefab;
     [field: SerializeField]
     public GameObject AreaChannelerBolt { get; private set; }
     [field: SerializeField]
     public GameObject FastChannelerBolt { get; private set; }
     [field: SerializeField]
     public GameObject StrongChannelerBolt { get; private set; }
     private Dictionary<Enum, GameObject> enumToGameObject;
     private void OnEnable()
     {
          HUDController.CreateTowerTriggered += CreateTower;
          //Load the prefabs to the dictionary
          enumToGameObject = new Dictionary<Enum, GameObject>
          {{ChannelerType.Area,areaChannelerPrefab},
          {ChannelerType.Fast,fastChannelerPrefab},
          {ChannelerType.Strong,strongChannelerPrefab},
          {StructureType.Beam,beamStructurePrefab},
          {StructureType.Circular,circularStructurePrefab},
          {StructureType.Cross,crossStructurePrefab},
          {Element.Earth,earthSourcePrefab},
          {Element.Fire,fireSourcePrefab},
          {Element.Thunder,thunderSourcePrefab},
          {Element.Water,waterSourcePrefab}};
     }
     /// <summary>
     /// Function that sets this parts to the tower Scriptable Object when there are 3 parts
     /// </summary>
     private void CreateTower()
     {
          Part channeler = gameData.partsManager.channelerSelectedPart;
          Part structure = gameData.partsManager.structureSelectedPart;
          Part source = gameData.partsManager.sourceSelectedPart;

          if (channeler == null || structure == null || source == null)
          {
               GameData.DisplayInformation?.Invoke("A tower must have all 3 parts", 2);
               return;
          }
          this.channeler = channeler;
          this.structure = structure;
          this.source = source;
          //create the new tower
          this.tower = new GameObject("Tower", typeof(TowerController));
          //set the reference to the gameData
          tower.GetComponent<TowerController>().gameData = gameData;
          //set the tower in an unwatchable position (cause the GameObject constructor sets the object at origin)
          tower.transform.position = Vector3.one * 1000;
          gameData.isTowerReady = true;
          gameData.gameState.TrySetState(GameStateType.PlacingTower);
          GameData.DisplayInformation?.Invoke("You can press R to rotate the tower", 2);
     }
     /// <summary>
     /// Returns the prefab associated to the channeler of the tower
     /// </summary>
     /// <returns>The prefab for the channeler</returns>
     public GameObject GetChannelerPrefab(Part channelerPart)
     {
          if(channelerPart.type != PartType.Channeler) return null;
          return enumToGameObject[channelerPart.specificTypeInfo];
     }
     /// <summary>
     /// Returns the prefab associated to the structure of the tower
     /// </summary>
     /// <returns>The prefab of the structure</returns>
     public GameObject GetStructurePrefab(Part structurePart)
     {
          if(structurePart.type != PartType.Structure) return null;
          return enumToGameObject[structurePart.specificTypeInfo];
     }
     /// <summary>
     /// Returns the prefab associated to the source of the tower
     /// </summary>
     /// <returns>The prefab of the source</returns>
     public GameObject GetSourcePrefab(Part sourcePart)
     {
          if(sourcePart.type != PartType.Source) return null;
          return enumToGameObject[sourcePart.specificTypeInfo];
     }
}
