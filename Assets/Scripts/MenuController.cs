using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


//This Script controlls the behaviour of the menu panel in the starter page
public class MenuController : MonoBehaviour
{
     private UIDocument menuDoc;
     private Button startButton;
     private Button quitButton;

     private void OnEnable()
     {
          getElements();
          registerEvents();

     }

     private void OnDisable()
     {
          unregisterEvents();
     }

     //Function that triggers when the startButton gets clicked
     private void startButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }

     //Function that triggers when the quitButton gets clicked
     private void quitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }

     private void getElements()
     {
          //Get the UI component (the element that has the script attached)
          menuDoc = GetComponent<UIDocument>();

          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q)
          startButton = menuDoc.rootVisualElement.Query<Button>("StartButton");
          quitButton = menuDoc.rootVisualElement.Query<Button>("QuitButton");
     }
     private void registerEvents()
     {
          //Sets the buttons to trigger certain functions as a response to an especific event
          startButton.RegisterCallback<ClickEvent>(startButtonOnClick);
          quitButton.RegisterCallback<ClickEvent>(quitButtonOnClick);
     }
     private void unregisterEvents()
     {
          //Removes the callbacks from the buttons (when the UI document gets disabled)
          startButton.UnregisterCallback<ClickEvent>(startButtonOnClick);
          quitButton.UnregisterCallback<ClickEvent>(quitButtonOnClick);
     }
}
