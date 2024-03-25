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
     private EnemyController enemy;
     void Awake()
     {
          healthBar = this.GetComponentInChildren<Image>();
          enemy = this.GetComponentInParent<EnemyController>();
     }
     void Start()
     {
          //The height calculation is a line that crosses (1,3) y (10,30)
          this.transform.position += Vector3.up * (enemy.gameObject.transform.localScale.y * 3);

     }
     void Update()
     {
          healthBar.fillAmount = enemy.health / enemy.originalHealth;
          RotateWithCameraInYAxis();
     }

     /// <summary>
     /// Rotates the gameBar in the Y axis to always look at the camera
     /// </summary>
     private void RotateWithCameraInYAxis()
     {
          transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
     }
}
