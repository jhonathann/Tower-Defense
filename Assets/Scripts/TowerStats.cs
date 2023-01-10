using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats
{
     public static readonly float CHANNALIZER_AREA_BASE_DAMAGE = 0.5f;
     public static readonly float CHANNALIZER_AREA_BASE_FIRE_RATE = 2f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> areaChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_AREA_BASE_FIRE_RATE) }
          };
     public static readonly float CHANNALIZER_FAST_BASE_DAMAGE = 1.0f;
     public static readonly float CHANNALIZER_FAST_BASE_FIRE_RATE = 1.0f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> fastChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_FAST_BASE_FIRE_RATE) }
          };
     public static readonly float CHANNALIZER_STRONG_BASE_DAMAGE = 2.0f;
     public static readonly float CHANNALIZER_STRONG_BASE_FIRE_RATE = 0.5f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> strongChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_STRONG_BASE_FIRE_RATE) }
          };
     public static readonly float STRUCTURE_BEAM_BASE_RANGE = 30.0f;
     public static readonly Dictionary<Rarity, float> beamStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Normal,  1.25f *STRUCTURE_BEAM_BASE_RANGE},
          { Rarity.Rare, 1.5f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_BEAM_BASE_RANGE}
          };
     public static readonly float STRUCTURE_CIRCULAR_BASE_RADIUS = 20.0f;
     public static readonly Dictionary<Rarity, float> circularStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Normal,  1.25f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Rare, 1.5f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Myth, 2.0f*STRUCTURE_CIRCULAR_BASE_RADIUS}
          };
     public static readonly float STRUCTURE_CROSS_BASE_RANGE = 15.0f;
     public static readonly Dictionary<Rarity, float> crossStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Normal,  1.25f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Rare, 1.5f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_CROSS_BASE_RANGE}
          };
     public static readonly Dictionary<Rarity, float> earthSourceStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f },
          { Rarity.Normal,  1.25f },
          { Rarity.Rare, 1.5f },
          { Rarity.UltraRare, 1.75f },
          { Rarity.Myth, 2.0f}
          };
     public static readonly Dictionary<Rarity, float> fireSourceStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f },
          { Rarity.Normal,  1.25f },
          { Rarity.Rare, 1.5f },
          { Rarity.UltraRare, 1.75f },
          { Rarity.Myth, 2.0f}
          };
     public static readonly Dictionary<Rarity, float> thunderSourceStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f },
          { Rarity.Normal,  1.25f },
          { Rarity.Rare, 1.5f },
          { Rarity.UltraRare, 1.75f },
          { Rarity.Myth, 2.0f}
          };
     public static readonly Dictionary<Rarity, float> waterSourceStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f },
          { Rarity.Normal,  1.25f },
          { Rarity.Rare, 1.5f },
          { Rarity.UltraRare, 1.75f },
          { Rarity.Myth, 2.0f}
          };
}
