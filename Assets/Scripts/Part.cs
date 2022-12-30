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
     public VisualTreeAsset template = Resources.Load<VisualTreeAsset>("Part");
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
          this.RegisterCallback<ClickEvent>(PartOnClick);
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
          this.RegisterCallback<ClickEvent>(PartOnClick);
     }
     /// <summary>
     /// Gets the rarity of the part by considered the probability of each option
     /// </summary>
     /// <returns>The randomly obtained rarity</returns>
     private Rarity GetRarity()
     {
          int chance = UnityEngine.Random.Range(1, 100);
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
     void PartOnClick(ClickEvent evt)
     {
          GameData.SelectPart(this);
          HUDController.RenderPanel?.Invoke();
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