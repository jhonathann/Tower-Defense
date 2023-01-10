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
     public Part channalizer;
     public Part structure;
     public Part source;
     [SerializeField]
     private GameObject areaChannalizerPrefab;
     [SerializeField]
     private GameObject fastChannalizerPrefab;
     [SerializeField]
     private GameObject strongChannalizerPrefab;
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
     public static Action<Part, Part, Part> OnCreateTower;
     private Dictionary<Enum, GameObject> enumtoGameObject;
     private void OnEnable()
     {
          OnCreateTower += CreateTower;
          //Load the prefabs to the dictionary
          enumtoGameObject = new Dictionary<Enum, GameObject>
          {{ChannalizerType.Area,areaChannalizerPrefab},
          {ChannalizerType.Fast,fastChannalizerPrefab},
          {ChannalizerType.Strong,strongChannalizerPrefab},
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
     /// <param name="channalizer">The channalizer part</param>
     /// <param name="structure">The structure part</param>
     /// <param name="source">The source part</param>
     private void CreateTower(Part channalizer, Part structure, Part source)
     {
          if (channalizer == null || structure == null || source == null) return;
          this.channalizer = channalizer;
          this.structure = structure;
          this.source = source;
          gameData.isTowerReady = true;
     }
     /// <summary>
     /// Returns the prefab associated to the channalizer of the tower
     /// </summary>
     /// <returns>The prefab for the channalizer</returns>
     public GameObject GetChannalizerPrefab()
     {
          return enumtoGameObject[channalizer.specificTypeInfo];
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
