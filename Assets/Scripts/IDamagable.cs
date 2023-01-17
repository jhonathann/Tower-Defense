using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Interface that must be implemented by objects that can be damaged
/// </summary>
public interface IDamagable
{
     void TakeDamage(GameObject damager, float damageAmount = 1, Func<EnemyController, IEnumerator> Effect = null);
}
