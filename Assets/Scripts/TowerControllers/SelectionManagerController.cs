using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// Class that controls the raycast of the mouse when it is clicked in the world
/// </summary>
public class SelectionManagerController : MonoBehaviour
{
     /// <summary>
     /// Function used to know if the raycasthit resulted in a tower selection, expects a return true if so, false otherwise
     /// </summary>
     public static Func<RaycastHit, bool> towerSelected;

     private void Update()
     {
          if (IsMouseOverAnUiElement()) return;
          if (!Input.GetMouseButtonDown(0)) return;
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit[] rayCastHits = Physics.RaycastAll(ray, Mathf.Infinity, LayerMask.NameToLayer("IgnoreRaycast"));
          rayCastHits = rayCastHits.OrderBy(hit => hit.distance).ToArray();//Orders the raycastHits by distance (the closest to the mouse will be the one that is analized first)
          foreach (RaycastHit rayCastHit in rayCastHits)
          {
               //Checks if the collider belongs to a tower
               if (rayCastHit.collider.gameObject.GetComponent<TowerController>() is not null)
               {
                    //Loops throughout all the subscribers
                    foreach (Func<RaycastHit, bool> function in towerSelected.GetInvocationList().Cast<Func<RaycastHit, bool>>())
                    {
                         //If al least one of the subscribers returns true (aka toggles the tower) the code finish
                         if (function.Invoke(rayCastHit)) return;
                    }
               }
          }
     }
     /// <summary>
     /// Helper function to know when the mouse is over the UI
     /// </summary>
     /// <returns>True if the mouse is over an UI element and false otherwise</returns>
     private bool IsMouseOverAnUiElement()
     {
          return EventSystem.current.IsPointerOverGameObject();
     }
}
