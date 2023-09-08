using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Class that controlls the behaviour of the HUD
/// </summary>
public class HUDController : MonoBehaviour
{
     /// <summary>
     /// Reference to the game data to get the health and the parts
     /// </summary>
     public GameData gameData;
     //HUD necessary references
     UIDocument HUD;
     ProgressBar healthBar;
     Label waveCountLabel;
     Label nextWaveTimeLabel;
     Button timeSpeedButton;
     VisualElement towerCreationPanel;
     /// <summary>
     /// VisualTree to be put inside the towerCreationPanel when maximized
     /// </summary>
     [SerializeField]
     VisualTreeAsset towerCreationPanelMaximized;
     /// <summary>
     /// VisualTree to be put inside the towerCreationPanel when minimized
     /// </summary>
     [SerializeField]
     VisualTreeAsset towerCreationPanelMinimized;
     public static Action RenderPanel;

     void Start()
     {
          GetElements();
          RenderPanel += RenderCreationPanel;
          RenderPanel?.Invoke();
          RegisterEvents();
     }

     void Update()
     {
          healthBar.value = gameData.health;
          nextWaveTimeLabel.text = "Next Wave In: " + gameData.timerString;
          waveCountLabel.text = "Wave " + gameData.waveCount;
          if (Input.GetKeyDown(KeyCode.T))
          {
               //Allows the toggle only when the game is Running(to avoid changes when player is placing a tower of when the game is over)
               if (gameData.gameState.State == GameStateType.Running)
               {
                    ToggleTowerCreationPanel();
               }
          }
     }
     void OnDisable()
     {
          UnregisterEvents();
     }
     /// <summary>
     /// Removes the subscription to the static Action when the gameObject is destroyed(when the scene is reloaded)
     /// </summary>
     void OnDestroy()
     {
          RenderPanel -= RenderCreationPanel;
     }
     /// <summary>
     /// Used to get the reference of each of the individual components
     /// </summary>
     void GetElements()
     {
          HUD = this.GetComponent<UIDocument>();
          healthBar = HUD.rootVisualElement.Query<ProgressBar>("HealthBar");
          nextWaveTimeLabel = HUD.rootVisualElement.Query<Label>("NextWaveTimeLabel");
          timeSpeedButton = HUD.rootVisualElement.Query<Button>("TimeSpeedButton");
          waveCountLabel = HUD.rootVisualElement.Query<Label>("WaveCountLabel");
          towerCreationPanel = HUD.rootVisualElement.Query<VisualElement>("TowerCreationPanel");
     }
     /// <summary>
     /// Registers Events to Buttons
     /// </summary>
     void RegisterEvents()
     {
          timeSpeedButton.RegisterCallback<ClickEvent>(TimeSpeedButtonOnClick);
     }
     /// <summary>
     /// Unrergister Events of Buttons
     /// </summary>
     void UnregisterEvents()
     {
          timeSpeedButton.UnregisterCallback<ClickEvent>(TimeSpeedButtonOnClick);
     }
     /// <summary>
     /// Function Called when the TimeSpeed Button is clicked, it toggles between x1 and x2 speed, and calls the gamestate functions to set it.
     /// </summary>
     /// <param name="evt">Click Event information</param>
     private void TimeSpeedButtonOnClick(ClickEvent evt)
     {
          if (timeSpeedButton.text == "X1")
          {
               timeSpeedButton.text = "X2";
               GameState.TimeSpeed = 2;
          }
          else if (timeSpeedButton.text == "X2")
          {
               timeSpeedButton.text = "X1";
               GameState.TimeSpeed = 1;
          }
          gameData.gameState.TrySetState(GameStateType.Running);
     }
     /// <summary>
     /// Toggles the tower creation panel on and off and re-renders the panel accordingly
     /// </summary>
     void ToggleTowerCreationPanel()
     {
          towerCreationPanel.ToggleInClassList("towerCreationPanel");
          towerCreationPanel.ToggleInClassList("towerCreationPanel-hidden");
          RenderCreationPanel();
     }
     /// <summary>
     /// Used to render the panel taking according to its state
     /// </summary>
     void RenderCreationPanel()
     {
          if (towerCreationPanel.ClassListContains("towerCreationPanel"))
          {
               towerCreationPanel.UnregisterCallback<ClickEvent>(towerCreationPanelMinimizedOnClick);
               towerCreationPanel.Clear();
               towerCreationPanel.Add(towerCreationPanelMaximized.CloneTree());
               // Add the currently selected parts to the panel
               AddSelectedParts();
               //Add all the current parts to the panel
               foreach (Part part in gameData.parts)
               {
                    AddPartToUI(part);
               }
               //Register callback in the createTowerButton of the panel
               Button createTowerButton = HUD.rootVisualElement.Query<Button>("CreateTowerButton");
               createTowerButton.RegisterCallback<ClickEvent>(createTowerButtonOnClick);
               void createTowerButtonOnClick(ClickEvent evt)
               {
                    TowerData.OnCreateTower(gameData.channelerSelectedPart, gameData.structureSelectedPart, gameData.sourceSelectedPart);
                    //toggle the panel to allow the user to place the tower
                    ToggleTowerCreationPanel();
               }
          }
          if (towerCreationPanel.ClassListContains("towerCreationPanel-hidden"))
          {
               towerCreationPanel.RegisterCallback<ClickEvent>(towerCreationPanelMinimizedOnClick);
               towerCreationPanel.Clear();
               towerCreationPanel.Add(towerCreationPanelMinimized.CloneTree());
          }
          //This function is used to allow the user to open the towercreation panel by clicking on the minimized version
          void towerCreationPanelMinimizedOnClick(ClickEvent evt)
          {
               //Allows the toggle only when the game is Running(to avoid changes when player is placing a tower of when the game is over)
               if (gameData.gameState.State == GameStateType.Running)
               {
                    ToggleTowerCreationPanel();
               }
          }
     }
     /// <summary>
     /// Adds a mockup of the part to the creation slot
     /// </summary>
     void AddSelectedParts()
     {
          VisualElement channelerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannelerPanel");
          VisualElement structurePanel = HUD.rootVisualElement.Query<VisualElement>("StructurePanel");
          VisualElement sourcePanel = HUD.rootVisualElement.Query<VisualElement>("SourcePanel");
          VisualElement channelerCreationSlot = channelerPanel.Query<VisualElement>("CreationSlot");
          VisualElement structureCreationSlot = structurePanel.Query<VisualElement>("CreationSlot"); VisualElement sourceCreationSlot = sourcePanel.Query<VisualElement>("CreationSlot");
          channelerCreationSlot.Add(new Part(gameData.channelerSelectedPart));
          structureCreationSlot.Add(new Part(gameData.structureSelectedPart));
          sourceCreationSlot.Add(new Part(gameData.sourceSelectedPart));
     }
     /// <summary>
     /// Adds a part to de adequate container in the UI
     /// </summary>
     /// <param name="part"></param>
     void AddPartToUI(Part part)
     {
          VisualElement partsContainer;
          switch (part.type)
          {
               case PartType.Channeler:
                    VisualElement channelerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannelerPanel");
                    partsContainer = channelerPanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(part);
                    break;
               case PartType.Structure:
                    VisualElement structurePanel = HUD.rootVisualElement.Query<VisualElement>("StructurePanel");
                    partsContainer = structurePanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(part);
                    break;
               case PartType.Source:
                    VisualElement sourcePanel = HUD.rootVisualElement.Query<VisualElement>("SourcePanel");
                    partsContainer = sourcePanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(part);
                    break;
          }

     }
}
