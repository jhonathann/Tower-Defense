using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for the animation of the sources
/// </summary>
public class SourceAnimation : MonoBehaviour
{
     private const float ROTATION_SPEED = 45.0f;
     private const float TRANSLATION_SPEED = 0.5f;
     private const float MOVEMENT_RANGE_Y = 2.0f;
     private float movementDirectionY = 1.0f;
     private float movementDirectionX;
     private float movementDirectionZ;

     void Start()
     {
          //Start with random directions
          movementDirectionX = Random.Range(-1.0f, 1.0f);
          movementDirectionZ = Random.Range(-1.0f, 1.0f);
     }
     void Update()
     {
          Rotate();
          Move();
     }

     /// <summary>
     /// Handels the movement of the object
     /// </summary>
     private void Move()
     {
          /// <summary>
          /// Changes the movement directions when a limit is reached
          /// </summary>
          if (HasReachedUpperBoundary())
          {
               movementDirectionY = -1.0f;
               movementDirectionX = -movementDirectionX;
               movementDirectionZ = -movementDirectionZ;

          }
          if (HasReachedLowerBoundary())
          {
               movementDirectionY = 1.0f;
               movementDirectionX = Random.Range(-1.0f, 1.0f); ;
               movementDirectionZ = Random.Range(-1.0f, 1.0f); ;
          }
          // Moves the object whith the correspondent direction
          this.gameObject.transform.Translate(new Vector3(movementDirectionX, movementDirectionY, movementDirectionZ) * Time.deltaTime * TRANSLATION_SPEED);

          bool HasReachedUpperBoundary()
          {
               return this.gameObject.transform.position.y - TowerStats.SOURCE_HEIGHT > MOVEMENT_RANGE_Y;
          }

          bool HasReachedLowerBoundary()
          {
               return this.gameObject.transform.position.y - TowerStats.SOURCE_HEIGHT < -MOVEMENT_RANGE_Y;
          }
     }

     /// <summary>
     /// Rotates the object at a constant speed
     /// </summary>
     private void Rotate()
     {
          this.gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * ROTATION_SPEED, 0));
     }
}
