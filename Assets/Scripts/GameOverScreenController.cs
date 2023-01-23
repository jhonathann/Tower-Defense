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
     private Button goToStartPageButton;
     private Button exitButton;
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
          gameData.gameState.TrySetState(GameStateType.GameOver);
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
          goToStartPageButton = gameOverScreen.rootVisualElement.Query<Button>("GoToStartPageButton");
          exitButton = gameOverScreen.rootVisualElement.Query<Button>("ExitButton");
     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          playAgainButton.RegisterCallback<ClickEvent>(PlayAgainButtonOnClick);
          goToStartPageButton.RegisterCallback<ClickEvent>(GoToStartPageButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          playAgainButton.UnregisterCallback<ClickEvent>(PlayAgainButtonOnClick);
          goToStartPageButton.UnregisterCallback<ClickEvent>(GoToStartPageButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Called when the PlayAgainButton is clicked. Fires the GameStarted event and reloads the gameScene
     /// </summary>
     /// <param name="evt">The click event information (unused)</param>
     private void PlayAgainButtonOnClick(ClickEvent evt)
     {
          GameData.GameStarted?.Invoke();
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }
     /// <summary>
     /// Called when the GoToStartPageButton is clicked. Loads the StartPage scene
     /// </summary>
     /// <param name="evt">The click event information (unused)</param>
     private void GoToStartPageButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("StartPage");
     }
     /// <summary>
     /// Called when the ExitButton is clicked. Closes the game
     /// </summary>
     /// <param name="evt">The click event information (unused)</param>
     private void ExitButtonOnClick(ClickEvent evt)
     {
          Application.Quit();
     }
}
