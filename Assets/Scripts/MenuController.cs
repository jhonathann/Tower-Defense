using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Script that controlls the behaviour of the menu
/// </summary>
public class MenuController : MonoBehaviour
{
     /// <summary>
     /// Reference to the gameData
     /// </summary>
     public GameData gameData;
     //References to the Ui elements
     private UIDocument menu;
     private VisualElement globalContainer;
     private Button continueButton;
     private Button exitButton;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
     }
     private void Start()
     {
          globalContainer.SetEnabled(false); //the menu starts the game disabled
     }
     private void Update()
     {
          ToggleMenu();
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
          menu = GetComponent<UIDocument>();
          //Get the GlobalContainer VisualElement diferente a rootVisualElement
          globalContainer = menu.rootVisualElement.Query<VisualElement>("GlobalContainer");
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          continueButton = menu.rootVisualElement.Query<Button>("ContinueButton");
          exitButton = menu.rootVisualElement.Query<Button>("ExitButton");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          continueButton.RegisterCallback<ClickEvent>(ContinueButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Toggles the menu on and off when esc is pressed and when the state of the game allows it
     /// </summary>
     private void ToggleMenu()
     {
          if (Input.GetKeyDown(KeyCode.Escape))
          {
               if (CanBePaused())
               {
                    globalContainer.SetEnabled(true);
                    gameData.State = GameState.Paused;
                    return;
               }
               if (IsPaused())
               {
                    globalContainer.SetEnabled(false);
                    gameData.State = GameState.Running;
                    return;
               }
          }
          bool CanBePaused()
          {
               return gameData.State == GameState.Running;
          }
          bool IsPaused()
          {
               return gameData.State == GameState.Paused;
          }
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          continueButton.UnregisterCallback<ClickEvent>(ContinueButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Function that triggers when the continueButton gets clicked
     /// </summary>
     /// <param name="evt">The ClickEvent information</param>
     private void ContinueButtonOnClick(ClickEvent evt)
     {
          globalContainer.SetEnabled(false); //deactivates the game object of the menu
          Time.timeScale = 1;//unpauses the game
     }
     /// <summary>
     /// Function that triggers when the exitButton gets clicked
     /// </summary>
     /// <param name="evt">The ClickEvent information</param>
     private void ExitButtonOnClick(ClickEvent evt)
     {
          //Closes the game
          Application.Quit();
     }
}
