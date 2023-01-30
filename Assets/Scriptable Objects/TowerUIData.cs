using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Scriptable object that holds all the sprites to create the tower's world UI and the dictionaries to access them
/// </summary>
[CreateAssetMenu]
public class TowerUIData : ScriptableObject
{
     //References to be populated in the inspector
     [SerializeField]
     private GameObject worldUIPrefab;
     [SerializeField]
     private Sprite mythBackground;
     [SerializeField]
     private Sprite ultraRareBackground;
     [SerializeField]
     private Sprite rareBackground;
     [SerializeField]
     private Sprite normalBackground;
     [SerializeField]
     private Sprite commonBackground;
     [SerializeField]
     private Sprite channelerStrongIcon;
     [SerializeField]
     private Sprite channelerFastIcon;
     [SerializeField]
     private Sprite channelerAreaIcon;
     [SerializeField]
     private Sprite structureBeamIcon;
     [SerializeField]
     private Sprite structureCircularIcon;
     [SerializeField]
     private Sprite structureCrossIcon;
     [SerializeField]
     private Sprite sourceEarthIcon;
     [SerializeField]
     private Sprite sourceFireIcon;
     [SerializeField]
     private Sprite sourceWaterIcon;
     [SerializeField]
     private Sprite sourceThunderIcon;

     /// <summary>
     /// Dictionary that associates a rarity to its correspondent sprite
     /// </summary>
     public Dictionary<Rarity, Sprite> rarityToSprite;

     /// <summary>
     /// Dictionary that associates a specificType to its correspondent sprite
     /// </summary>
     public Dictionary<Enum, Sprite> specificTypeToSprite;

     void OnEnable()
     {
          rarityToSprite = new Dictionary<Rarity, Sprite> {
            {Rarity.Common,commonBackground},
            {Rarity.Normal,normalBackground},
            {Rarity.Rare,rareBackground},
            {Rarity.UltraRare,ultraRareBackground},
            {Rarity.Myth,mythBackground}
        };
          specificTypeToSprite = new Dictionary<Enum, Sprite>
          {
            {ChannelerType.Area,channelerAreaIcon},
            {ChannelerType.Fast,channelerFastIcon},
            {ChannelerType.Strong,channelerStrongIcon},
            {StructureType.Beam,structureBeamIcon},
            {StructureType.Circular,structureCircularIcon},
            {StructureType.Cross,structureCrossIcon},
            {SourceType.Earth,sourceEarthIcon},
            {SourceType.Fire,sourceFireIcon},
            {SourceType.Thunder,sourceThunderIcon},
            {SourceType.Water,sourceWaterIcon}
          };
     }

     public GameObject GetWorldUIPrefab()
     {
          return this.worldUIPrefab;
     }
}
