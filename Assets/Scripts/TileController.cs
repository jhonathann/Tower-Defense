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
     /// Flag to check if the target ocupied with a tower already
     /// </summary>
     private bool isOcupied = false;
     private void OnMouseEnter()
     {
          if (isMouseOverAnUiElement()) return;
          if (!isThereATowerToPlace()) return;
          if (isOcupied) return;
          gameData.towerData.tower.transform.position = this.transform.position;
     }
     private void OnMouseExit()
     {
          if (isMouseOverAnUiElement()) return;
          if (!isThereATowerToPlace()) return;
          //Sets the tower in an unwatchable position
          gameData.towerData.tower.transform.position = Vector3.one * 1000;
     }
     private void OnMouseUpAsButton()
     {
          if (isMouseOverAnUiElement()) return;
          if (!isThereATowerToPlace()) return;
          if (isOcupied) return;
          //mark the tile as ocupied
          isOcupied = true;
          //trigers the towerPlaced event
          TowerPlaced?.Invoke();
     }
     /// <summary>
     /// Helper function to know when the mouse is over the UI
     /// </summary>
     /// <returns>True if the mouse is over an UI element and false otherwise</returns>
     private bool isMouseOverAnUiElement()
     {
          return EventSystem.current.IsPointerOverGameObject();
     }
     /// <summary>
     /// Helper function to know if there is an available tower to be placed
     /// </summary>
     /// <returns>True if there is an available tower and false otherwise</returns>
     private bool isThereATowerToPlace()
     {
          return gameData.isTowerReady;
     }
}
