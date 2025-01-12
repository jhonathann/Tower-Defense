using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

/// <summary>
/// Creates VisualElements out of parts (used in the different UI screens)
/// </summary>
public static class PartVisualElementFactory
{
    /// <summary>
    /// Template of the part (used to import the classes to be used)
    /// </summary>
    private static readonly VisualTreeAsset Template = Resources.Load<VisualTreeAsset>("Part");

    /// <summary>
    /// Dictionary that matches each rarity with the corresponding USS class
    /// </summary>
    /// <value>A string corresponding to the USS class to be assigned</value>
    private static readonly Dictionary<Rarity, string> RarityClasses = new()
    {
        { Rarity.Common, "commonRarity" },
        { Rarity.Normal, "normalRarity" },
        { Rarity.Rare, "rareRarity" },
        { Rarity.UltraRare, "ultraRareRarity" },
        { Rarity.Myth, "mythRarity" }
    };

    /// <summary>
    /// Dictionary that matches each type with a USS class to display the icons of the elements in the UI 
    /// </summary>
    /// <value>A string corresponding to the USS class to be assigned</value>
    private static readonly Dictionary<Enum, string> SpecificTypeClasses = new()
    {
        { ChannelerType.Fast, "channelerFast" },
        { ChannelerType.Strong, "channelerStrong" },
        { ChannelerType.Area, "channelerArea" },
        { StructureType.Beam, "structureBeam" },
        { StructureType.Circular, "structureCircular" },
        { StructureType.Cross, "structureCross" },
        { Element.Earth, "sourceEarth" },
        { Element.Fire, "sourceFire" },
        { Element.Thunder, "sourceThunder" },
        { Element.Water, "sourceWater" }
    };

    /// <summary>
    /// Creates a VisualElement out of a part
    /// </summary>
    /// <param name="part">the part to be shown</param>
    /// <param name="isSelected">show the part as selected?</param>
    /// <param name="isMock">is the part a mock? (for the mock containers)</param>
    /// <returns>The VisualElement of the part</returns>
    public static VisualElement CreateVisualElement(Part part, bool isSelected = false, bool isMock = false)
    {
        if (part is null) return null;
        VisualElement partVisualElement = new();
        Template.CloneTree(partVisualElement);
        partVisualElement.AddToClassList(RarityClasses[part.rarity]);
        VisualElement iconContainer = CreateIconContainerElement(part);
        partVisualElement.Add(iconContainer);
        VisualElement contextMenu = CreateContextMenu(part);
        iconContainer.Add(contextMenu);
        SetCallBacks();
        if (isSelected) partVisualElement.AddToClassList("selected");
        if (isMock) partVisualElement.AddToClassList("partMock");
        return partVisualElement;


        //Sets the Callback functions
        void SetCallBacks()
        {
            partVisualElement.RegisterCallback<ClickEvent>(PartOnClick);
            if (isMock) return; //Do not add mouse enter/leave events if the VisualElement is a mock
            partVisualElement.RegisterCallback<MouseEnterEvent>(PartOnMouseEnter);
            partVisualElement.RegisterCallback<MouseLeaveEvent>(PartOnMouseLeave);
            return;


            // Callback triggered when the part is clicked
            void PartOnClick(ClickEvent evt)
            {
                PartsManager.SelectPart(part);
                HUDController.RenderPanel?.Invoke();
            }

            // Callback triggered when the mouse enters the part
            void PartOnMouseEnter(MouseEnterEvent evt)
            {
                //Doubles the size of the part
                partVisualElement.AddToClassList("partBigger");
                // Adds the context menu
                contextMenu.AddToClassList("contextMenu-show");
                contextMenu.RemoveFromClassList("contextMenu-hidden");
            }

            // Callback triggered when the mouse leaves the part
            void PartOnMouseLeave(MouseLeaveEvent evt)
            {
                //Sets the size of the part to normal again
                partVisualElement.RemoveFromClassList("partBigger");
                //Toggles the context menu classes
                contextMenu.RemoveFromClassList("contextMenu-show");
                contextMenu.AddToClassList("contextMenu-hidden");
            }
        }
    }

    /// <summary>
    /// Creates the visual element for the change parts context menu
    /// </summary>
    /// <param name="part">the part to be shown</param>
    /// <param name="partToBeChanged">the part that is going to be changed</param>
    /// <returns>The VisualElement of the part</returns>
    public static VisualElement CreateVisualElementForChangePartPanel(Part part, Part partToBeChanged)
    {
        if (part is null) return null;
        VisualElement partVisualElement = new();
        Template.CloneTree(partVisualElement);
        partVisualElement.AddToClassList(RarityClasses[part.rarity]);
        VisualElement iconContainer = CreateIconContainerElement(part);
        partVisualElement.Add(iconContainer);
        VisualElement contextMenu = CreateContextMenu(part);
        iconContainer.Add(contextMenu);
        SetCallBacks();
        return partVisualElement;

        void SetCallBacks()
        {
            partVisualElement.RegisterCallback<ClickEvent>(PartOnClick);
            partVisualElement.RegisterCallback<MouseEnterEvent>(PartOnMouseEnter);
            partVisualElement.RegisterCallback<MouseLeaveEvent>(PartOnMouseLeave);
            return;

            //Callback triggered when the part is clicked
            void PartOnClick(ClickEvent evt)
            {
                PartsManager.ChangePart(part, partToBeChanged);
                HUDController.RenderPanel?.Invoke();
            }

            // Callback triggered when the mouse enters the part
            void PartOnMouseEnter(MouseEnterEvent evt)
            {
                //Doubles the size of the part
                partVisualElement.AddToClassList("partBigger");
                // Adds the context menu
                contextMenu.AddToClassList("contextMenu-show");
                contextMenu.RemoveFromClassList("contextMenu-hidden");
            }

            // Callback triggered when the mouse leaves the part
            void PartOnMouseLeave(MouseLeaveEvent evt)
            {
                //Sets the size of the part to normal again
                partVisualElement.RemoveFromClassList("partBigger");
                //Toggles the context menu classes
                contextMenu.RemoveFromClassList("contextMenu-show");
                contextMenu.AddToClassList("contextMenu-hidden");
            }
        }
    }

    // Creates the IconContainer
    private static VisualElement CreateIconContainerElement(Part part)
    {
        VisualElement iconContainer = new VisualElement();
        iconContainer.AddToClassList("iconContainer");
        iconContainer.AddToClassList(SpecificTypeClasses[part.specificTypeInfo]);
        return iconContainer;
    }

    //Creates the context menu and adds the corresponding USS class
    private static VisualElement CreateContextMenu(Part part)
    {
        VisualElement contextMenu = new();
        contextMenu.AddToClassList("contextMenu-hidden");
        AddStatsLabels(contextMenu);
        return contextMenu;

        //Switches to the part characteristics and adds the correct labels to the context menu
        void AddStatsLabels(VisualElement visualElement)
        {
            switch (part.type)
            {
                case PartType.Source:
                    switch (part.specificTypeInfo)
                    {
                        case Element.Earth:
                            visualElement.Add(
                                new Label($"Effect: Stun {TowerStats.earthSourceTimes[part.rarity]} sec"));
                            break;
                        case Element.Fire:
                            visualElement.Add(
                                new Label($"Effect: Burn {TowerStats.fireSourceTimes[part.rarity]} sec"));
                            break;
                        case Element.Thunder:
                            visualElement.Add(
                                new Label($"Effect: Confusion {TowerStats.thunderSourceTimes[part.rarity]} sec"));
                            break;
                        case Element.Water:
                            visualElement.Add(
                                new Label($"Effect: Slow {TowerStats.waterSourceTimes[part.rarity]} sec"));
                            break;
                    }

                    break;
                case PartType.Structure:
                    switch (part.specificTypeInfo)
                    {
                        case StructureType.Beam:
                            visualElement.Add(
                                new Label($"Range: {TowerStats.beamStructureStats[part.rarity] / 10} Tiles"));
                            break;
                        case StructureType.Circular:
                            visualElement.Add(
                                new Label($"Radius: {TowerStats.circularStructureStats[part.rarity] / 10} Tiles"));
                            break;
                        case StructureType.Cross:
                            visualElement.Add(
                                new Label($"Range: {TowerStats.crossStructureStats[part.rarity] / 10} Tiles"));
                            break;
                    }

                    break;
                case PartType.Channeler:
                    switch (part.specificTypeInfo)
                    {
                        case ChannelerType.Area:
                            visualElement.Add(
                                new Label($"Damage: {TowerStats.areaChannelerStats[part.rarity].damage}"));
                            visualElement.Add(
                                new Label($"Fire Rate: {TowerStats.areaChannelerStats[part.rarity].fireRate}"));
                            break;
                        case ChannelerType.Fast:
                            visualElement.Add(
                                new Label($"Damage: {TowerStats.fastChannelerStats[part.rarity].damage}"));
                            visualElement.Add(
                                new Label($"Fire Rate: {TowerStats.fastChannelerStats[part.rarity].fireRate}"));
                            break;
                        case ChannelerType.Strong:
                            visualElement.Add(
                                new Label($"Damage: {TowerStats.strongChannelerStats[part.rarity].damage}"));
                            visualElement.Add(
                                new Label($"Fire Rate: {TowerStats.strongChannelerStats[part.rarity].fireRate}"));
                            break;
                    }

                    break;
            }
        }
    }
}