using System.Collections;
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
     public static Action<Part, Part, Part> OnCreateTower;
     private Dictionary<Enum, GameObject> enumtoGameObject;
     private void OnEnable()
     {
          OnCreateTower += CreateTower;
          //Load the prefabs to the dictionary
          enumtoGameObject = new Dictionary<Enum, GameObject>
          {{ChannelerType.Area,areaChannelerPrefab},
          {ChannelerType.Fast,fastChannelerPrefab},
          {ChannelerType.Strong,strongChannelerPrefab},
          {StructureType.Beam,beamStructurePrefab},
          {StructureType.Circular,circularStructurePrefab},
          {StructureType.Cross,crossStructurePrefab},
          {SourceType.Earth,earthSourcePrefab},
          {SourceType.Fire,fireSourcePrefab},
          {SourceType.Thunder,thunderSourcePrefab},
          {SourceType.Water,waterSourcePrefab}};
     }
     /// <summary>
     /// Function that sets this parts to the tower SO when there are 3 parts
     /// </summary>
     /// <param name="channeler">The channeler part</param>
     /// <param name="structure">The structure part</param>
     /// <param name="source">The source part</param>
     private void CreateTower(Part channeler, Part structure, Part source)
     {
          if (channeler == null || structure == null || source == null)
          {
               GameData.DisplayInformation?.Invoke("A tower must have all 3 parts", 2);
               return;
          }
          this.channeler = channeler;
          this.structure = structure;
          this.source = source;
          gameData.isTowerReady = true;
          gameData.gameState.TrySetState(GameStateType.PlacingTower);
          GameData.DisplayInformation?.Invoke("You can press R to rotate the tower", 2);
     }
     /// <summary>
     /// Returns the prefab associated to the channeler of the tower
     /// </summary>
     /// <returns>The prefab for the channeler</returns>
     public GameObject GetChannelerPrefab()
     {
          return enumtoGameObject[channeler.specificTypeInfo];
     }
     /// <summary>
     /// Returns the prefab associated to the structure of the tower
     /// </summary>
     /// <returns>The prefab of the structure</returns>
     public GameObject GetStructurePrefab()
     {
          return enumtoGameObject[structure.specificTypeInfo];
     }
     /// <summary>
     /// Returns the prefab associated to the source of the tower
     /// </summary>
     /// <returns>The prefab of the source</returns>
     public GameObject GetSourcePrefab()
     {
          return enumtoGameObject[source.specificTypeInfo];
     }
}
