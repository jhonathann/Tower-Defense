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
     /// Reference to the scriptable object that manage the parts
     /// </summary>
     public PartsManager partsManager;
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
     //Slots for the three selected parts
     public Part channelerSelectedPart;
     public Part structureSelectedPart;
     public Part sourceSelectedPart;
     /// <summary>
     /// Action called when a part is clicked
     /// </summary>
     public static Action<Part> SelectPart;
     /// <summary>
     /// Tower to be put on the map
     /// </summary>
     public TowerData towerData;
     public bool isTowerReady;
     /// <summary>
     /// Scriptable Object of the sprites for the towerWorldUI
     /// </summary>
     public TowerUIData towerUiData;
     /// <summary>
     /// Text to be shown in the HUD regarding the countdown for the next wave
     /// </summary>
     public string timerString;
     /// <summary>
     /// UiDocument used to pass information in certain points in the game
     /// </summary>
     public GameObject informationScreenPrefab;
     /// <summary>
     /// Action that triggers the informationMessage
     /// </summary>
     public static Action<string, float> DisplayInformation;
     void OnEnable()
     {
          //Subscribe to the events
          GameStarted += OnGameStarted;
          SelectPart += OnSelectPart;
          DisplayInformation += OnDisplayInformation;
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
          channelerSelectedPart = null;
          structureSelectedPart = null;
          sourceSelectedPart = null;
          isTowerReady = false;
     }
     /// <summary>
     /// Sets the new selected part to the corresponding field and changes the uss classes of newSelect and the previously selected part
     /// </summary>
     /// <param name="newSelectedPart">Part that was selected</param>
     void OnSelectPart(Part newSelectedPart)
     {
          switch (newSelectedPart.type)
          {
               case PartType.Channeler:
                    channelerSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    channelerSelectedPart = newSelectedPart;
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
          partsManager.parts.Remove(channelerSelectedPart);
          partsManager.parts.Remove(structureSelectedPart);
          partsManager.parts.Remove(sourceSelectedPart);
          //Set the selected parts again to null;
          this.channelerSelectedPart = null;
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
     }
     /// <summary>
     /// Used when a new information message needs to be displayed
     /// </summary>
     /// <param name="text">The message to be displayed</param>
     /// <param name="destroyTime">the time of the message</param>
     public void OnDisplayInformation(string text, float destroyTime)
     {
          InformationScreenController informationScreen = Instantiate(informationScreenPrefab).GetComponent<InformationScreenController>();
          informationScreen.SetUp(text, destroyTime);
     }
}