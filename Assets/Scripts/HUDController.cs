using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
     public GameData gameData;
     UIDocument HUD;
     ProgressBar healthBar;
     Button nextWaveButton;

     void Start()
     {
          getElements();
     }

     void Update()
     {
          healthBar.value = gameData.health;
          nextWaveButton.RegisterCallback<ClickEvent>(nextWaveButtonOnClick);
     }

     void getElements()
     {
          HUD = this.GetComponent<UIDocument>();
          healthBar = HUD.rootVisualElement.Query<ProgressBar>("HealthBar");
          nextWaveButton = HUD.rootVisualElement.Query<Button>("NextWaveButton");
     }

     void nextWaveButtonOnClick(ClickEvent evt)
     {
          gameData.sendNextWave = true;
     }
}
