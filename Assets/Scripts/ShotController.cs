using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controlles the behaviour of the shots
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
     /// The effect the show will aplly
     /// </summary>
     public Func<EnemyController, IEnumerator> Effect;
     /// <summary>
     /// The height at wich the show will start
     /// </summary>
     private const float SHOT_SPAWN_HEIGHT = 20.0f;
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
               this.transform.Translate(Vector3.forward * Time.deltaTime * 100);
          }
          else
          {
               Destroy(this.gameObject);
          }
     }
     void OnTriggerEnter(Collider enteringObjectCollider)
     {
          IDamagable damageable = enteringObjectCollider.GetComponent<IDamagable>();
          if (damageable != null)
          {
               damageable.TakeDamage(this.damage, this.Effect);
               Destroy(this.gameObject);
          }
     }
}
