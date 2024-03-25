using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Holds the information that will be used to create the enemies
/// </summary>
public class WaveMember
{
    public float DistanceReached { get; set; } = Mathf.Infinity; //Starts at infinity cause the goal is at distance 0
    public float Health { get; private set; } = 50;
    public float Speed { get; private set; } = 10;
    public Element Element { get; private set; } = UtilityEnum.GetRandomTypeFromAnEnum<Element>();
    public Action OnDestroyCallback;
    public WaveMember(Action OnDestroyCallback)
    {
        this.OnDestroyCallback = OnDestroyCallback;
    }
    /// <summary>
    /// Improves either the health, the speed or changes the element
    /// </summary>
    public void Mutate()
    {
        int roll = UnityEngine.Random.Range(0, 100);
        if (roll < 33) this.Health *= 1.2f;
        if (roll < 66) this.Speed *= 1.1f;
        if (roll < 100) this.Element = UtilityEnum.GetRandomTypeFromAnEnum<Element>();
    }
    /// <summary>
    /// Changes the stats based on other member stats
    /// </summary>
    /// <param name="parent">The member to inherit from</param>
    public void Inherit(WaveMember parent)
    {
        this.Health = parent.Health;
        this.Speed = parent.Speed;
        this.Element = parent.Element;
    }
}
