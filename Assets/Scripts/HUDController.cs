using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Class that controlls the behaviour of the HUD
/// </summary>
public class HUDController : MonoBehaviour
{
     public GameData gameData;
     UIDocument HUD;
     ProgressBar healthBar;
     Button nextWaveButton;
     VisualElement towerCreationPanel;
     [SerializeField]
     VisualTreeAsset towerCreationPanelMaximized;
     [SerializeField]
     VisualTreeAsset towerCreationPanelMinimized;

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
          }
          if (towerCreationPanel.ClassListContains("towerCreationPanel-hidden"))
          {
               towerCreationPanel.Clear();
               towerCreationPanel.Add(towerCreationPanelMinimized.CloneTree());
          }
     }
}
