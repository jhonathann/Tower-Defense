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
     private Button manageTowersButton;
     private Button exitButton;
     private VisualElement leftColumn;
     private VisualElement contentContainer;
     private VisualElement partsContainer;
     [SerializeField]
     private VisualTreeAsset card;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
     }
     private void OnDisable()
     {
          UnregisterEvents();
     }
     private void Start()
     {
          contentContainer.SetEnabled(false); //This container starts unenabled;
     }
     /// <summary>
     /// Gets the UI elements from the panel 
     /// </summary>
     private void GetElements()
     {
          //Get the UI component (the element that has the script attached)
          startPage = GetComponent<UIDocument>();
          leftColumn = startPage.rootVisualElement.Query<VisualElement>("LeftColumn");
          contentContainer = startPage.rootVisualElement.Query<VisualElement>("ContentContainer");
          partsContainer = startPage.rootVisualElement.Query<VisualElement>("PartsContainer");
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          startButton = startPage.rootVisualElement.Query<Button>("StartButton");
          manageTowersButton = startPage.rootVisualElement.Query<Button>("ManageTowersButton");
          exitButton = startPage.rootVisualElement.Query<Button>("ExitButton");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          startButton.RegisterCallback<ClickEvent>(StartButtonOnClick);
          manageTowersButton.RegisterCallback<ClickEvent>(ManageTowersButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          startButton.UnregisterCallback<ClickEvent>(StartButtonOnClick);
          manageTowersButton.UnregisterCallback<ClickEvent>(ManageTowersButtonOnClick);
          exitButton.UnregisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Function that triggers when the startButton gets clicked
     /// </summary>
     /// <param name="evt"> Click event that triggered the callback</param>
     private void StartButtonOnClick(ClickEvent evt)
     {
          SceneManager.LoadSceneAsync("Game"); //Make sure the scene is already in the build settings
     }
     /// <summary>
     /// Function that triggers when the selectTowersButton gets clicked
     /// </summary> 
     /// <param name="evt"> Click event that triggered the callback</param>
     private void ManageTowersButtonOnClick(ClickEvent evt)
     {
          leftColumn.Clear();
          Button createTower = CreateButton("CREATE TOWER", CreateTowerOnClick);
          leftColumn.Add(createTower);
          Button createTowerSet = CreateButton("CREATE TOWER SET", CreateTowerSetOnClick);
          leftColumn.Add(createTowerSet);
          Button back = CreateButton("BACK", BackButtonOnClick);
          leftColumn.Add(back);
          VisualElement VE = card.CloneTree();
          partsContainer.Add(VE);
          contentContainer.SetEnabled(true);
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
     /// <summary>
     /// Function Triggered when the create Tower Button is clicked
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void CreateTowerOnClick(ClickEvent evt)
     {
          Debug.Log(1);
     }
     /// <summary>
     /// Function that triggers when the CreateTowerSet is clicked
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void CreateTowerSetOnClick(ClickEvent evt)
     {
          Debug.Log(2);
     }
     /// <summary>
     /// Function that thiggers when the BackButton is clicked
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void BackButtonOnClick(ClickEvent evt)
     {
          Debug.Log(3);
     }
     /// <summary>
     /// Helper Function that creates a button, assigns the corresponding class, assigns the corresponding callback and returns it
     /// </summary>
     /// <param name="text"> Text that will be displayed in the button</param>
     /// <param name="onClick"> Callback function that will be executed when the button is clicked</param>
     /// <returns>Returns the created button</returns>
     private Button CreateButton(string text, EventCallback<ClickEvent> onClick)
     {
          Button button = new Button();
          button.RegisterCallback(onClick);
          button.text = text;
          button.AddToClassList("button");
          return button;
     }
}
