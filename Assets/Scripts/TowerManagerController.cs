using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerManagerController : MonoBehaviour
{
     UIDocument TowerManager;
     [SerializeField]
     VisualTreeAsset Card;

     void Start()
     {
          TowerManager = GetComponent<UIDocument>();
          TowerManager.rootVisualElement.Add(new Button());
          TowerManager.visualTreeAsset.Instantiate();
     }

     void Update()
     {

     }
}
