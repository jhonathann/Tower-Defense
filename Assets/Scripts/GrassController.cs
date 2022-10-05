using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
     public Material Mat1;
     public Material Mat2;
     void Start()
     {

     }
     void Update()
     {

     }

     void OnMouseEnter()
     {
          this.gameObject.GetComponentInChildren<Renderer>().material = Mat2;
     }
     void OnMouseExit()
     {
          this.gameObject.GetComponentInChildren<Renderer>().material = Mat1;
     }
}
