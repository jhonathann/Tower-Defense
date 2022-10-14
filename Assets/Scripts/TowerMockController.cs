using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMockController : MonoBehaviour
{
     Renderer[] renderers;
     public Shader hologram;

     void Start()
     {
          //Change the shader of all of the components of the tower
          renderers = this.GetComponentsInChildren<Renderer>();
          foreach (Renderer renderer in renderers)
          {
               renderer.material.shader = hologram;
          }
     }
}
