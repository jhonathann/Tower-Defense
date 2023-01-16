using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerStats
{
     public static readonly float CHANNALIZER_AREA_BASE_DAMAGE = 0.5f;
     public static readonly float CHANNALIZER_AREA_BASE_FIRE_RATE = 0.25f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> areaChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_AREA_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_AREA_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_AREA_BASE_FIRE_RATE) }
          };
     public static readonly float CHANNALIZER_FAST_BASE_DAMAGE = 1.0f;
     public static readonly float CHANNALIZER_FAST_BASE_FIRE_RATE = 2.0f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> fastChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_FAST_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_FAST_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_FAST_BASE_FIRE_RATE) }
          };
     public static readonly float CHANNALIZER_STRONG_BASE_DAMAGE = 2.0f;
     public static readonly float CHANNALIZER_STRONG_BASE_FIRE_RATE = 1.0f;
     public static readonly Dictionary<Rarity, (float damage, float fireRate)> strongChannalizerStats = new Dictionary<Rarity, (float damage, float fireRate)> {
          { Rarity.Common, (damage: 1.0f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.0f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Normal, (damage: 1.25f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.25f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Rare, (damage: 1.5f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.5f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.UltraRare, (damage: 1.75f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 1.75f*CHANNALIZER_STRONG_BASE_FIRE_RATE) },
          { Rarity.Myth, (damage: 2.0f*CHANNALIZER_STRONG_BASE_DAMAGE, fireRate: 2.0f*CHANNALIZER_STRONG_BASE_FIRE_RATE) }
          };
     public static readonly float STRUCTURE_BEAM_BASE_RANGE = 40.0f;
     public static readonly Dictionary<Rarity, float> beamStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Normal,  1.25f *STRUCTURE_BEAM_BASE_RANGE},
          { Rarity.Rare, 1.5f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_BEAM_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_BEAM_BASE_RANGE}
          };
     public static readonly float STRUCTURE_CIRCULAR_BASE_RADIUS = 15.0f;
     public static readonly Dictionary<Rarity, float> circularStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Normal,  1.25f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Rare, 1.5f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CIRCULAR_BASE_RADIUS },
          { Rarity.Myth, 2.0f*STRUCTURE_CIRCULAR_BASE_RADIUS}
          };
     public static readonly float STRUCTURE_CROSS_BASE_RANGE = 20.0f;
     public static readonly Dictionary<Rarity, float> crossStructureStats = new Dictionary<Rarity, float> {
          { Rarity.Common, 1.0f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Normal,  1.25f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Rare, 1.5f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.UltraRare, 1.75f*STRUCTURE_CROSS_BASE_RANGE },
          { Rarity.Myth, 2.0f*STRUCTURE_CROSS_BASE_RANGE}
          };
     public static Func<EnemyController, IEnumerator> EarthEffectSetUp(float time)
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
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> earthSourceStats = new Dictionary<Rarity, Func<EnemyController, IEnumerator>> {
          { Rarity.Common, EarthEffectSetUp(0.1f) },
          { Rarity.Normal,  EarthEffectSetUp(0.2f) },
          { Rarity.Rare, EarthEffectSetUp(0.3f) },
          { Rarity.UltraRare, EarthEffectSetUp(0.4f) },
          { Rarity.Myth, EarthEffectSetUp(0.5f)}
          };

     public static Func<EnemyController, IEnumerator> FireEffectSetUp(float time)
     {
          return FireEffect;
          IEnumerator FireEffect(EnemyController enemy)
          {
               float internalTime = time; //Needed because if the time were to be altered directly the next coroutines will be afected as well. 
               IDamagable enemyIDamagable = (IDamagable)enemy;
               while (internalTime > 0)
               {
                    enemyIDamagable.TakeDamage(0.2f);
                    internalTime -= 0.1f;
                    yield return new WaitForSeconds(0.1f);
               }
          }
     }
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> fireSourceStats = new Dictionary<Rarity, Func<EnemyController, IEnumerator>> {
          { Rarity.Common, FireEffectSetUp(0.2f) },
          { Rarity.Normal,  FireEffectSetUp(0.4f) },
          { Rarity.Rare, FireEffectSetUp(0.6f) },
          { Rarity.UltraRare, FireEffectSetUp(0.8f) },
          { Rarity.Myth, FireEffectSetUp(1.0f)}
          };
     public static Func<EnemyController, IEnumerator> ThunderEffectSetUp(float time)
     {
          return ThunderEffect;
          IEnumerator ThunderEffect(EnemyController enemy)
          {
               if (enemy.OnUpdate != enemy.TravelPathBackwards)
               {
                    enemy.goalCheckPoint++;
               }
               enemy.OnUpdate = enemy.TravelPathBackwards;
               yield return new WaitForSeconds(time);
               enemy.OnUpdate = enemy.TravelPath;
          }
     }
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> thunderSourceStats = new Dictionary<Rarity, Func<EnemyController, IEnumerator>> {
          { Rarity.Common, ThunderEffectSetUp(0.05f) },
          { Rarity.Normal,  ThunderEffectSetUp(0.10f) },
          { Rarity.Rare, ThunderEffectSetUp(0.15f) },
          { Rarity.UltraRare, ThunderEffectSetUp(0.20f) },
          { Rarity.Myth, ThunderEffectSetUp(0.25f)}
          };
     public static Func<EnemyController, IEnumerator> WaterEffectSetUp(float time)
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
     public static readonly Dictionary<Rarity, Func<EnemyController, IEnumerator>> waterSourceStats = new Dictionary<Rarity, Func<EnemyController, IEnumerator>> {
          { Rarity.Common, WaterEffectSetUp(0.2f) },
          { Rarity.Normal,  WaterEffectSetUp(0.4f) },
          { Rarity.Rare, WaterEffectSetUp(0.6f) },
          { Rarity.UltraRare, WaterEffectSetUp(0.8f) },
          { Rarity.Myth, WaterEffectSetUp(1.0f)}
          };
}
