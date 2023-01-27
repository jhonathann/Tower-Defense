using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to the tower that handles the events that ocurr when the tower is selected
/// </summary>
public class TowerSelectionHandler : MonoBehaviour
{
     private Collider selectionCollider;
     private List<GameObject> hitZoneGameObjects;
     private void OnEnable()
     {
          SelectionManagerController.towerSelected += OnTowerSelected;
     }

     public void Setup(Collider selectionCollider, List<GameObject> hitZoneGameObjects)
     {
          this.selectionCollider = selectionCollider;
          this.hitZoneGameObjects = hitZoneGameObjects;
     }
     private void OnTowerSelected(RaycastHit raycastHit)
     {
          if (raycastHit.collider == this.selectionCollider)
          {
               toggleSelectionOfTower();
          }
     }
     private void toggleSelectionOfTower()
     {
          foreach (GameObject hitzone in hitZoneGameObjects)
          {
               hitzone.SetActive(!hitzone.activeSelf);
          }
     }
}
