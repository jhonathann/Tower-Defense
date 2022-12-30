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
     Button nextWaveButton;
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
     [SerializeField]
     /// <summary>
     /// VisualTree of the part
     /// </summary>
     VisualTreeAsset partTemplate;
     public static Action RenderPanel;

     void Start()
     {
          GetElements();
          nextWaveButton.RegisterCallback<ClickEvent>(NextWaveButtonOnClick);
          RenderPanel += RenderCreationPanel;
          RenderPanel?.Invoke();
     }

     void Update()
     {
          healthBar.value = gameData.health;
          if (Input.GetKeyDown(KeyCode.T))
          {
               ToggleTowerCreationPanel();
          }
     }

     /// <summary>
     /// Used to get the reference of each of the individual components
     /// </summary>
     void GetElements()
     {
          HUD = this.GetComponent<UIDocument>();
          healthBar = HUD.rootVisualElement.Query<ProgressBar>("HealthBar");
          nextWaveButton = HUD.rootVisualElement.Query<Button>("NextWaveButton");
          towerCreationPanel = HUD.rootVisualElement.Query<VisualElement>("TowerCreationPanel");
     }

     void NextWaveButtonOnClick(ClickEvent evt)
     {
          PortalController.NextWave?.Invoke();
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
                    Tower.OnCreateTower(gameData.channalizerSelectedPart, gameData.structureSelectedPart, gameData.sourceSelectedPart);
                    //toggle the panel to allow the user to place the tower
                    ToggleTowerCreationPanel();
               }
          }
          if (towerCreationPanel.ClassListContains("towerCreationPanel-hidden"))
          {
               towerCreationPanel.Clear();
               towerCreationPanel.Add(towerCreationPanelMinimized.CloneTree());
          }
     }
     /// <summary>
     /// Adds a mockup of the part to the creation slot
     /// </summary>
     void AddSelectedParts()
     {
          VisualElement channalizerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannalizerPanel");
          VisualElement structurePanel = HUD.rootVisualElement.Query<VisualElement>("StructurePanel");
          VisualElement sourcePanel = HUD.rootVisualElement.Query<VisualElement>("SourcePanel");
          VisualElement channalizerCreationSlot = channalizerPanel.Query<VisualElement>("CreationSlot");
          VisualElement structureCreationSlot = structurePanel.Query<VisualElement>("CreationSlot"); VisualElement sourceCreationSlot = sourcePanel.Query<VisualElement>("CreationSlot");
          channalizerCreationSlot.Add(new Part(gameData.channalizerSelectedPart));
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
               case PartType.Channalizer:
                    VisualElement channalizerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannalizerPanel");
                    partsContainer = channalizerPanel.Query<VisualElement>("PartsContainer");
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
