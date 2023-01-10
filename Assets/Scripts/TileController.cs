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
     /// Reference to the mock tower 
     /// </summary>
     private GameObject towerMock;
     /// <summary>
     /// Action triggered when the tower is succesfully placed
     /// </summary>
     public static Action TowerPlaced;
     private void OnMouseEnter()
     {
          if (isMouseOverAnUiElement()) return;
          if (!isThereATowerToPlace()) return;
          InstantiateTowerMock();
     }
     private void OnMouseExit()
     {
          if (!isThereATowerToPlace()) return;
          DestroyMock();
     }
     private void OnMouseUpAsButton()
     {
          if (isMouseOverAnUiElement()) return;
          if (!isThereATowerToPlace()) return;
          InstantiateTower();
          DestroyMock(); //Destroy the previous mock
          //Trigger the towerplaced event
          TowerPlaced?.Invoke();
     }
     /// <summary>
     /// Function that instantiate the correct prefabs
     /// </summary>
     private void InstantiateTower()
     {
          //create the new tower
          GameObject tower = new GameObject("Tower", typeof(TowerController));
          //set the reference to the gameData
          tower.GetComponent<TowerController>().gameData = gameData;
          //set the right position (cause the GameObject constructor sets the object at origin)
          tower.transform.position = this.transform.position;
     }
     /// <summary>
     /// Funtion that instantiates the prefabs and holds a refernce to them so they can be deleated
     /// </summary>
     private void InstantiateTowerMock()
     {
          towerMock = new GameObject("TowerMock", typeof(TowerController));
          towerMock.GetComponent<TowerController>().gameData = gameData;
          towerMock.transform.position = this.transform.position;
     }
     /// <summary>
     /// Deletes the prefabs
     /// </summary>
     private void DestroyMock()
     {
          Destroy(towerMock);
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
