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
          RotateTowardsCameraInYAxis();
     }
     public void SetUp(float damageAmount)
     {
          textMesh.SetText(damageAmount.ToString());
     }
     /// <summary>
     /// Rotates the text in the Y axis to always look at the camera
     /// </summary>
     private void RotateTowardsCameraInYAxis()
     {
          transform.LookAt(Camera.main.transform);
          Vector3 rotationJustInY = transform.rotation.eulerAngles;
          rotationJustInY.x = 0;
          rotationJustInY.z = 0;
          ///Adds 180 degress to the rotation because of the layout of textMeshPro
          rotationJustInY.y = transform.rotation.eulerAngles.y + 180;
          transform.rotation = Quaternion.Euler(rotationJustInY);
     }
}
