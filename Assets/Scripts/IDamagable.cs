using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that must be implemented by objects that can be damaged
/// </summary>
public interface IDamagable
{
     void TakeDamage(float damageAmount);
}
