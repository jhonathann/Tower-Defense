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
     /// Reference to the scriptable object that handles the state od the game
     /// </summary>
     public GameState gameState;
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
     /// Function called when a new part is added (returns the part that was created and added)
     /// </summary>
     public static Func<Part, Part> AddNewPart;
     /// <summary>
     /// Event triggered each time a new part (or parts) is added
     /// </summary>
     public static Action<List<Part>> NewPartsAdded;
     /// <summary>
     /// Tower to be put on the map
     /// </summary>
     public TowerData towerData;
     public bool isTowerReady;
     /// <summary>
     /// Text to be shown in the HUD regarding the countdown for the next wave
     /// </summary>
     public string timerString;
     void OnEnable()
     {
          //Subscribe to the events
          GameStarted += OnGameStarted;
          SelectPart += OnSelectPart;
          AddNewPart += OnAddNewPart;
          TileController.TowerPlaced += OnTowerPlaced;
          PortalController.NextWave += OnNextWave;
     }
     /// <summary>
     /// Used to reset the variables when the game is started/restarted
     /// </summary>
     private void OnGameStarted()
     {
          gameState.TrySetState(GameStateType.Running);
          health = 10;
          waveCount = 0;
          parts?.Clear();
          isTowerReady = false;
          AddStartingParts(3);
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
     /// Adds a new part to the list and re-renders the HUD panel to reflect the change (returns the added part)
     /// </summary>
     Part OnAddNewPart(Part part = null)
     {
          Part partToBeAdded = part;
          if (partToBeAdded != null)
          {
               parts.Add(partToBeAdded);
          }
          else
          {
               partToBeAdded = new Part();
               parts.Add(partToBeAdded);
          }
          HUDController.RenderPanel?.Invoke();
          return partToBeAdded;
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
          //Set the available tower again to false
          this.isTowerReady = false;
          gameState.TrySetState(GameStateType.Running);
     }
     /// <summary>
     /// Triggers when a next wave is called
     /// </summary>
     void OnNextWave()
     {
          waveCount++;
          List<Part> addedParts = new List<Part>();
          // Gives between 1 and 3 new parts each time a wave starts
          int randomNumberOfParts = UnityEngine.Random.Range(1, 4);
          for (int i = 0; i < randomNumberOfParts; i++)
          {
               //Triggers the AddNewPart event and adds the created part to the addedPartsList
               addedParts.Add(GameData.AddNewPart?.Invoke(null));
          }
          //Trigger the new parts added event
          NewPartsAdded?.Invoke(addedParts);
     }
}