using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// This class controlls the tiles where the tower can span
/// </summary>
public class TileController : MonoBehaviour
{
     /// <summary>
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
     /// <summary>
     /// Action triggered when the tower is succesfully placed
     /// </summary>
     public static Action TowerPlaced;
     /// <summary>
     /// Reference to the tower that is on the tile
     /// </summary>
     private TowerController tower;
     private void OnMouseEnter()
     {
          if (IsMouseOverAnUiElement()) return;
          if (!IsThereATowerToPlace()) return;
          if (IsOcupied()) return;
          gameData.towerData.tower.transform.position = this.transform.position;
     }
     private void OnMouseExit()
     {
          if (IsMouseOverAnUiElement()) return;
          if (!IsThereATowerToPlace()) return;
          //Sets the tower in an unwatchable position
          gameData.towerData.tower.transform.position = Vector3.one * 1000;
     }
     private void OnMouseUpAsButton()
     {
          if (IsMouseOverAnUiElement()) return;
          if (!IsThereATowerToPlace()) return;
          if (IsOcupied()) return;
          //set the reference to the tower
          this.tower = gameData.towerData.tower.GetComponent<TowerController>();
          //Subscribes to the tower relocated event
          tower.TowerBeingRelocated += OnTowerRelocated;
          //Sets tower state to placed
          tower.State = TowerState.Placed;
          //triggers the towerPlaced event
          TowerPlaced?.Invoke();
     }
     /// <summary>
     /// Function to be triggered when the tower of the tile is relocated
     /// </summary>
     private void OnTowerRelocated()
     {
          tower = null;
     }
     /// <summary>
     /// Helper function to know when the mouse is over the UI
     /// </summary>
     /// <returns>True if the mouse is over an UI element and false otherwise</returns>
     private bool IsMouseOverAnUiElement()
     {
          return EventSystem.current.IsPointerOverGameObject();
     }
     /// <summary>
     /// Helper function to know if there is an available tower to be placed
     /// </summary>
     /// <returns>True if there is an available tower and false otherwise</returns>
     private bool IsThereATowerToPlace()
     {
          return gameData.isTowerReady;
     }
     /// <summary>
     /// Helper function to know if the Tile already has a tower in it
     /// </summary>
     /// <returns>True if the there is already a tower in the tile</returns>
     private bool IsOcupied()
     {
          return tower is not null;
     }
}
