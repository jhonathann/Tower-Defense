using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// ScriptableObject that contains relevant game informatcion
/// </summary>
[CreateAssetMenu]
public class GameData : ScriptableObject
{
     /// <summary>
     /// Action that is trigger when the game has started or restarted
     /// </summary>
     public static Action GameStarted;
     /// <summary>
     /// Reference to the generated path
     /// </summary>
     public List<Tile> path;
     /// <summary>
     /// The health of the player
     /// </summary>
     public float health;
     /// <summary>
     /// Count the number of waves
     /// </summary>
     public int waveCount;
     /// <summary>
     /// Counts the duration of the current game
     /// </summary>
     public float gameTime;
     /// <summary>
     /// The list of parts that the player currently has
     /// </summary>
     /// <typeparam name="Part">The Part Class</typeparam>
     /// <returns></returns>
     public List<Part> parts = new List<Part>();
     //Slots for the three selected parts
     public Part channalizerSelectedPart;
     public Part structureSelectedPart;
     public Part sourceSelectedPart;
     /// <summary>
     /// Action called when a part is clicked
     /// </summary>
     public static Action<Part> SelectPart;
     /// <summary>
     /// Action called when a new part is added
     /// </summary>
     public static Action<Part> AddNewPart;
     /// <summary>
     /// Tower to be put on the map
     /// </summary>
     public TowerData towerData;
     public bool isTowerReady;
     void OnEnable()
     {
          //Subscribe to the events
          GameStarted += OnGameStarted;
          SelectPart += OnSelectPart;
          AddNewPart += OnAddNewPart;
          TileController.TowerPlaced += OnTowerPlaced;
          PortalController.NextWave += OnNextWave;
          CastleController.CastleDestroyed += OnCastleDestroyed;
     }

     /// <summary>
     /// Used to reset the variables when the game is started/restarted
     /// </summary>
     private void OnGameStarted()
     {
          health = 10;
          waveCount = 0;
          gameTime = Time.time;
          parts?.Clear();
          isTowerReady = false;
          AddStartingParts(3);
          Time.timeScale = 1; //Unpauses the game (in case of a restart) (idk why but this isnt noticeable in the editor but affects the build)
     }
     /// <summary>
     /// Adds the starting parts to the gameobject
     /// </summary>
     /// <param name="numberOfExtraParts">The number of starting parts</param>
     void AddStartingParts(int numberOfExtraParts)
     {
          //Add a part of each type to ensure that the user is able to place the starting tower
          GameData.AddNewPart(new Part(PartType.Channalizer));
          GameData.AddNewPart(new Part(PartType.Structure));
          GameData.AddNewPart(new Part(PartType.Source));
          //Add some extra parts
          for (int i = 0; i < numberOfExtraParts; i++)
          {
               GameData.AddNewPart.Invoke(null);
          }
     }
     /// <summary>
     /// Adds a new part to the list and re-renders the HUD panel to reflect the change
     /// </summary>
     void OnAddNewPart(Part part = null)
     {
          if (part != null)
          {
               parts.Add(part);
          }
          else
          {
               parts.Add(new Part());
          }
          HUDController.RenderPanel?.Invoke();
     }
     /// <summary>
     /// Sets the new selected part to the corresponding field and changes the uss classes of newSelect and the previously selected part
     /// </summary>
     /// <param name="newSelectedPart">Part that was selected</param>
     void OnSelectPart(Part newSelectedPart)
     {
          switch (newSelectedPart.type)
          {
               case PartType.Channalizer:
                    channalizerSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    channalizerSelectedPart = newSelectedPart;
                    break;
               case PartType.Structure:
                    structureSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    structureSelectedPart = newSelectedPart;
                    break;
               case PartType.Source:
                    sourceSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    sourceSelectedPart = newSelectedPart;
                    break;
          }
     }
     /// <summary>
     /// This triggers when a tower is successfully placed
     /// </summary>
     void OnTowerPlaced()
     {
          //Remove the parts from the part List
          this.parts.Remove(channalizerSelectedPart);
          this.parts.Remove(structureSelectedPart);
          this.parts.Remove(sourceSelectedPart);
          //Set the selected parts again to null;
          this.channalizerSelectedPart = null;
          this.structureSelectedPart = null;
          this.sourceSelectedPart = null;
          //Set the available tower again to null
          this.isTowerReady = false;
     }
     /// <summary>
     /// Triggers when the castle is destroyed
     /// </summary>
     void OnCastleDestroyed()
     {
          gameTime = Time.time - gameTime;
     }
     /// <summary>
     /// Triggers when a next wave is called
     /// </summary>
     void OnNextWave()
     {
          waveCount++;
     }
}
