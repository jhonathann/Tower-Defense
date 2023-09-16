using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the collection of parts in the game.
/// </summary>
[CreateAssetMenu]
public class PartsManager : ScriptableObject
{
    /// <summary>
    /// Event triggered when new parts are added.
    /// </summary>
    public static Action<List<Part>> NewPartsAdded;

    /// <summary>
    /// List of parts currently managed by the PartsManager.
    /// </summary>
    public List<Part> parts = new();

    /// <summary>
    /// Called when the PartsManager scriptable object is enabled.
    /// Subscribes to necessary events.
    /// </summary>
    void OnEnable()
    {
        GameData.GameStarted += OnGameStarted;
        PortalController.NextWave += OnNextWave;
    }

    /// <summary>
    /// Event handler for the GameStarted event.
    /// Clears the list of parts and adds starting parts.
    /// </summary>
    private void OnGameStarted()
    {
        parts.Clear();
        AddStartingParts();
    }

    /// <summary>
    /// Adds starting parts to the parts list.
    /// </summary>
    private void AddStartingParts()
    {
        AddNewPart(new Part(PartType.Source));
        AddNewPart(new Part(PartType.Channeler));
        AddNewPart(new Part(PartType.Structure));
        AddNewPart(new Part(PartType.Source));
        AddNewPart(new Part(PartType.Channeler));
        AddNewPart(new Part(PartType.Structure));
    }

    /// <summary>
    /// Adds a new part to the parts list.
    /// </summary>
    /// <param name="part">Optional part to be added. If null, a new part is created.</param>
    /// <returns>The added part.</returns>
    private Part AddNewPart(Part part = null)
    {
        Part partToBeAdded = part ?? new Part();
        parts.Add(partToBeAdded);
        HUDController.RenderPanel?.Invoke();
        return partToBeAdded;
    }

    /// <summary>
    /// Event handler for the NextWave event.
    /// Adds a random number of new parts when a new wave starts.
    /// </summary>
    private void OnNextWave()
    {
        List<Part> addedParts = new();
        // Gives between 1 and 2 new parts each time a wave starts
        int randomNumberOfParts = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < randomNumberOfParts; i++)
        {
            addedParts.Add(AddNewPart());
        }
        // Triggers the new parts added event
        NewPartsAdded?.Invoke(addedParts);
    }
}
