using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles the rotation of the tower
/// </summary>
public class TowerRotationHandler : MonoBehaviour
{
     public Action RotateTower;
     private void OnEnable()
     {
          RotateTower += OnRotateTower;
     }
     private void OnRotateTower()
     {
          if (Input.GetKeyDown(KeyCode.R))
          {
               this.gameObject.transform.Rotate(new Vector3(0, 90, 0));
          }
     }
}
