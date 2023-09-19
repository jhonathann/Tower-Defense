using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Controls the behavior of the camera.
/// </summary>
public class CameraController : MonoBehaviour
{
     private const float MOVEMENT_SPEED = 100.0f;
     private const float ROTATION_SPEED = 50.0f;
     private const float ZOOM_SENSITIVITY = 10.0f;
     private readonly Vector2 X_BOUNDARIES = new(-300, 300);
     private readonly Vector2 Z_BOUNDARIES = new(-300, 300);
     private readonly Vector2 ZOOM_BOUNDARIES = new(10, 130);
     private Camera ortografic_Camera;

     private void Start()
     {
          ortografic_Camera = this.gameObject.GetComponent<Camera>();
     }

     private void Update()
     {
          Move();
          Zoom();
          Rotate();
     }

     /// <summary>
     /// Handles camera movement.
     /// </summary>
     private void Move()
     {
          GetInputs(out float movementInX, out float movementInZ);
          MakeMovement(movementInX, movementInZ);
          CorrectMovementWithBoundaries();

          static void GetInputs(out float movementInX, out float movementInZ)
          {
               movementInX = Input.GetAxis("Horizontal") * Time.deltaTime * MOVEMENT_SPEED;
               movementInZ = Input.GetAxis("Vertical") * Time.deltaTime * MOVEMENT_SPEED;
          }

          void MakeMovement(float movementInX, float movementInZ)
          {
               // Get the camera angle and calculate isometric movement vectors.
               float angle = -1 * this.transform.eulerAngles.y * Mathf.Deg2Rad;
               Vector3 isometricXDirection = new(Mathf.Cos(angle), 0, Mathf.Sin(angle));
               Vector3 isometricZDirection = new(-1 * Mathf.Sin(angle), 0, Mathf.Cos(angle));

               // Translate the camera based on movement inputs.
               this.gameObject.transform.Translate(movementInX * isometricXDirection, Space.World);
               this.gameObject.transform.Translate(movementInZ * isometricZDirection, Space.World);
          }

          void CorrectMovementWithBoundaries()
          {
               this.transform.position = CheckForPositionBoundaries(this.transform.position);

               Vector3 CheckForPositionBoundaries(Vector3 position)
               {
                    if (position.x < X_BOUNDARIES.x) position.x = X_BOUNDARIES.x;
                    if (position.x > X_BOUNDARIES.y) position.x = X_BOUNDARIES.y;
                    if (position.z < Z_BOUNDARIES.x) position.z = Z_BOUNDARIES.x;
                    if (position.z > Z_BOUNDARIES.y) position.z = Z_BOUNDARIES.y;
                    return position;
               }
          }
     }

     /// <summary>
     /// Handles camera zooming.
     /// </summary>
     private void Zoom()
     {
          float zoom = GetZoomAmount();
          ApplyZoom(zoom);
          CorrectForBoundaries();

          void ApplyZoom(float zoom)
          {
               ortografic_Camera.orthographicSize += zoom;
          }

          static float GetZoomAmount()
          {
               return Input.GetAxis("Mouse ScrollWheel") * ZOOM_SENSITIVITY * -1; // The *-1 is because the zoom in is a reduction in orthographic size.
          }

          void CorrectForBoundaries()
          {
               if (ortografic_Camera.orthographicSize < ZOOM_BOUNDARIES.x)
                    ortografic_Camera.orthographicSize = ZOOM_BOUNDARIES.x;
               if (ortografic_Camera.orthographicSize > ZOOM_BOUNDARIES.y)
                    ortografic_Camera.orthographicSize = ZOOM_BOUNDARIES.y;
          }
     }

     /// <summary>
     /// Handles camera rotation.
     /// </summary>
     private void Rotate()
     {
          float rotation = GetInput();
          MakeRotation(rotation);

          static float GetInput()
          {
               return Input.GetAxis("Rotation") * Time.deltaTime * ROTATION_SPEED;
          }

          void MakeRotation(float rotation)
          {
               this.gameObject.transform.Rotate(new Vector3(0, rotation, 0), Space.World);//Only rotates in y axis for correct isometric control
          }
     }
}
