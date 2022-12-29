using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour
{
     public GameObject towerPrefab;
     public GameObject towerMockPrefab;
     GameObject towerMock;

     void OnMouseEnter()
     {
          if (isMouseOverAnUiElement()) return;
          towerMock = Instantiate(towerMockPrefab, this.transform.position + Vector3.up, towerPrefab.transform.rotation);
     }
     void OnMouseExit()
     {
          Destroy(towerMock);
     }
     void OnMouseUpAsButton()
     {
          if (isMouseOverAnUiElement()) return;
          Instantiate(towerPrefab, this.transform.position + Vector3.up, towerPrefab.transform.rotation);
     }
     bool isMouseOverAnUiElement()
     {
          return EventSystem.current.IsPointerOverGameObject();
     }
}
