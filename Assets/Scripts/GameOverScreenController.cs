using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the GameOverScreen
/// </summary>
public class GameOverScreenController : MonoBehaviour
{
     /// <summary>
     /// Access to the gameData
     /// </summary>
     public GameData gameData;
     //References to the ui elements
     private UIDocument gameOverScreen;
     private VisualElement globalContainer;
     private Label scoreText;
     private Button playAgainButton;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
          //Subscribe to the castle destroyed event
          CastleController.CastleDestroyed += OnCastleDestroy;
     }
     /// <summary>
     /// Called when the castle is destroyed
     /// </summary>
     private void OnCastleDestroy()
     {
          //Change the state of the game to GameOver
          gameData.State = GameState.GameOver;
          //Shows the gameOverScreen
          globalContainer.style.display = DisplayStyle.Flex;
          //Sets the adequate score text
          scoreText.text = $"You survived for {gameData.waveCount} waves";
     }
     private void Start()
     {
          //Starts the game with the screen undisplayed
          globalContainer.style.display = DisplayStyle.None;
     }

     private void OnDisable()
     {
          UnregisterEvents();
     }
     /// <summary>
     /// Gets the UI elements from the panel 
     /// </summary>
     private void GetElements()
     {
          //Get the UI component (the element that has the script attached)
          gameOverScreen = GetComponent<UIDocument>();
          //Get the GlobalContainer VisualElement diferente a rootVisualElement
          globalContainer = gameOverScreen.rootVisualElement.Query<VisualElement>("GlobalContainer");
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          scoreText = gameOverScreen.rootVisualElement.Query<Label>("ScoreText");
          playAgainButton = gameOverScreen.rootVisualElement.Query<Button>("PlayAgainButton");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          playAgainButton.RegisterCallback<ClickEvent>(PlayAgainButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          playAgainButton.UnregisterCallback<ClickEvent>(PlayAgainButtonOnClick);
     }
     /// <summary>
     /// Called when the PlayAgainButton is clicked. Fires the GameSatarted event and reloads the gameScene
     /// </summary>
     /// <param name="evt">The click event information (unused)</param>
     private void PlayAgainButtonOnClick(ClickEvent evt)
     {
          GameData.GameStarted?.Invoke();
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }
}
