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
          VisualElement channalizerPanel = HUD.rootVisualElement.Query<VisualElement>("ChannalizerPanel");
          VisualElement structurePanel = HUD.rootVisualElement.Query<VisualElement>("StructurePanel");
          VisualElement sourcePanel = HUD.rootVisualElement.Query<VisualElement>("SourcePanel");
          VisualElement partsContainer;
          VisualElement addedPart;
          /// <summary>
          /// Dictionary that match each rarity with the corresponding USS class
          /// </summary>
          /// <value>A string corresponding to the USS class to be assigned</value>
          Dictionary<Rarity, string> rarityClasses = new Dictionary<Rarity, string>
          {
               {Rarity.Common,"commonRarity"},
               {Rarity.Normal,"normalRarity"},
               {Rarity.Rare,"rareRarity"},
               {Rarity.UltraRare,"ultraRareRarity"},
               {Rarity.Myth,"mythRarity"}
          };
          switch (part.type)
          {
               case PartType.Channalizer:
                    partsContainer = channalizerPanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(partTemplate.CloneTree());
                    ModifyPart(partsContainer);
                    break;
               case PartType.Structure:
                    partsContainer = structurePanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(partTemplate.CloneTree());
                    ModifyPart(partsContainer);
                    break;
               case PartType.Source:
                    partsContainer = sourcePanel.Query<VisualElement>("PartsContainer");
                    partsContainer.Add(partTemplate.CloneTree());
                    ModifyPart(partsContainer);
                    break;
          }
          /// <summary>
          /// Helper function to add the class corresponding to the rarity and change the name of the part so a new one can be added without problem
          /// </summary>
          /// <param name="container"></param>
          void ModifyPart(VisualElement container)
          {
               addedPart = container.Query<VisualElement>("Part");
               addedPart.AddToClassList(rarityClasses[part.rarity]);
               addedPart.name = "OldPart"; //to avoid conflict when searching for a new part
          }
     }
}
