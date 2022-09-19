using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
     private float speedInX;
     private float speedInY;
     private float speedInZ;

     void Start()
     {

     }
     private void Update()
     {
          moveCamera();
     }
     private void moveCamera()
     {
          speedInX = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
          speedInY = Input.GetAxis("Jump") * Time.deltaTime * 5;
          speedInZ = Input.GetAxis("Vertical") * Time.deltaTime * 5;

          this.gameObject.transform.Translate(new Vector3(speedInX, speedInY, speedInZ));
     }
}
