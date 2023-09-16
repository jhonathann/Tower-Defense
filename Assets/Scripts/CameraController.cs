using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behaviour of the camera
/// </summary>
public class CameraController : MonoBehaviour
{
     private const float MOVEMENT_SPEED = 50.0f;
     private const float ROTATION_SPEED = 50.0f;
     private readonly Vector2 X_BOUNDARIES = new Vector2(50, 150);
     private readonly Vector2 Y_BOUNDARIES = new Vector2(50, 150);
     private readonly Vector2 Z_BOUNDARIES = new Vector2(-150, 50);
     private readonly Vector2 ROTATION_BOUNDARIES = new Vector2(-45, 45);
     private void Update()
     {
          Move();
          Rotate();
     }
     private void Move()
     {

          GetInputs(out float movementInX, out float movementInY, out float movementInZ);
          MakeMovement(movementInX, movementInY, movementInZ);
          CorrectMovementWithBoundaries();

          static void GetInputs(out float movementInX, out float movementInY, out float movementInZ)
          {
               movementInX = Input.GetAxis("Horizontal") * Time.deltaTime * MOVEMENT_SPEED;
               movementInY = Input.GetAxis("Jump") * Time.deltaTime * MOVEMENT_SPEED;
               movementInZ = Input.GetAxis("Vertical") * Time.deltaTime * MOVEMENT_SPEED;
          }

          void MakeMovement(float movementInX, float movementInY, float movementInZ)
          {
               this.gameObject.transform.Translate(new Vector3(movementInX, movementInY, movementInZ), Space.World);
          }

          void CorrectMovementWithBoundaries()
          {
               this.transform.position = CheckForPositionBoundaries(this.transform.position);
          }
     }
     private void Rotate()
     {
          float rotation = GetInput();
          MakeRotation(rotation);
          CorrectRotationWithBoundaries();

          static float GetInput()
          {
               return Input.GetAxis("Rotation") * Time.deltaTime * ROTATION_SPEED;
          }

          void MakeRotation(float rotation)
          {
               this.gameObject.transform.Rotate(new Vector3(0, rotation, 0), Space.World);
          }

          void CorrectRotationWithBoundaries()
          {
               this.transform.rotation = CheckForRotationBoundaries(this.transform.rotation);
          }
     }

     private Vector3 CheckForPositionBoundaries(Vector3 position)
     {
          if (position.x < X_BOUNDARIES.x) position.x = X_BOUNDARIES.x;
          if (position.x > X_BOUNDARIES.y) position.x = X_BOUNDARIES.y;
          if (position.y < Y_BOUNDARIES.x) position.y = Y_BOUNDARIES.x;
          if (position.y > Y_BOUNDARIES.y) position.y = Y_BOUNDARIES.y;
          if (position.z < Z_BOUNDARIES.x) position.z = Z_BOUNDARIES.x;
          if (position.z > Z_BOUNDARIES.y) position.z = Z_BOUNDARIES.y;
          return position;
     }

     private Quaternion CheckForRotationBoundaries(Quaternion rotation)
     {
          Vector3 eulerRotation = rotation.eulerAngles;
          eulerRotation = ChangeAnglesRange(eulerRotation);
          if (eulerRotation.y < ROTATION_BOUNDARIES.x) eulerRotation.y = ROTATION_BOUNDARIES.x;
          if (eulerRotation.y > ROTATION_BOUNDARIES.y) eulerRotation.y = ROTATION_BOUNDARIES.y;
          return Quaternion.Euler(eulerRotation);


          /// <summary>
          /// Changes the range of the eulerAngles from [0,360] to [-180,180]
          /// </summary>
          /// <param name="eulerRotation">eulerRotation to be changed</param>
          /// <returns>The eulerAngles within the new range</returns>
          static Vector3 ChangeAnglesRange(Vector3 eulerRotation)
          {
               for (int i = 0; i < 3; i++)
               {
                    eulerRotation[i] = Mathf.Repeat(eulerRotation[i] + 180.0f, 360.0f) - 180.0f;
               }
               return eulerRotation;
          }
     }
}
