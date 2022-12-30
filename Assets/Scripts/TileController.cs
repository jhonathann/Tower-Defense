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
     public GameData gameData;
     //Constant for the correct instantiation of the prefabs
     private const float CHANNALIZER_HEIGHT = 18.0f;
     private const float SOURCE_HEIGHT = 8.0f;
     private const float STRUCTURE_HEIGHT = 1.0f;
     //References to the mock prefabs 
     private GameObject channalizerMock;
     private GameObject structureMock;
     private GameObject sourceMock;
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
          Instantiate(gameData.availableTower.GetChannalizerPrefab(), this.transform.position + Vector3.up * CHANNALIZER_HEIGHT, Quaternion.identity);
          Instantiate(gameData.availableTower.GetStructurePrefab(), this.transform.position + Vector3.up * STRUCTURE_HEIGHT, Quaternion.identity);
          Instantiate(gameData.availableTower.GetSourcePrefab(), this.transform.position + Vector3.up * SOURCE_HEIGHT, Quaternion.identity);
     }
     /// <summary>
     /// Funtion that instantiates the prefabs and holds a refernce to them so they can be deleated
     /// </summary>
     private void InstantiateTowerMock()
     {
          channalizerMock = Instantiate(gameData.availableTower.GetChannalizerPrefab(), this.transform.position + Vector3.up * CHANNALIZER_HEIGHT, Quaternion.identity);
          structureMock = Instantiate(gameData.availableTower.GetStructurePrefab(), this.transform.position + Vector3.up * STRUCTURE_HEIGHT, Quaternion.identity);
          sourceMock = Instantiate(gameData.availableTower.GetSourcePrefab(), this.transform.position + Vector3.up * SOURCE_HEIGHT, Quaternion.identity);
     }
     /// <summary>
     /// Deletes the prefabs
     /// </summary>
     private void DestroyMock()
     {
          Destroy(channalizerMock);
          Destroy(structureMock);
          Destroy(sourceMock);
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
          return gameData.availableTower != null;
     }
}
