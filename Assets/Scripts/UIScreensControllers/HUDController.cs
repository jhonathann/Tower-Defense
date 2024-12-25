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
     Button createTowerTab;
     Button mixPartsTab;
     Button closeButton;
     VisualElement partsPanel;
     VisualElement bodyContainer;
     /// <summary>
     /// VisualTree to be put inside the PartsPanel when the tower creation is being done
     /// </summary>
     [SerializeField]
     VisualTreeAsset towerCreationPanel;
     /// <summary>
     /// VisualTree to be put inside the PartsPanel when a mix parts is being triggered
     /// </summary>
     [SerializeField]
     VisualTreeAsset mixPartsPanel;
     /// <summary>
     /// Action that can be called to re-render the panel
     /// </summary>
     public static Action RenderPanel;
     //Event Triggered when the HUD changes (from one tab to another)
     public static event Action<CreationPanelState> ChangeOfHUDState;
     //Event Triggered when the CreateTowerButton Is pressed
     public static event Action CreateTowerTriggered;
     //Event Triggered when the MixPartsButton Is pressed
     public static event Action MixPartsTriggered;
     /// <summary>
     /// Variable used to determine the current state of the panel
     /// </summary>
     private CreationPanelState state;
     public CreationPanelState State
     {
          get
          {
               return state;
          }
          private set
          {
               state = value;
               ChangeOfHUDState?.Invoke(State);
               RenderCreationPanel();//Renders the panel again every time its state changes
          }
     }

     void Start()
     {
          GetElements();
          RenderPanel += RenderCreationPanel;
          RenderPanel?.Invoke();
          State = CreationPanelState.TowerCreation;
          RegisterEvents();
     }

     void Update()
     {
          //Updates Values
          healthBar.value = gameData.health;
          nextWaveTimeLabel.text = "Next Wave In: " + gameData.timerString;
          waveCountLabel.text = "Wave " + gameData.waveCount;
          //Allows the toggle only when the game is Running(to avoid changes when player is placing a tower of when the game is over)
          if (gameData.gameState.State == GameStateType.Running)
          {
               if (Input.GetKeyDown(KeyCode.T))
               {
                    if (State != CreationPanelState.Hidden)
                    {
                         State = CreationPanelState.Hidden;
                    }
                    else
                    {
                         State = CreationPanelState.TowerCreation;
                    }
               }
               if (Input.GetKeyDown(KeyCode.Space))
               {
                    TimeSpeedButtonOnClick();
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
          partsPanel = HUD.rootVisualElement.Query<VisualElement>("PartsPanel");
          bodyContainer = HUD.rootVisualElement.Query<VisualElement>("BodyContainer");
          createTowerTab = HUD.rootVisualElement.Query<Button>("CreateTowerTab");
          mixPartsTab = HUD.rootVisualElement.Query<Button>("MixPartsTab");
          closeButton = HUD.rootVisualElement.Query<Button>("CloseButton");
     }
     /// <summary>
     /// Registers Events to Buttons
     /// </summary>
     void RegisterEvents()
     {
          timeSpeedButton.RegisterCallback<ClickEvent>(TimeSpeedButtonOnClick);
          createTowerTab.RegisterCallback<ClickEvent>(CreateTowerTabOnClick);
          mixPartsTab.RegisterCallback<ClickEvent>(MixPartsTabOnClick);
          closeButton.RegisterCallback<ClickEvent>(CloseButtonOnClick);
     }
     /// <summary>
     /// Unrergister Events of Buttons
     /// </summary>
     void UnregisterEvents()
     {
          timeSpeedButton.UnregisterCallback<ClickEvent>(TimeSpeedButtonOnClick);
          createTowerTab.UnregisterCallback<ClickEvent>(CreateTowerTabOnClick);
          mixPartsTab.UnregisterCallback<ClickEvent>(MixPartsTabOnClick);
          closeButton.UnregisterCallback<ClickEvent>(CloseButtonOnClick);
     }
     /// <summary>
     /// Function Called when the TimeSpeed Button is clicked, it toggles between x0, x1 and x2 speed, and calls the gamestate functions to set it.
     /// </summary>
     /// <param name="evt">Click Event information</param>
     private void TimeSpeedButtonOnClick(ClickEvent evt = null)
     {
          if (timeSpeedButton.text == "X0")
          {
               timeSpeedButton.text = "X1";
               GameState.TimeSpeed = 1;
          }
          else if (timeSpeedButton.text == "X1")
          {
               timeSpeedButton.text = "X2";
               GameState.TimeSpeed = 2;
          }
          else if (timeSpeedButton.text == "X2")
          {
               timeSpeedButton.text = "X0";
               GameState.TimeSpeed = 0;
          }
          gameData.gameState.TrySetState(GameStateType.Running);
     }
     private void CreateTowerTabOnClick(ClickEvent evt)
     {
          State = CreationPanelState.TowerCreation;
     }
     private void MixPartsTabOnClick(ClickEvent evt)
     {
          State = CreationPanelState.MixParts;
     }
     private void CloseButtonOnClick(ClickEvent evt)
     {
          State = CreationPanelState.Hidden;
     }
     /// <summary>
     /// Used to render the panel taking according to its state
     /// </summary>
     void RenderCreationPanel()
     {
          switch (State)
          {
               case CreationPanelState.Hidden:
                    partsPanel.AddToClassList("partsPanel-hidden");
                    partsPanel.RegisterCallback<MouseDownEvent>(TowerCreationHiddenOnClick);
                    break;
               case CreationPanelState.TowerCreation:
                    partsPanel.RemoveFromClassList("partsPanel-hidden");
                    createTowerTab.AddToClassList("tab-selectedOne");
                    mixPartsTab.RemoveFromClassList("tab-selectedOne");
                    partsPanel.UnregisterCallback<MouseDownEvent>(TowerCreationHiddenOnClick);
                    bodyContainer.Clear();
                    bodyContainer.Add(towerCreationPanel.CloneTree());
                    // Add the currently selected parts to the panel
                    AddSelectedPartsToCreateTowerUI();
                    //Add all the current parts to the panel
                    foreach (Part part in gameData.partsManager.parts)
                    {
                         AddPartsToCreateTowerUI(part);
                    }
                    //Register callback in the createTowerButton of the panel
                    Button createTowerButton = HUD.rootVisualElement.Query<Button>("CreateTowerButton");
                    createTowerButton.RegisterCallback<ClickEvent>(createTowerButtonOnClick);
                    void createTowerButtonOnClick(ClickEvent evt)
                    {
                         CreateTowerTriggered?.Invoke();
                         //toggle the panel to allow the user to place the tower
                         State = CreationPanelState.Hidden;
                    }
                    break;
               case CreationPanelState.MixParts:
                    partsPanel.RemoveFromClassList("partsPanel-hidden");
                    mixPartsTab.AddToClassList("tab-selectedOne");
                    createTowerTab.RemoveFromClassList("tab-selectedOne");
                    partsPanel.UnregisterCallback<MouseDownEvent>(TowerCreationHiddenOnClick);
                    bodyContainer.Clear();
                    bodyContainer.Add(mixPartsPanel.CloneTree());
                    // Add the currently selected parts to the panel
                    AddSelectedPartsToMixPartsUI();
                    //Add all the current parts to the panel
                    foreach (Part part in gameData.partsManager.parts)
                    {
                         AddPartsToMixPartsUI(part);
                    }
                    //Register callback in the mixPartsButton of the panel
                    Button mixPartsButton = HUD.rootVisualElement.Query<Button>("MixPartsButton");
                    mixPartsButton.RegisterCallback<ClickEvent>((evt) =>
                    {
                         MixPartsTriggered?.Invoke();
                    });
                    break;
               default: break;
          }
          //This function is used to allow the user to open the towercreation panel by clicking on the hidden version
          void TowerCreationHiddenOnClick(MouseDownEvent evt)
          {
               //Allows the toggle only when the game is Running(to avoid changes when player is placing a tower of when the game is over)
               if (gameData.gameState.State == GameStateType.Running)
               {
                    State = CreationPanelState.TowerCreation;
               }
          }
          /// <summary>
          /// Adds a mockup of the part to the creation slot
          /// </summary>
          void AddSelectedPartsToCreateTowerUI()
          {
               VisualElement channelerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannelerPanel");
               VisualElement structurePanel = HUD.rootVisualElement.Query<VisualElement>("StructurePanel");
               VisualElement sourcePanel = HUD.rootVisualElement.Query<VisualElement>("SourcePanel");
               VisualElement channelerCreationSlot = channelerPanel.Query<VisualElement>("CreationSlot");
               VisualElement structureCreationSlot = structurePanel.Query<VisualElement>("CreationSlot"); VisualElement sourceCreationSlot = sourcePanel.Query<VisualElement>("CreationSlot");
               channelerCreationSlot.Add(new Part(gameData.partsManager.channelerSelectedPart));
               structureCreationSlot.Add(new Part(gameData.partsManager.structureSelectedPart));
               sourceCreationSlot.Add(new Part(gameData.partsManager.sourceSelectedPart));
          }
          /// <summary>
          /// Adds a mockup of the part to the mix slot
          /// </summary>
          void AddSelectedPartsToMixPartsUI()
          {
               VisualElement Part1 = bodyContainer.Query<VisualElement>("Part1");
               VisualElement Part2 = bodyContainer.Query<VisualElement>("Part2");
               VisualElement Part3 = bodyContainer.Query<VisualElement>("Part3");
               //An array out of bounds exeption will ocurr when the list isn't fully populated
               try
               {
                    Part1.Add(new Part(gameData.partsManager.partsToMix[0]));
                    Part2.Add(new Part(gameData.partsManager.partsToMix[1]));
                    Part3.Add(new Part(gameData.partsManager.partsToMix[2]));
               }
               catch { }

          }
          /// <summary>
          /// Adds a part to de adequate container in the UI
          /// </summary>
          /// <param name="part"></param>
          void AddPartsToCreateTowerUI(Part part)
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
          void AddPartsToMixPartsUI(Part part)
          {
               VisualElement partsPanel = bodyContainer.Query<VisualElement>("PartsPanel");
               partsPanel.Add(part);
          }
     }
}
public enum CreationPanelState
{
     Hidden,
     TowerCreation,
     MixParts
}