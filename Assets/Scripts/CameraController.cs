using UnityEngine;

/// <summary>
/// Controls the behavior of the camera.
/// </summary>
public class CameraController : MonoBehaviour
{
     private const float MOVEMENT_SPEED = 100.0f;
     private const float ROTATION_SPEED = 50.0f;
     private const float ZOOM_SENSITIVITY = 10.0f;
     private readonly Vector2 xBoundaries = new(-300, 300);
     private readonly Vector2 zBoundaries = new(-300, 300);
     private readonly Vector2 zoomBoundaries = new(10, 130);
     private Camera ortograficCamera;

     private void Start()
     {
          ortograficCamera = this.gameObject.GetComponent<Camera>();
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
                    if (position.x < xBoundaries.x) position.x = xBoundaries.x;
                    if (position.x > xBoundaries.y) position.x = xBoundaries.y;
                    if (position.z < zBoundaries.x) position.z = zBoundaries.x;
                    if (position.z > zBoundaries.y) position.z = zBoundaries.y;
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
               ortograficCamera.orthographicSize += zoom;
          }

          static float GetZoomAmount()
          {
               return Input.GetAxis("Mouse ScrollWheel") * ZOOM_SENSITIVITY * -1; // The *-1 is because the zoom in is a reduction in orthographic size.
          }

          void CorrectForBoundaries()
          {
               if (ortograficCamera.orthographicSize < zoomBoundaries.x)
                    ortograficCamera.orthographicSize = zoomBoundaries.x;
               if (ortograficCamera.orthographicSize > zoomBoundaries.y)
                    ortograficCamera.orthographicSize = zoomBoundaries.y;
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
               this.gameObject.transform.Rotate(new Vector3(0, rotation, 0), Space.World);//Only rotates in y-axis for correct isometric control
          }
     }
}
