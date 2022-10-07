using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
     public Material Mat1;
     public Material Mat2;
     public GameObject towerPrefab;
     GameObject tower;
     void Start()
     {

     }
     void Update()
     {

     }

     void OnMouseEnter()
     {
          tower = Instantiate(towerPrefab, this.transform.position, towerPrefab.transform.rotation);
          tower.gameObject.GetComponentInChildren<Renderer>().material = Mat2;
     }
     void OnMouseExit()
     {
          Destroy(tower);
          tower.gameObject.GetComponentInChildren<Renderer>().material = Mat1;
     }
     void OnMouseUpAsButton()
     {
          Instantiate(towerPrefab, this.transform.position, towerPrefab.transform.rotation);
     }
}
