using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that handels the behaviour of the healthBar
/// </summary>
public class HealthBarController : MonoBehaviour
{
     private Image healthBar;
     private float baseHealth;
     private EnemyController enemy;
     private float heightOffset = 5.0f;
     void Awake()
     {
          healthBar = this.GetComponentInChildren<Image>();
          enemy = this.GetComponentInParent<EnemyController>();
     }
     void Start()
     {
          this.transform.position += Vector3.up * heightOffset;
     }
     void Update()
     {
          float health = enemy.health;
          healthBar.fillAmount = health / 100;//100 is the max current enemy health
          RotateTowardsCameraInYAxis();
     }

     /// <summary>
     /// Rotates the gameBar in the Y axis to always look at the camera
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
