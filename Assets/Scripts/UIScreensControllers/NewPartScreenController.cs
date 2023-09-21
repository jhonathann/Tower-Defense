using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Control the behaviour of the new part Screen
/// </summary>
public class NewPartScreenController : MonoBehaviour
{
     /// <summary>
     /// Access to the gameData
     /// </summary>
     public GameData gameData;
     //References to the ui elements
     private UIDocument newPartScren;
     private VisualElement globalContainer;
     private VisualElement partContainer;
     private Button okButton;
     private void OnEnable()
     {
          GetElements();
          RegisterEvents();
     }
     /// <summary>
     /// Gets the UI elements from the panel 
     /// </summary>
     private void GetElements()
     {
          //Get the UI component (the element that has the script attached)
          newPartScren = GetComponent<UIDocument>();
          //Get the GlobalContainer VisualElement diferente a rootVisualElement
          globalContainer = newPartScren.rootVisualElement.Query<VisualElement>("GlobalContainer");
          //Search for the buttons inside the menu
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          partContainer = newPartScren.rootVisualElement.Query<VisualElement>("PartContainer");
          okButton = newPartScren.rootVisualElement.Query<Button>("OkButton");

     }
     /// <summary>
     /// Sets the buttons to trigger certain functions as a response to an especific event
     /// </summary>
     private void RegisterEvents()
     {
          okButton.RegisterCallback<ClickEvent>(OkButtonOnClick);
     }
     private void Start()
     {
          //Starts the game with the screen undisplayed
          globalContainer.style.display = DisplayStyle.None;
          //Subscribe to the new parts added event
          PartsManager.NewPartsAdded += OnNewPartsAdded;
     }
     private void OnDisable()
     {
          UnregisterEvents();
     }
     /// <summary>
     /// Removes the callbacks from the buttons (when the UI document gets disabled)
     /// </summary>
     private void UnregisterEvents()
     {
          okButton.UnregisterCallback<ClickEvent>(OkButtonOnClick);
     }

     /// <summary>
     /// Method that is called when the GotItButton is clicked
     /// </summary>
     /// <param name="evt">The event information (unused)</param>
     private void OkButtonOnClick(ClickEvent evt)
     {
          globalContainer.style.display = DisplayStyle.None;
          gameData.gameState.TrySetState(GameStateType.Running);
          ///clears the cointainer for the next call 
          this.partContainer.Clear();
     }

     /// <summary>
     /// Function executed when a new part (or parts) is added, adds the adequate text to the part and displays the panel.
     /// </summary>
     /// <param name="addedParts">list of added parts</param>
     private void OnNewPartsAdded(List<Part> addedParts)
     {
          globalContainer.style.display = DisplayStyle.Flex;
          gameData.gameState.TrySetState(GameStateType.OnNewPartAddedScreen);
          foreach (Part part in addedParts)
          {
               //Generates a mirage of the part and adds it to the partContainer
               Part partToBeAdded = new(part);
               partToBeAdded.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
               partToBeAdded.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
               partToBeAdded.AddToClassList("part");
               VisualElement description = new();
               description.AddToClassList("description");
               partToBeAdded.iconContainer.Add(description);
               AddStatsLabels(description, partToBeAdded);
               this.partContainer.Add(partToBeAdded);
          }
          void AddStatsLabels(VisualElement visualElement, Part part)
          {
               switch (part.type)
               {
                    case PartType.Source:
                         switch (part.specificTypeInfo)
                         {
                              case SourceType.Earth:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Effect: Stun for {TowerStats.earthSourceTimes[part.rarity]} sec making the enemy unable to move"));
                                   break;
                              case SourceType.Fire:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Effect: Burn for {TowerStats.fireSourceTimes[part.rarity]} sec making the enemy take {TowerStats.FIRE_BURN_DAMAGE} damage each {TowerStats.FIRE_BURN_INTERVAL_TIME} sec"));
                                   break;
                              case SourceType.Thunder:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Effect: Confussion for {TowerStats.thunderSourceTimes[part.rarity]} sec making the enemy go in the opposite direction"));
                                   break;
                              case SourceType.Water:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Effect: Slow for {TowerStats.waterSourceTimes[part.rarity]} sec halving the enemy speed"));
                                   break;
                         }
                         break;
                    case PartType.Structure:
                         switch (part.specificTypeInfo)
                         {
                              case StructureType.Beam:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Range: {TowerStats.beamStructureStats[part.rarity] / 10} Tiles"));
                                   break;
                              case StructureType.Circular:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Radius: {TowerStats.circularStructureStats[part.rarity] / 10} Tiles"));
                                   break;
                              case StructureType.Cross:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Range: {TowerStats.crossStructureStats[part.rarity] / 10} Tiles"));
                                   break;
                         }
                         break;
                    case PartType.Channeler:
                         switch (part.specificTypeInfo)
                         {
                              case ChannelerType.Area:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Attacks all the enemies in range"));
                                   visualElement.Add(new Label($"Damage: {TowerStats.areaChannelerStats[part.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.areaChannelerStats[part.rarity].fireRate} Shots/sec"));
                                   break;
                              case ChannelerType.Fast:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Damage: {TowerStats.fastChannelerStats[part.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.fastChannelerStats[part.rarity].fireRate} Shots/second"));
                                   break;
                              case ChannelerType.Strong:
                                   visualElement.Add(new Label($"{part.rarity} {part.type}"));
                                   visualElement.Add(new Label($"{part.specificTypeInfo}"));
                                   visualElement.Add(new Label($"Damage: {TowerStats.strongChannelerStats[part.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.strongChannelerStats[part.rarity].fireRate} Shots/second"));
                                   break;
                         }
                         break;
               }
          }
     }
}