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
          healthBar.fillAmount = health / 100;//|100 is the max current enemy health
     }
}
