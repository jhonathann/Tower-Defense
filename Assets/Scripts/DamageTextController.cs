using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script that handles the behaviour of the damage text
/// </summary>
public class DamageTextController : MonoBehaviour
{
     private TextMeshPro textMesh;
     private void Awake()
     {
          textMesh = this.GetComponent<TextMeshPro>();
          Destroy(this.gameObject, 0.5f);
     }

     public void SetUp(float damageAmount)
     {
          textMesh.SetText(damageAmount.ToString());
     }
}
