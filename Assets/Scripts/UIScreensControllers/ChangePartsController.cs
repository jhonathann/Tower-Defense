using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Handles the changeParts screen
/// </summary>
public class ChangePartsController : MonoBehaviour
{
    /// <summary>
    /// Action triggered when a part change button is pressed
    /// </summary>
    public static Action<Part, TowerController> PartsChanged;

    public GameData gameData;

    //References to the UI elements
    private UIDocument changeParts;
    private VisualElement globalContainer;
    private VisualElement contextMenu;
    private bool mouseIsOverTheElement;
    private TowerController invokerTower;

    private void OnEnable()
    {
        GetElements();
        HidePanel(); //Starts with the panel off
        PartsChanged = OnPartsChanged;
        PartsManager.ChangePart += OnPartsChangeTriggered;
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        PartsManager.ChangePart -= OnPartsChangeTriggered;
    }

    private void GetElements()
    {
        //Get the UI component (the element that has the script attached)
        changeParts = GetComponent<UIDocument>();
        //Get the VisualElements
        globalContainer = changeParts.rootVisualElement.Query<VisualElement>("GlobalContainer");
        contextMenu = changeParts.rootVisualElement.Query<VisualElement>("ChangePartsContextMenu");
    }

    /// <summary>
    /// Registers mouse events (used to determine if the mouse was clicked while inside the Context Menu)
    /// Had to be done this way because the global panel has visibility: hidden, so it can't catch events
    /// </summary>
    private void RegisterCallbacks()
    {
        contextMenu.RegisterCallback<MouseEnterEvent>(ContextMenuOnEnter);
        contextMenu.RegisterCallback<MouseLeaveEvent>(ContextMenuOnLeave);
        contextMenu.RegisterCallback<GeometryChangedEvent>(ContextMenuGeometryChanged);

        void ContextMenuOnEnter(MouseEnterEvent evt)
        {
            mouseIsOverTheElement = true;
        }

        void ContextMenuOnLeave(MouseLeaveEvent evt)
        {
            mouseIsOverTheElement = false;
        }

        //Used to actualize the position (need to be done in this event to assure that the layout finish updating)
        void ContextMenuGeometryChanged(GeometryChangedEvent evt)
        {
            if (globalContainer.style.display == DisplayStyle.None) return;
            //If the click was made on the right side of the screen, display the panel on the left
            float xOffset = (Input.mousePosition.x >= Screen.width / 2.0) ? -contextMenu.worldBound.width : 0;
            //If the click was made on the top side of the screen, display the panel on the bottom
            float yOffset = (Input.mousePosition.y >= Screen.height / 2.0) ? -contextMenu.worldBound.height : 0;
            //Places the menu
            contextMenu.style.left = (Input.mousePosition.x / GetCanvasScaleFactor()) + xOffset;
            contextMenu.style.bottom = (Input.mousePosition.y / GetCanvasScaleFactor()) + yOffset;
        }
    }

    /// <summary>
    /// Method Called when a part change button is clicked, displays and populates the context menu
    /// </summary>
    /// <param name="partToBeChanged">The partToBeChanged that is going to be changed</param>
    /// <param name="invoker">The towerController where there is going to be a partToBeChanged change</param>
    private void OnPartsChanged(Part partToBeChanged, TowerController invoker)
    {
        invokerTower = invoker;
        ShowPanel();
        contextMenu.Clear();
        switch (partToBeChanged.type)
        {
            case PartType.Channeler:
                List<Part> channelerParts =
                    gameData.partsManager.parts.Where(part => part.type == PartType.Channeler).ToList();
                foreach (Part channelerPart in channelerParts)
                {
                    contextMenu.Add(
                        PartVisualElementFactory.CreateVisualElementForChangePartPanel(channelerPart, partToBeChanged));
                }

                break;
            case PartType.Source:
                List<Part> sourceParts =
                    gameData.partsManager.parts.Where(part => part.type == PartType.Source).ToList();
                foreach (Part sourcePart in sourceParts)
                {
                    contextMenu.Add(
                        PartVisualElementFactory.CreateVisualElementForChangePartPanel(sourcePart, partToBeChanged));
                }

                break;
            case PartType.Structure:
                List<Part> structureParts =
                    gameData.partsManager.parts.Where(part => part.type == PartType.Structure).ToList();
                foreach (Part structurePart in structureParts)
                {
                    contextMenu.Add(
                        PartVisualElementFactory.CreateVisualElementForChangePartPanel(structurePart, partToBeChanged));
                }

                break;
        }
    }

    private void OnPartsChangeTriggered(Part newPart, Part partToBeChanged)
    {
        HidePanel();
        invokerTower.PartChanged?.Invoke(newPart);
    }

    /// <summary>
    /// Calculates the current canvas scale factor to account for then trying to absolute position an element
    /// </summary>
    /// <returns>The canvas scale factor</returns>
    private float GetCanvasScaleFactor()
    {
        PanelSettings panelSettings = changeParts.panelSettings;
        return ((float)Screen.width / panelSettings.referenceResolution.x) * (1 - panelSettings.match) +
               ((float)Screen.height / panelSettings.referenceResolution.y) * (panelSettings.match);
    }

    private void Update()
    {
        if (globalContainer.style.display == DisplayStyle.None) return;
        // ReSharper disable once InvertIf
        if (Input.GetMouseButtonDown(0))
        {
            if (!mouseIsOverTheElement) HidePanel();
        }
    }

    private void HidePanel()
    {
        globalContainer.style.display = DisplayStyle.None;
        Time.timeScale = 1;
    }

    private void ShowPanel()
    {
        globalContainer.style.display = DisplayStyle.Flex;
        Time.timeScale = 0;
    }
}