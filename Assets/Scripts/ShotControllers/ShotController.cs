using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controlls the behaviour of the shots
/// </summary>
public class ShotController : MonoBehaviour
{
     /// <summary>
     /// The target of the shot
     /// </summary>
     public GameObject target;
     /// <summary>
     /// The ammount of damage the shot will make
     /// </summary>
     public float damage;
     /// <summary>
     /// The element of the shot
     /// </summary>
     public Element element;
     /// <summary>
     /// The effect the shot will apply
     /// </summary>
     public Func<EnemyController, IEnumerator> Effect;
     /// <summary>
     /// The height at which the show will start
     /// </summary>
     private const float SHOT_SPAWN_HEIGHT = 20.0f;
     /// <summary>
     /// The Speed of the proyectile
     /// </summary>
     private const float SPEED = 75.0f;
     void Start()
     {
          this.gameObject.name = "Shot";
          this.transform.position += Vector3.up * SHOT_SPAWN_HEIGHT;
     }

     void Update()
     {
          moveTowards(target);
     }
     public void moveTowards(GameObject target)
     {
          if (target != null)
          {
               this.transform.LookAt(target.transform);
               this.transform.Translate(SPEED * Time.deltaTime * Vector3.forward);
          }
          else
          {
               //If the target is destroyed the shot is destroyed
               Destroy(this.gameObject);
          }
     }
     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          if (isNotAnEnemy()) return;

          IDamageable damageable = enteringObjectCollider.GetComponent<IDamageable>();

          //Checks if the object is the target (used for the case where the shot goes through several enemies)
          EnemyController enemy = damageable as EnemyController;
          if (enemy.gameObject == target)
          {
               damageable.TakeDamage(this.damage, this.Effect, this.element);
               Destroy(this.gameObject);
          }

          bool isNotAnEnemy()
          {
               return enteringObjectCollider.GetComponent<EnemyController>() is null;
          }
     }
}
