using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Interface that must be implemented by objects that can be damaged
/// </summary>
public interface IDamageable
{
     void TakeDamage(float damageAmount = 1, Func<EnemyController, IEnumerator> Effect = null, Element element = Element.Water);
}
