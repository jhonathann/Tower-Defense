using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoltController : MonoBehaviour
{
     public GameObject target;
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
          if (enteringObjectCollider.gameObject.name == "Enemy")
          {
               Destroy(this.gameObject);
          }
     }
}
