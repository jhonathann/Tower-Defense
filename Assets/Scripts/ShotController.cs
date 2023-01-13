using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShotController : MonoBehaviour
{
     public GameObject target;
     public float damage;
     public Func<EnemyController, IEnumerator> Effect;
     void Start()
     {
          this.gameObject.name = "Bolt";
          this.transform.position += Vector3.up * 23;
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
