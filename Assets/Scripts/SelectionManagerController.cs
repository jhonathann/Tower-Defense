using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controls the raycast of the mouse when it is clicked in the world
/// </summary>
public class SelectionManagerController : MonoBehaviour
{
     public static Action<RaycastHit> towerSelected;

     private void Update()
     {
          if (!Input.GetMouseButtonDown(0)) return;
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit[] rayCastHits = Physics.RaycastAll(ray, Mathf.Infinity, LayerMask.NameToLayer("IgnoreRaycast"));
          foreach (RaycastHit rayCastHit in rayCastHits)
          {
               if (rayCastHit.collider.gameObject.GetComponent<TowerController>() is not null)
               {
                    towerSelected?.Invoke(rayCastHit);
               }
          }
     }
}
