using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    /// Action called when a part is clicked
    /// </summary>
    public static Action<Part> SelectPart;
    /// <summary>
    /// List of parts currently managed by the PartsManager.
    /// </summary>
    public List<Part> parts = new();
    /// <summary>
    /// Reference to the creation panel state
    /// </summary>
    /// <value></value>
    private CreationPanelState CreationPanelState { get; set; }
    //Slots for the three selected parts for the tower creation
    public Part channelerSelectedPart;
    public Part structureSelectedPart;
    public Part sourceSelectedPart;
    //Array of parts that are going to be mixed
    public List<Part> partsToMix = new();
    /// <summary>
    /// Called when the PartsManager scriptable object is enabled.
    /// Subscribes to necessary events.
    /// </summary>
    void OnEnable()
    {
        SelectPart += OnSelectPart;
        GameData.GameStarted += OnGameStarted;
        PortalController.NextWave += OnNextWave;
        HUDController.ChangeOfHUDState += OnChangeOfHUDState;
        HUDController.MixPartsTriggered += OnMixParts;
        HUDController.CreateTowerTriggered += OnCreateTower;
    }

    /// <summary>
    /// Event handler for the GameStarted event.
    /// Clears the list of parts and adds starting parts.
    /// </summary>
    private void OnGameStarted()
    {
        //Clears all the parts
        parts.Clear();
        partsToMix.Clear();
        channelerSelectedPart = null;
        structureSelectedPart = null;
        sourceSelectedPart = null;

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
        parts.Sort(ComparePartsByRarity);
        HUDController.RenderPanel?.Invoke();
        return partToBeAdded;

        //Comparison delegate used to compare the parts by their rarity
        int ComparePartsByRarity(Part part1, Part part2)
        {
            if (part1.rarity > part2.rarity) return 1;
            if (part1.rarity < part2.rarity) return -1;
            return 0;
        }
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
    /// <summary>
    /// Sets the new selected part to the corresponding field and changes their classes accordingly
    /// </summary>
    /// <param name="newSelectedPart">Part that was selected</param>
    void OnSelectPart(Part newSelectedPart)
    {
        if (CreationPanelState == CreationPanelState.TowerCreation)
        {
            TowerCreationPartSelected(newSelectedPart);
        }
        if (CreationPanelState == CreationPanelState.MixParts)
        {
            MixPartsPartSelected(newSelectedPart);
        }

        void TowerCreationPartSelected(Part newSelectedPart)
        {
            switch (newSelectedPart.type)
            {
                case PartType.Channeler:
                    if (IsPartAlreadySelected(newSelectedPart, channelerSelectedPart))
                    {
                        newSelectedPart.RemoveFromClassList("selected");
                        channelerSelectedPart = null;
                    }
                    else
                    {
                        channelerSelectedPart?.RemoveFromClassList("selected");
                        newSelectedPart.AddToClassList("selected");
                        channelerSelectedPart = newSelectedPart;
                    }
                    break;
                case PartType.Structure:
                    if (IsPartAlreadySelected(newSelectedPart, structureSelectedPart))
                    {
                        newSelectedPart.RemoveFromClassList("selected");
                        structureSelectedPart = null;
                        return;
                    }
                    else
                    {
                        structureSelectedPart?.RemoveFromClassList("selected");
                        newSelectedPart.AddToClassList("selected");
                        structureSelectedPart = newSelectedPart;
                    }
                    break;
                case PartType.Source:
                    if (IsPartAlreadySelected(newSelectedPart, sourceSelectedPart))
                    {
                        newSelectedPart.RemoveFromClassList("selected");
                        sourceSelectedPart = null;
                        return;
                    }
                    else
                    {
                        sourceSelectedPart?.RemoveFromClassList("selected");
                        newSelectedPart.AddToClassList("selected");
                        sourceSelectedPart = newSelectedPart;
                    }
                    break;
            }
            bool IsPartAlreadySelected(Part newSelectedPart, Part currentlySelectedPart)
            {
                return newSelectedPart == currentlySelectedPart;
            }
        }

        void MixPartsPartSelected(Part newSelectedPart)
        {
            //If the part is already selected then it is deselected
            if (partsToMix.Contains(newSelectedPart))
            {
                newSelectedPart.RemoveFromClassList("selected");
                partsToMix.Remove(newSelectedPart);
            }
            else
            {
                //If there are already 3 selected parts
                if (partsToMix.Count == 3) return;

                newSelectedPart.AddToClassList("selected");
                partsToMix.Add(newSelectedPart);
            }
        }
    }
    void OnCreateTower()
    {
        //Remove the parts from the part List
        parts.Remove(channelerSelectedPart);
        parts.Remove(structureSelectedPart);
        parts.Remove(sourceSelectedPart);
    }
    void OnChangeOfHUDState(CreationPanelState state)
    {
        //Sets the state
        CreationPanelState = state;
        //Clears Selected Parts
        channelerSelectedPart = null;
        structureSelectedPart = null;
        sourceSelectedPart = null;
        partsToMix.Clear();
        foreach (Part part in parts)
        {
            part.RemoveFromClassList("selected");
        }
    }
    /// <summary>
    /// Activated when the user clicks the mix parts button
    /// </summary>
    public void OnMixParts()
    {
        //When there aren't enough parts
        if (partsToMix.Count < 3)
        {
            GameData.DisplayInformation?.Invoke("Three parts are needed to be mixed", 2);
            return;
        }
        //Calculates the average of the rarity of the parts to mix (the rarity is an enum after all)
        float rarityAverage = 0;
        foreach (Part part in partsToMix)
        {
            rarityAverage += (int)part.rarity;
        }
        rarityAverage /= 3;
        //Adds one more to that rarity (the part that results should be of a higher rarity)
        float baseMixScore = 1 + rarityAverage;
        //Gets the decimal Part of this computation
        float extraProbability = baseMixScore % 1;
        int newPartRarityInt = Mathf.RoundToInt(baseMixScore);
        //The decimal part is used as a probability to get even a higher rarity
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        if (roll < extraProbability)
        {
            newPartRarityInt += 1;
        }
        //Clamps to the higher rarity
        newPartRarityInt = Mathf.Clamp(newPartRarityInt, 0, 4);
        //Cast again to rarity
        Rarity newPartRarity = (Rarity)newPartRarityInt;
        //Remove of the partsToMix from the general parts
        foreach (Part part in partsToMix)
        {
            parts.Remove(part);
        }
        //Adds the new part with the calculated rarity
        Part newPartAdded = AddNewPart(new Part(newPartRarity));
        //Show the newly added part
        NewPartsAdded?.Invoke(new List<Part>() { newPartAdded });
        //Clears the partsToMix list
        partsToMix.Clear();
        //Calls a rerender of the panel
        HUDController.RenderPanel?.Invoke();
    }
}

