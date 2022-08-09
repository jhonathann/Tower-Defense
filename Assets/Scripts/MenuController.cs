using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Script that controlls the behaviour of the menu
public class MenuController : MonoBehaviour
{
     private UIDocument menu;
     private VisualElement globalContainer;
     private Button continueButton;
     private Button exitButton;
     private void OnEnable()
     {
          getElements();
          registerEvents();
     }
     private void Start()
     {
          globalContainer.SetEnabled(false); //the menu starts the game disabled
     }
     private void Update()
     {
          toggleMenu();
     }
     private void OnDisable()
     {
          unregisterEvents();
     }
     //Gets the UI elements from the panel 
     private void getElements()
     {
          //Get the UI component (the element that has the script attached)
          menu = GetComponent<UIDocument>();
          //Get the GlobalContainer VisualElement diferente a rootVisualElement
          globalContainer = menu.rootVisualElement.Query<VisualElement>("GlobalContainer");
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          continueButton = menu.rootVisualElement.Query<Button>("ContinueButton");
          exitButton = menu.rootVisualElement.Query<Button>("ExitButton");

     }
     //Sets the buttons to trigger certain functions as a response to an especific event
     private void registerEvents()
     {
          continueButton.RegisterCallback<ClickEvent>(continueButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(exitButtonOnClick);
     }
     //Toggles the menu on and off when esc is pressed and pauses/unpauses the game accordingly
     private void toggleMenu()
     {
          if (Input.GetKeyDown(KeyCode.Escape))
          {
               globalContainer.SetEnabled(!globalContainer.enabledInHierarchy);
               Time.timeScale = System.Convert.ToSingle(!globalContainer.enabledInHierarchy);
          }
     }
     //Removes the callbacks from the buttons (when the UI document gets disabled)
     private void unregisterEvents()
     {
          continueButton.UnregisterCallback<ClickEvent>(continueButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(exitButtonOnClick);
     }
     //Function that triggers when the continueButton gets clicked
     private void continueButtonOnClick(ClickEvent evt)
     {
          globalContainer.SetEnabled(false); //deactivates the game object of the menu
          Time.timeScale = 1;//unpauses the game
     }
     //Function that triggers when the exitButton gets clicked
     private void exitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }
}
