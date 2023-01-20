using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
/// <summary>
/// Part Class for the parts that the enemies drop and are used to build the towers
/// </summary>
public class Part : VisualElement
{
     /// <summary>
     /// The type of the part
     /// </summary>
     public PartType type;
     /// <summary>
     /// The rarity of the part;
     /// </summary>
     public Rarity rarity;
     /// <summary>
     /// Specific Info about the effect of the part
     /// </summary>
     public Enum specificTypeInfo;
     /// <summary>
     /// template of the part (used to import the classes to be used)
     /// </summary>
     /// <typeparam name="VisualTreeAsset"><the uxml file with the classes uss file/typeparam>
     /// <returns></returns>
     public VisualTreeAsset template = Resources.Load<VisualTreeAsset>("Part");
     /// <summary>
     /// Dictionary that matches each rarity with the corresponding USS class
     /// </summary>
     /// <value>A string corresponding to the USS class to be assigned</value>
     private VisualElement contextMenu;
     Dictionary<Rarity, string> rarityClasses = new Dictionary<Rarity, string>
          {
               {Rarity.Common,"commonRarity"},
               {Rarity.Normal,"normalRarity"},
               {Rarity.Rare,"rareRarity"},
               {Rarity.UltraRare,"ultraRareRarity"},
               {Rarity.Myth,"mythRarity"}
          };
     /// <summary>
     /// Dictionary that matches each type with a USS class to display the icons of the elements in the UI 
     /// </summary>
     /// <value>A string corresponding to the USS class to be assigned</value>
     Dictionary<Enum, string> specificTypeClasses = new Dictionary<Enum, string>
          {
               {ChannalizerType.Fast,"channalizerFast"},
               {ChannalizerType.Strong,"channalizerStrong"},
               {ChannalizerType.Area,"channalizerArea"},
               {StructureType.Beam,"structureBeam"},
               {StructureType.Circular,"structureCircular"},
               {StructureType.Cross,"structureCross"},
               {SourceType.Earth,"sourceEarth"},
               {SourceType.Fire,"sourceFire"},
               {SourceType.Thunder,"sourceThunder"},
               {SourceType.Water,"sourceWater"}
          };
     /// <summary>
     /// This constructor initialized a new part with random (but weighted) rarity and random values
     /// </summary>
     public Part()
     {
          this.type = UtilityEnum.GetRandomTypeFromAnEnum<PartType>();
          this.rarity = GetRarity();
          this.specificTypeInfo = GetSpecificTypeInfo();
          template.CloneTree(this);
          this.AddToClassList(rarityClasses[this.rarity]);
          this.AddToClassList(specificTypeClasses[this.specificTypeInfo]);
          SetCallBacks();
          SetContextMenu();
     }
     /// <summary>
     /// This constructor returns a clone of the part that's passed in (with alteration on the width and height) (the cloned class doesn't have a registered callback) (used for the selectedContainerMockUp)
     /// </summary>
     /// <param name="part">Part to be cloned</param>
     public Part(Part part)
     {
          if (part == null) return;
          this.type = part.type;
          this.rarity = part.rarity;
          this.specificTypeInfo = part.specificTypeInfo;
          template.CloneTree(this);
          this.AddToClassList(rarityClasses[this.rarity]);
          this.AddToClassList(specificTypeClasses[this.specificTypeInfo]);
          this.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
          this.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
     }
     /// <summary>
     /// This constructor returns a random part of the specifyc type (use to ensure that the player has sufficient parts to create the initial tower)
     /// </summary>
     /// <param name="type"></param>
     public Part(PartType type)
     {
          this.type = type;
          this.rarity = GetRarity();
          this.specificTypeInfo = GetSpecificTypeInfo();
          template.CloneTree(this);
          this.AddToClassList(rarityClasses[this.rarity]);
          this.AddToClassList(specificTypeClasses[this.specificTypeInfo]);
          SetCallBacks();
          SetContextMenu();
     }
     /// <summary>
     /// Gets the rarity of the part by considered the probability of each option
     /// </summary>
     /// <returns>The randomly obtained rarity</returns>
     private Rarity GetRarity()
     {
          int chance = UnityEngine.Random.Range(0, 100);
          if (chance <= 40) return Rarity.Common; //40% chance of Common
          if (chance <= 70) return Rarity.Normal; // 30% chance of Normal
          if (chance <= 85) return Rarity.Rare; //15% chance of Rare
          if (chance <= 95) return Rarity.UltraRare; // 10% chance of UltraRare
          if (chance <= 100) return Rarity.Myth; //5% chance of Myth
          return Rarity.Common;
     }

     /// <summary>
     /// Gets a random specific type enum based on the (previously defined) type of the part
     /// </summary>
     /// <returns>An Enum with one of the three specific types</returns>
     private Enum GetSpecificTypeInfo()
     {
          switch (this.type)
          {
               case PartType.Source:
                    return UtilityEnum.GetRandomTypeFromAnEnum<SourceType>();
               case PartType.Structure:
                    return UtilityEnum.GetRandomTypeFromAnEnum<StructureType>();
               case PartType.Channalizer:
                    return UtilityEnum.GetRandomTypeFromAnEnum<ChannalizerType>();
               default: return null;
          }
     }
     /// <summary>
     /// Sets the callback function for the part (used in the constructor)
     /// </summary>
     private void SetCallBacks()
     {
          this.RegisterCallback<ClickEvent>(PartOnClick);
          this.RegisterCallback<MouseEnterEvent>(PartOnMouseEnter);
          this.RegisterCallback<MouseLeaveEvent>(PartOnMouseLeave);
     }
     /// <summary>
     /// Configures and adds the context menu to the part
     /// </summary>
     private void SetContextMenu()
     {
          this.contextMenu = CreateContextMenu();
          this.Add(contextMenu);
     }
     /// <summary>
     /// Callback triggered when the part is clicked
     /// </summary>
     /// <param name="evt">(not used)</param>
     private void PartOnClick(ClickEvent evt)
     {
          GameData.SelectPart(this);
          HUDController.RenderPanel?.Invoke();
     }
     /// <summary>
     /// Callback triggered when the mouse enters the part
     /// </summary>
     /// <param name="evt">(not used)</param>
     private void PartOnMouseEnter(MouseEnterEvent evt)
     {
          //Doubles the size of the part
          this.AddToClassList("partBigger");
          // Adds the context menu
          this.contextMenu.ToggleInClassList("contextMenu-show");
          this.contextMenu.ToggleInClassList("contextMenu-hidden");
     }
     /// <summary>
     /// Callback triggered when the mouse leaves the part
     /// </summary>
     /// <param name="evt">(not used)</param>
     private void PartOnMouseLeave(MouseLeaveEvent evt)
     {
          //Sets the size of the part to normal again
          this.RemoveFromClassList("partBigger");
          //Toggles the context menu classes
          this.contextMenu.ToggleInClassList("contextMenu-show");
          this.contextMenu.ToggleInClassList("contextMenu-hidden");
     }
     //Creates the context menu and adds the corresponding USS classe
     private VisualElement CreateContextMenu()
     {
          VisualElement contextMenu = new VisualElement();
          contextMenu.AddToClassList("contextMenu-hidden");
          AddStatsLabels(contextMenu);
          return contextMenu;

          //Switches to the part characteristics and adds the correct labels to the context menu
          void AddStatsLabels(VisualElement visualElement)
          {
               switch (this.type)
               {
                    case PartType.Source:
                         switch (this.specificTypeInfo)
                         {
                              case SourceType.Earth:
                                   visualElement.Add(new Label($"Effect: Stun {TowerStats.earthSourceTimes[this.rarity]} sec"));
                                   break;
                              case SourceType.Fire:
                                   visualElement.Add(new Label($"Effect: Burn {TowerStats.fireSourceTimes[this.rarity]} sec"));
                                   break;
                              case SourceType.Thunder:
                                   visualElement.Add(new Label($"Effect: Confussion {TowerStats.thunderSourceTimes[this.rarity]} sec"));
                                   break;
                              case SourceType.Water:
                                   visualElement.Add(new Label($"Effect: Slow {TowerStats.waterSourceTimes[this.rarity]} sec"));
                                   break;
                         }
                         break;
                    case PartType.Structure:
                         switch (this.specificTypeInfo)
                         {
                              case StructureType.Beam:
                                   visualElement.Add(new Label($"Range: {TowerStats.beamStructureStats[this.rarity] / 10} Tiles"));
                                   break;
                              case StructureType.Circular:
                                   visualElement.Add(new Label($"Radius: {TowerStats.circularStructureStats[this.rarity] / 10} Tiles"));
                                   break;
                              case StructureType.Cross:
                                   visualElement.Add(new Label($"Range: {TowerStats.crossStructureStats[this.rarity] / 10} Tiles"));
                                   break;
                         }
                         break;
                    case PartType.Channalizer:
                         switch (this.specificTypeInfo)
                         {
                              case ChannalizerType.Area:
                                   visualElement.Add(new Label($"Damage: {TowerStats.areaChannalizerStats[this.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.areaChannalizerStats[this.rarity].fireRate}"));
                                   break;
                              case ChannalizerType.Fast:
                                   visualElement.Add(new Label($"Damage: {TowerStats.fastChannalizerStats[this.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.fastChannalizerStats[this.rarity].fireRate}"));
                                   break;
                              case ChannalizerType.Strong:
                                   visualElement.Add(new Label($"Damage: {TowerStats.strongChannalizerStats[this.rarity].damage}"));
                                   visualElement.Add(new Label($"Fire Rate: {TowerStats.strongChannalizerStats[this.rarity].fireRate}"));
                                   break;
                         }
                         break;
               }
          }
     }

}

/// <summary>
/// An Enum representig the different types of parts
/// </summary>
public enum PartType
{
     Source,
     Structure,
     Channalizer,
}
/// <summary>
/// An Enum representing the different rarities of parts
/// </summary>
public enum Rarity
{
     Common,
     Normal,
     Rare,
     UltraRare,
     Myth
}
/// <summary>
/// An Enum representing the especific types within the source type
/// </summary>
public enum SourceType
{
     Water,
     Fire,
     Earth,
     Thunder
}
/// <summary>
/// An Enum representing the especific types within the structure type
/// </summary>
public enum StructureType
{
     Circular,
     Beam,
     Cross
}
/// <summary>
/// An Enum representing the especific types within the channalizer type
/// </summary>
public enum ChannalizerType
{
     Fast,
     Strong,
     Area
}