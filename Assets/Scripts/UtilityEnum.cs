using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class with some Utility functions for working with enums
/// </summary>
public static class UtilityEnum
{
     /// <summary>
     /// Helper Function that returns a random value from an specific enum type
     /// </summary>
     /// <typeparam name="T">Type of Enum to work with</typeparam>
     /// <returns>The generated enum value</returns>
     public static T GetRandomTypeFromAnEnum<T>()
     {
          Array values = System.Enum.GetValues(typeof(T));
          T randomValue = (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
          return randomValue;
     }
}
