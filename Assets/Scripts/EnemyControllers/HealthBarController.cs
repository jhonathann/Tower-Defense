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
          this.transform.position += Vector3.up * EnemyStats.GetHeight(enemy.type);
     }
     void Update()
     {
          float health = enemy.health;
          healthBar.fillAmount = health / EnemyStats.GetHealth(enemy.type);
          RotateWithCameraInYAxis();
     }

     /// <summary>
     /// Rotates the gameBar in the Y axis to always look at the camera
     /// </summary>
     private void RotateWithCameraInYAxis()
     {
          transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
          ;
          return;
     }
}
