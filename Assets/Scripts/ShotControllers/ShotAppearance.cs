using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the appearance of a shot based on its source type.
/// </summary>
public class ShotAppearance : MonoBehaviour
{
     /// <summary>
     /// The type of the shot
     /// </summary>
     public SourceType type;
     /// <summary>
     /// Reference to the particle system component
     /// </summary>
     private ParticleSystem particle;
     /// <summary>
     /// Reference to the trail renderer component
     /// </summary>
     private TrailRenderer trail;

     /// <summary>
     /// Initializes the necessary references and sets the color of the shot.
     /// </summary>
     private void Start()
     {
          // Get references to the required components.
          particle = GetComponentInChildren<ParticleSystem>();
          trail = GetComponentInChildren<TrailRenderer>();

          // Set the color of the shot.
          SetColor();
     }

     /// <summary>
     /// Sets the color of the shot based on its source type.
     /// </summary>
     private void SetColor()
     {
          ParticleSystem.MainModule mainModule = particle.main; // Get the main module of the particle system.

          Color32 shotColor = new(); // The color of the shot.

          // Determine the color based on the source type.
          switch (type)
          {
               case SourceType.Earth:
                    shotColor = new Color32(7, 77, 0, 255);
                    break;
               case SourceType.Fire:
                    shotColor = new Color32(200, 41, 0, 255);
                    break;
               case SourceType.Thunder:
                    shotColor = new Color32(45, 3, 147, 255);
                    break;
               case SourceType.Water:
                    shotColor = new Color32(16, 97, 152, 255);
                    break;
          }

          // Set the start color of the particle system.
          mainModule.startColor = new ParticleSystem.MinMaxGradient(shotColor);

          // Set the start color of the trail renderer.
          trail.startColor = shotColor;
     }
}
