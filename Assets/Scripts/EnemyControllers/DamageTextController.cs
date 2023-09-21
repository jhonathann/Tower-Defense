using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script that handles the behaviour of the damage text
/// </summary>
public class DamageTextController : MonoBehaviour
{
     private TextMeshPro textMesh;
     private void Awake()
     {
          textMesh = this.GetComponent<TextMeshPro>();
          Destroy(this.gameObject, 0.5f);
     }
     private void Update()
     {
          RotateWithCameraInYAxis();
     }
     public void SetUp(float damageAmount)
     {
          textMesh.SetText(damageAmount.ToString());
     }
     /// <summary>
     /// Rotates the text in the Y axis to always look at the camera
     /// </summary>
     private void RotateWithCameraInYAxis()
     {
          transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
          ;
          return;
     }
}
