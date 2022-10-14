using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
     public GameObject towerPrefab;
     public GameObject towerMockPrefab;
     GameObject towerMock;
     void Start()
     {

     }
     void Update()
     {

     }

     void OnMouseEnter()
     {
          towerMock = Instantiate(towerMockPrefab, this.transform.position + Vector3.up, towerPrefab.transform.rotation);
     }
     void OnMouseExit()
     {
          Destroy(towerMock);
     }
     void OnMouseUpAsButton()
     {
          Instantiate(towerPrefab, this.transform.position + Vector3.up, towerPrefab.transform.rotation);
     }
}
