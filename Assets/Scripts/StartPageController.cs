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
     private Button highScoresButton;
     private Button exitButton;
     private VisualElement contentContainer;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
     }
     private void Start()
     {
          //Starts with the contentContainer disabled
          contentContainer.SetEnabled(false);
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
          highScoresButton = startPage.rootVisualElement.Query<Button>("HighScoresButton");
          exitButton = startPage.rootVisualElement.Query<Button>("ExitButton");
          contentContainer = startPage.rootVisualElement.Query<VisualElement>("ContentContainer");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          startButton.RegisterCallback<ClickEvent>(StartButtonOnClick);
          highScoresButton.RegisterCallback<ClickEvent>(HighScoresButtonOnClick);
          exitButton.RegisterCallback<ClickEvent>(ExitButtonOnClick);
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          startButton.UnregisterCallback<ClickEvent>(StartButtonOnClick);
          highScoresButton.UnregisterCallback<ClickEvent>(HighScoresButtonOnClick);
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
     /// Function that triggers when the highScoresButton gets clicked. displays the highscores
     /// </summary>
     /// <param name="evt">Click event that triggered the callback</param>
     private void HighScoresButtonOnClick(ClickEvent evt)
     {
          if (IsAlreadyOpen()) return;

          contentContainer.SetEnabled(true);
          AddTitle();
          AddHighScores();

          bool IsAlreadyOpen()
          {
               return contentContainer.enabledInHierarchy;
          }

          void AddTitle()
          {
               Label title = new("HIGH SCORES");
               title.AddToClassList("highScoresTitle");
               contentContainer.Add(title);
          }

          void AddHighScores()
          {
               foreach (int highScore in highScoresManager
                         .highScores)
               {
                    VisualElement highScoreContainer = new();
                    highScoreContainer.AddToClassList("highScoreContainer");
                    int position = highScoresManager.highScores.IndexOf(highScore) + 1;
                    Label highScorePositionText = new($"#{position}");
                    highScorePositionText.AddToClassList("highScorePositionText");
                    Label highScoreText = new($"{highScore} Waves");
                    highScoreText.AddToClassList("highScoreText");
                    highScoreContainer.Add(highScorePositionText);
                    highScoreContainer.Add(highScoreText);
                    contentContainer.Add(highScoreContainer);
               }
          }
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
