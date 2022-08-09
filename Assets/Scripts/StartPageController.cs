using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


//This Script controlls the behaviour of the start page in the starter page
public class StartPageController : MonoBehaviour
{
     private UIDocument startPage;
     private Button startButton;
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

     //Gets the UI elements from the panel 
     private void getElements()
     {
          //Get the UI component (the element that has the script attached)
          startPage = GetComponent<UIDocument>();
          print(startPage.rootVisualElement.name);

          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          startButton = startPage.rootVisualElement.Query<Button>("StartButton");
          exitButton = startPage.rootVisualElement.Query<Button>("ExitButton");
     }

     //Sets the buttons to trigger certain functions as a response to an especific event
     private void registerEvents()
     {
          startButton.RegisterCallback<ClickEvent>(startButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(exitButtonOnClick);
     }
     //Removes the callbacks from the buttons (when the UI document gets disabled)
     private void unregisterEvents()
     {
          startButton.UnregisterCallback<ClickEvent>(startButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(exitButtonOnClick);
     }

     //Function that triggers when the startButton gets clicked
     private void startButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }

     //Function that triggers when the exitButton gets clicked
     private void exitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }

}
