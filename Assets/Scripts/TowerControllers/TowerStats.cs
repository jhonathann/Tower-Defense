using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that acts as a data container and handels all the values regarding the tower stats
/// </summary>
public class TowerStats
{
     public static float CHANNELER_HEIGHT = 18.0f;
     public static float SOURCE_HEIGHT = 8.0f;
     public static float STRUCTURE_HEIGHT = 1.0f;
     public static float TOWER_HEIGHT = 20.0f;
     public static float TOWER_WIDTH = 10.0f;

     private const float CHANNELER_AREA_BASE_DAMAGE = 10.0f;
     private const float CHANNELER_AREA_BASE_FIRE_RATE = 0.125f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> areaChannelerStats = new()
     {
          { Rarity.Common, (damage: 1.0f*CHANNELER_AREA_BASE_DAMAGE, fireRate: 1.0f*CHANNELER_AREA_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNELER_AREA_BASE_DAMAGE, fireRate: 1.25f*CHANNELER_AREA_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNELER_AREA_BASE_DAMAGE, fireRate: 1.5f*CHANNELER_AREA_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNELER_AREA_BASE_DAMAGE, fireRate: 1.75f*CHANNELER_AREA_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNELER_AREA_BASE_DAMAGE, fireRate: 2.0f*CHANNELER_AREA_BASE_FIRE_RATE) }
          };
     private const float CHANNELER_FAST_BASE_DAMAGE = 20.0f;
     private const float CHANNELER_FAST_BASE_FIRE_RATE = 1.0f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> fastChannelerStats = new()
     {
          { Rarity.Common, (damage: 1.0f*CHANNELER_FAST_BASE_DAMAGE, fireRate: 1.0f*CHANNELER_FAST_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNELER_FAST_BASE_DAMAGE, fireRate: 1.25f*CHANNELER_FAST_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNELER_FAST_BASE_DAMAGE, fireRate: 1.5f*CHANNELER_FAST_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNELER_FAST_BASE_DAMAGE, fireRate: 1.75f*CHANNELER_FAST_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNELER_FAST_BASE_DAMAGE, fireRate: 2.0f*CHANNELER_FAST_BASE_FIRE_RATE) }
          };
     private const float CHANNELER_STRONG_BASE_DAMAGE = 40.0f;
     private const float CHANNELER_STRONG_BASE_FIRE_RATE = 0.5f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> strongChannelerStats = new() {
          { Rarity.Common, (damage: 1.0f*CHANNELER_STRONG_BASE_DAMAGE, fireRate: 1.0f*CHANNELER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNELER_STRONG_BASE_DAMAGE, fireRate: 1.25f*CHANNELER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNELER_STRONG_BASE_DAMAGE, fireRate: 1.5f*CHANNELER_STRONG_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNELER_STRONG_BASE_DAMAGE, fireRate: 1.75f*CHANNELER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNELER_STRONG_BASE_DAMAGE, fireRate: 2.0f*CHANNELER_STRONG_BASE_FIRE_RATE) }
          };
     private const float STRUCTURE_BEAM_BASE_RANGE = 40.0f;
     public static readonly Dictionary<Rarity, float> beamStructureStats = new() {
          { Rarity.Common, 1.0f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Normal,  1.25f *STRUCTURE_BEAM_BASE_RANGE},
          { Rarity.Rare, 1.5f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_BEAM_BASE_RANGE}
          };
     private const float STRUCTURE_CIRCULAR_BASE_RADIUS = 15.0f;
     public static readonly Dictionary<Rarity, float> circularStructureStats = new(){
          { Rarity.Common, 1.0f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Normal,  1.25f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Rare, 1.5f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Myth, 2.0f*STRUCTURE_CIRCULAR_BASE_RADIUS}
          };
     private const float STRUCTURE_CROSS_BASE_RANGE = 20.0f;
     public static readonly Dictionary<Rarity, float> crossStructureStats = new() {
          { Rarity.Common, 1.0f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Normal,  1.25f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Rare, 1.5f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_CROSS_BASE_RANGE}
          };
     private const float EARTH_BASE_TIME = 0.5f;
     private static Func<EnemyController, IEnumerator> EarthEffectSetUp(float time)
     {
          return EarthEffect;
          IEnumerator EarthEffect(EnemyController enemy)
          {
               if (enemy.OnUpdate != null)
               {
                    enemy.OnUpdate = null;
               }
               yield return new WaitForSeconds(time);
               if (enemy.OnUpdate == null)
               {
                    enemy.OnUpdate = enemy.TravelPath;
               }
          }
     }
     public static readonly Dictionary<Rarity, float> earthSourceTimes = new()
     {
          { Rarity.Common, EARTH_BASE_TIME * 1.0f },
          { Rarity.Normal, EARTH_BASE_TIME * 2.0f },
          { Rarity.Rare, EARTH_BASE_TIME * 3.0f },
          { Rarity.UltraRare, EARTH_BASE_TIME * 4.0f },
          { Rarity.Myth, EARTH_BASE_TIME * 5.0f },
      };
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> earthSourceStats = new()
     {
          { Rarity.Common, EarthEffectSetUp(earthSourceTimes[Rarity.Common]) },
          { Rarity.Normal,  EarthEffectSetUp(earthSourceTimes[Rarity.Normal]) },
          { Rarity.Rare, EarthEffectSetUp(earthSourceTimes[Rarity.Rare]) },
          { Rarity.UltraRare, EarthEffectSetUp(earthSourceTimes[Rarity.UltraRare]) },
          { Rarity.Myth, EarthEffectSetUp(earthSourceTimes[Rarity.Myth])}
          };

     private const float FIRE_BASE_TIME = 0.5f;
     public const float FIRE_BURN_DAMAGE = 5.0f;
     public const float FIRE_BURN_INTERVAL_TIME = 0.25f;
     private static Func<EnemyController, IEnumerator> FireEffectSetUp(float time)
     {
          return FireEffect;
          IEnumerator FireEffect(EnemyController enemy)
          {
               float internalTime = time; //Needed because if the time were to be altered directly the next coroutines will be afected as well. 
               IDamageable enemyIDamageable = enemy;
               while (internalTime > 0)
               {
                    enemyIDamageable.TakeDamage(FIRE_BURN_DAMAGE);
                    internalTime -= FIRE_BURN_INTERVAL_TIME;
                    yield return new WaitForSeconds(FIRE_BURN_INTERVAL_TIME);
               }
          }
     }
     public static readonly Dictionary<Rarity, float> fireSourceTimes = new()
     {
          { Rarity.Common, FIRE_BASE_TIME * 1.0f },
          { Rarity.Normal, FIRE_BASE_TIME * 2.0f },
          { Rarity.Rare, FIRE_BASE_TIME * 3.0f },
          { Rarity.UltraRare, FIRE_BASE_TIME * 4.0f },
          { Rarity.Myth, FIRE_BASE_TIME * 5.0f },
      };
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> fireSourceStats = new()
     {
          { Rarity.Common, FireEffectSetUp(fireSourceTimes[Rarity.Common]) },
          { Rarity.Normal,  FireEffectSetUp(fireSourceTimes[Rarity.Normal]) },
          { Rarity.Rare, FireEffectSetUp(fireSourceTimes[Rarity.Rare]) },
          { Rarity.UltraRare, FireEffectSetUp(fireSourceTimes[Rarity.UltraRare]) },
          { Rarity.Myth, FireEffectSetUp(fireSourceTimes[Rarity.Myth])}
          };

     private const float THUNDER_BASE_TIME = 0.25f;
     private static Func<EnemyController, IEnumerator> ThunderEffectSetUp(float time)
     {
          return ThunderEffect;
          IEnumerator ThunderEffect(EnemyController enemy)
          {
               if (enemy.OnUpdate != enemy.TravelPathBackwards)
               {
                    enemy.goalCheckPoint++;
                    enemy.OnUpdate = enemy.TravelPathBackwards;
               }
               yield return new WaitForSeconds(time);
               if (enemy.OnUpdate != enemy.TravelPath)
               {
                    enemy.goalCheckPoint--;
                    enemy.OnUpdate = enemy.TravelPath;
               }
          }
     }
     public static readonly Dictionary<Rarity, float> thunderSourceTimes = new()
     {
          { Rarity.Common, THUNDER_BASE_TIME * 1.0f },
          { Rarity.Normal, THUNDER_BASE_TIME * 2.0f },
          { Rarity.Rare, THUNDER_BASE_TIME * 3.0f },
          { Rarity.UltraRare, THUNDER_BASE_TIME * 4.0f },
          { Rarity.Myth, THUNDER_BASE_TIME * 5.0f },
      };
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> thunderSourceStats = new()
     {
          { Rarity.Common, ThunderEffectSetUp(thunderSourceTimes[Rarity.Common]) },
          { Rarity.Normal,  ThunderEffectSetUp(thunderSourceTimes[Rarity.Normal]) },
          { Rarity.Rare, ThunderEffectSetUp(thunderSourceTimes[Rarity.Rare]) },
          { Rarity.UltraRare, ThunderEffectSetUp(thunderSourceTimes[Rarity.UltraRare]) },
          { Rarity.Myth, ThunderEffectSetUp(thunderSourceTimes[Rarity.Myth])}
          };

     private const float WATER_BASE_TIME = 1.0f;
     private static Func<EnemyController, IEnumerator> WaterEffectSetUp(float time)
     {
          return WaterEffect;
          IEnumerator WaterEffect(EnemyController enemy)
          {
               if (enemy.speed == EnemyStats.GetSpeed(enemy.type))
               {
                    enemy.speed /= 2;
               }
               yield return new WaitForSeconds(time);
               if (enemy.speed != EnemyStats.GetSpeed(enemy.type))
               {
                    enemy.speed *= 2;
               }
          }
     }
     public static readonly Dictionary<Rarity, float> waterSourceTimes = new()
     {
          { Rarity.Common, WATER_BASE_TIME * 1.0f },
          { Rarity.Normal, WATER_BASE_TIME * 2.0f },
          { Rarity.Rare, WATER_BASE_TIME * 3.0f },
          { Rarity.UltraRare, WATER_BASE_TIME * 4.0f },
          { Rarity.Myth, WATER_BASE_TIME * 5.0f },
      };
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> waterSourceStats = new()
     {
          { Rarity.Common, WaterEffectSetUp(waterSourceTimes[Rarity.Common]) },
          { Rarity.Normal,  WaterEffectSetUp(waterSourceTimes[Rarity.Normal]) },
          { Rarity.Rare, WaterEffectSetUp(waterSourceTimes[Rarity.Rare]) },
          { Rarity.UltraRare, WaterEffectSetUp(waterSourceTimes[Rarity.UltraRare]) },
          { Rarity.Myth, WaterEffectSetUp(waterSourceTimes[Rarity.Myth])}
          };
}
