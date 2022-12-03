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
     private UIDocument startPage;
     private Button startButton;
     private Button selectTowersButton;
     private Button exitButton;
     private void OnEnable()
     {
          getElements();
          registerEvents();
     }
     private void OnDisable()
     {
          unregisterEvents();
     }
     /// <summary>
     /// Gets the UI elements from the panel 
     /// </summary>
     private void getElements()
     {
          //Get the UI component (the element that has the script attached)
          startPage = GetComponent<UIDocument>();
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          startButton = startPage.rootVisualElement.Query<Button>("StartButton");
          selectTowersButton = startPage.rootVisualElement.Query<Button>("SelectTowersButton");
          exitButton = startPage.rootVisualElement.Query<Button>("ExitButton");
     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void registerEvents()
     {
          startButton.RegisterCallback<ClickEvent>(startButtonOnClick);
          selectTowersButton.RegisterCallback<ClickEvent>(selectTowersButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(exitButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void unregisterEvents()
     {
          startButton.UnregisterCallback<ClickEvent>(startButtonOnClick);
          selectTowersButton.UnregisterCallback<ClickEvent>(selectTowersButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(exitButtonOnClick);
     }
     /// <summary>
     /// Function that triggers when the startButton gets clicked
     /// </summary>
     /// <param name="evt"> Click event that triggered the callback</param>
     private void startButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }
     /// <summary>
     /// Function that triggers when the selectTowersButton gets clicked
     /// </summary>
     /// <param name="evt"> Click event that triggered the callback</param>
     private void selectTowersButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("TowerManager"); //Make sure the scene is already in the build settings
     }
     /// <summary>
     /// //Function that triggers when the exitButton gets clicked
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void exitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }

}
