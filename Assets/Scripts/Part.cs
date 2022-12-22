using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Part Class for the parts that the enemies drop and are used to build the towers
/// </summary>
public class Part
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
     /// This constructor initialized a new part with random (but weighted) rarity and random values
     /// </summary>
     public Part()
     {
          this.type = GetRandomTypeFromAnEnum<PartType>();
          this.rarity = GetRarity();
          this.specificTypeInfo = GetSpecificTypeInfo();
     }

     /// <summary>
     /// Gets the rarity of the part by considered the probability of each option
     /// </summary>
     /// <returns>The randomly obtained rarity</returns>
     public Rarity GetRarity()
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
                    return GetRandomTypeFromAnEnum<SourceType>();
               case PartType.Structure:
                    return GetRandomTypeFromAnEnum<StructureType>();
               case PartType.Channalizer:
                    return GetRandomTypeFromAnEnum<ChannalizerType>();
               default: return null;
          }
     }

     /// <summary>
     /// Helper Function that returns a random value from an specific enum type
     /// </summary>
     /// <typeparam name="T">Type of Enum to work with</typeparam>
     /// <returns>The generated enum value</returns>
     private T GetRandomTypeFromAnEnum<T>()
     {
          Array values = System.Enum.GetValues(typeof(T));
          T randomValue = (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
          return randomValue;
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
     Wind
}
/// <summary>
/// An Enum representing the especific types within the structure type
/// </summary>
public enum StructureType
{
     Low,
     Medium,
     High
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