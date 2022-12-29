using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// ScriptableObject that contains relevant game informatcion
/// </summary>
[CreateAssetMenu]
public class GameData : ScriptableObject
{
     /// <summary>
     /// Reference to the generated path
     /// </summary>
     public List<Tile> path;
     /// <summary>
     /// The health of the player
     /// </summary>
     public int health;
     /// <summary>
     /// The list of parts that the player currently has
     /// </summary>
     /// <typeparam name="Part">The Part Class</typeparam>
     /// <returns></returns>
     public List<Part> parts = new List<Part>();
     //Slots for the three selected parts
     public Part channalizerSelectedPart;
     public Part structureSelectedPart;
     public Part sourceSelectedPart;
     /// <summary>
     /// Action called when a part is clicked
     /// </summary>
     public static Action<Part> SelectPart;
     /// <summary>
     /// Action called when a new part is added
     /// </summary>
     public static Action AddNewPart;
     void OnEnable()
     {
          health = 10;
          SelectPart += OnSelectPart;
          AddNewPart += OnAddNewPart;
          AddStartingParts(6);
     }
     /// <summary>
     /// Adds the starting parts to the gameobject
     /// </summary>
     /// <param name="numberOfParts">The number of starting parts</param>
     void AddStartingParts(int numberOfParts)
     {
          for (int i = 0; i < numberOfParts; i++)
          {
               GameData.AddNewPart?.Invoke();
          }
     }
     /// <summary>
     /// Adds a new part to the list and re-renders the HUD panel to reflect the change
     /// </summary>
     void OnAddNewPart()
     {
          parts.Add(new Part());
          HUDController.RenderPanel?.Invoke();
     }
     /// <summary>
     /// Sets the new selected part to the corresponding field and changes the uss classes of newSelect and the previously selected part
     /// </summary>
     /// <param name="newSelectedPart">Part that was selected</param>
     void OnSelectPart(Part newSelectedPart)
     {
          switch (newSelectedPart.type)
          {
               case PartType.Channalizer:
                    channalizerSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    channalizerSelectedPart = newSelectedPart;
                    break;
               case PartType.Structure:
                    structureSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    structureSelectedPart = newSelectedPart;
                    break;
               case PartType.Source:
                    sourceSelectedPart?.RemoveFromClassList("selected");
                    newSelectedPart.AddToClassList("selected");
                    sourceSelectedPart = newSelectedPart;
                    break;
          }
     }
}
