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

     void Start()
     {
          GetElements();
          nextWaveButton.RegisterCallback<ClickEvent>(NextWaveButtonOnClick);
          RenderCreationPanel();
     }

     void Update()
     {
          healthBar.value = gameData.health;
          ToggleTowerCreationPanel();
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
          if (Input.GetKeyDown(KeyCode.T))
          {
               towerCreationPanel.ToggleInClassList("towerCreationPanel");
               towerCreationPanel.ToggleInClassList("towerCreationPanel-hidden");
               RenderCreationPanel();
          }
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
               //Add all the current parts to the panel
               foreach (Part part in gameData.parts)
               {
                    AddPartToUI(part);
               }
          }
          if (towerCreationPanel.ClassListContains("towerCreationPanel-hidden"))
          {
               towerCreationPanel.Clear();
               towerCreationPanel.Add(towerCreationPanelMinimized.CloneTree());
          }
     }
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
