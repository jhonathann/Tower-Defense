using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class Tower : ScriptableObject
{
     private Part channalizer;
     private Part structure;
     private Part source;
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
     public static Action<Tower> OnCreateTower;
     public Tower(Part channalizer, Part structure, Part source)
     {
          this.channalizer = channalizer;
          this.structure = structure;
          this.source = source;
     }
     private void OnEnable()
     {
          OnCreateTower += CreateTower;
     }
     private void CreateTower(Tower tower)
     {
          Debug.Log($"{tower.channalizer.specificTypeInfo} {tower.structure.specificTypeInfo} {tower.source.specificTypeInfo}");
     }
}
