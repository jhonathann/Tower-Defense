using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Scripts that controlls the informationScreen
/// </summary>
public class InformationScreenController : MonoBehaviour
{
     UIDocument informationScreen;
     VisualElement displayContainer;
     Label informationText;
     // Start is called before the first frame update
     void OnEnable()
     {
          GetElements();
     }
     /// <summary>
     /// Gets the UI elements from the panel 
     /// </summary>
     private void GetElements()
     {
          //Get the UI component (the element that has the script attached)
          informationScreen = GetComponent<UIDocument>();
          //.Query search for especific elements in the visual tree (short .Q) in this case by the name of the element
          displayContainer = informationScreen.rootVisualElement.Query<VisualElement>("DisplayContainer");
          //For the element to ignore events? Unsure
          displayContainer.pickingMode = PickingMode.Ignore;
          informationText = informationScreen.rootVisualElement.Query<Label>("InformationText");
     }
     public void SetUp(string text, float destroyTime)
     {
          informationText.text = text;
          Destroy(this.gameObject, destroyTime);
     }
}
