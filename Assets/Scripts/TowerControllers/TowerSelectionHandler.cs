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
     private GameObject worldUI;
     private const float WORLD_UI_HEIGHT = 25.0f;
     private void OnEnable()
     {
          SelectionManagerController.towerSelected += OnTowerSelected;
     }

     public void Setup(Collider selectionCollider, List<GameObject> hitZoneGameObjects, GameObject worldUIPrefab)
     {
          this.selectionCollider = selectionCollider;
          this.hitZoneGameObjects = hitZoneGameObjects;
          this.worldUI = Instantiate(worldUIPrefab, this.transform.position + Vector3.up * WORLD_UI_HEIGHT, Quaternion.identity, this.transform);
     }
     private void OnTowerSelected(RaycastHit raycastHit)
     {
          if (raycastHit.collider == this.selectionCollider)
          {
               ToggleSelectionOfTower();
          }
     }
     private void ToggleSelectionOfTower()
     {
          //Toggles the meshes of the hitZones
          foreach (GameObject hitzone in hitZoneGameObjects)
          {
               hitzone.SetActive(!hitzone.activeSelf);
          }
          //Toggles the tower's world UI
          worldUI.SetActive(!worldUI.activeSelf);
     }
}
