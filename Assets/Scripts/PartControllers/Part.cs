using System;

/// <summary>
/// Part Class for the parts that the enemies drop and are used to build the towers
/// </summary>
public class Part
{
    /// <summary>
    /// The type of the part
    /// </summary>
    public readonly PartType type;

    /// <summary>
    /// The rarity of the part;
    /// </summary>
    public readonly Rarity rarity;

    /// <summary>
    /// Specific Info about the effect of the part
    /// </summary>
    public readonly Enum specificTypeInfo;

    /// <summary>
    /// This constructor initializes a new part with random (but weighted) rarity and random values
    /// </summary>
    public Part()
    {
        this.type = UtilityEnum.GetRandomTypeFromAnEnum<PartType>();
        this.rarity = GetRarity();
        this.specificTypeInfo = GetSpecificTypeInfo();
    }

    /// <summary>
    /// This constructor returns a random part of the specific type (use to ensure that the player has sufficient parts to create the initial tower)
    /// </summary>
    /// <param name="type">The type of the resulting part</param>
    public Part(PartType type)
    {
        this.type = type;
        this.rarity = GetRarity();
        this.specificTypeInfo = GetSpecificTypeInfo();
    }

    /// <summary>
    /// This constructor returns a random part of the specific rarity (use for part mixture)
    /// </summary>
    /// <param name="rarity">The rarity of the resulting part</param>
    public Part(Rarity rarity)
    {
        this.type = UtilityEnum.GetRandomTypeFromAnEnum<PartType>();
        this.rarity = rarity;
        this.specificTypeInfo = GetSpecificTypeInfo();
    }

    /// <summary>
    /// Gets the rarity of the part by considered the probability of each option
    /// </summary>
    /// <returns>The randomly obtained rarity</returns>
    private Rarity GetRarity()
    {
        int chance = UnityEngine.Random.Range(0, 100);
        return chance switch
        {
            < 40 => Rarity.Common //40% chance of Common
            ,
            < 70 => Rarity.Normal // 30% chance of Normal
            ,
            < 85 => Rarity.Rare //15% chance of Rare
            ,
            < 95 => Rarity.UltraRare // 10% chance of UltraRare
            ,
            < 100 => Rarity.Myth //5% chance of Myth
            ,
            _ => Rarity.Common
        };
    }

    /// <summary>
    /// Gets a random specific type enum based on the (previously defined) type of the part
    /// </summary>
    /// <returns>An Enum with one of the three specific types</returns>
    private Enum GetSpecificTypeInfo()
    {
        return this.type switch
        {
            PartType.Source => UtilityEnum.GetRandomTypeFromAnEnum<Element>(),
            PartType.Structure => UtilityEnum.GetRandomTypeFromAnEnum<StructureType>(),
            PartType.Channeler => UtilityEnum.GetRandomTypeFromAnEnum<ChannelerType>(),
            _ => null,
        };
    }
}

/// <summary>
/// An Enum representing the different types of parts
/// </summary>
public enum PartType
{
    Source,
    Structure,
    Channeler,
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
/// An Enum representing the specific types within the source type
/// </summary>
public enum Element
{
    Water,
    Fire,
    Earth,
    Thunder
}

/// <summary>
/// An Enum representing the specific types within the structure type
/// </summary>
public enum StructureType
{
    Circular,
    Beam,
    Cross
}

/// <summary>
/// An Enum representing the specific types within the channeler type
/// </summary>
public enum ChannelerType
{
    Fast,
    Strong,
    Area
}