using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// This Script controlls the behaviour of the start page in the starter page
/// </summary>
public class StartPageController : MonoBehaviour
{
     /// <summary>
     /// Used reference to gameData so that the gameData scriptable object is created in the startpage scene (else it won't work in the build)
     /// </summary>
     public GameData gameData;
     public HighScoresManager highScoresManager;
     private UIDocument startPage;
     private Button startButton;
     private Button exitButton;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
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
          startPage = GetComponent<UIDocument>();
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          startButton = startPage.rootVisualElement.Query<Button>("StartButton");
          exitButton = startPage.rootVisualElement.Query<Button>("ExitButton");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          startButton.RegisterCallback<ClickEvent>(StartButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          startButton.UnregisterCallback<ClickEvent>(StartButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Function that triggers when the startButton gets clicked.Loads the gameScene and fires the GameStarted event
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void StartButtonOnClick(ClickEvent evt)
     {
          GameData.GameStarted?.Invoke();
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }
     /// <summary>
     /// Function that triggers when the exitButton gets clicked
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void ExitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }
}
