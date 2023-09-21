using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIController : MonoBehaviour
{
     public GameData gameData;
     private Part channeler;
     private Part structure;
     private Part source;
     private Image channelerBackground;
     private Image channelerIcon;
     private Image structureBackground;
     private Image structureIcon;
     private Image sourceBackground;
     private Image sourceIcon;
     void Start()
     {
          TowerController towerController = this.GetComponentInParent<TowerController>();
          this.gameData = towerController.gameData;
          this.channeler = towerController.channeler;
          this.structure = towerController.structure;
          this.source = towerController.source;
          SetImageReferences(this.GetComponentsInChildren<Image>());
          SetImages();
     }

     /// <summary>
     /// Sets the aproppiate image reference (necesary cause the GetComponentsInChildren function does not guarantees an order)
     /// </summary>
     /// <param name="imageReferences">the array of imageReferences</param>
     private void SetImageReferences(Image[] imageReferences)
     {
          foreach (Image image in imageReferences)
          {
               switch (image.name)
               {
                    case "Channeler":
                         this.channelerBackground = image;
                         break;
                    case "ChannelerIcon":
                         this.channelerIcon = image;
                         break;
                    case "Structure":
                         this.structureBackground = image;
                         break;
                    case "StructureIcon":
                         this.structureIcon = image;
                         break;
                    case "Source":
                         this.sourceBackground = image;
                         break;
                    case "SourceIcon":
                         this.sourceIcon = image;
                         break;
               }
          }
     }

     /// <summary>
     /// Sets each image's sprite according to the tower's parts
     /// </summary>
     void SetImages()
     {
          channelerBackground.sprite = gameData.towerUiData.rarityToSprite[channeler.rarity];
          channelerIcon.sprite = gameData.towerUiData.specificTypeToSprite[channeler.specificTypeInfo];
          structureBackground.sprite = gameData.towerUiData.rarityToSprite[structure.rarity];
          structureIcon.sprite = gameData.towerUiData.specificTypeToSprite[structure.specificTypeInfo];
          sourceBackground.sprite = gameData.towerUiData.rarityToSprite[source.rarity];
          sourceIcon.sprite = gameData.towerUiData.specificTypeToSprite[source.specificTypeInfo];
     }
     void Update()
     {
          RotateWithCameraInYAxis();
     }

     /// <summary>
     /// Rotates the tower in the Y axis to always look at the camera
     /// </summary>
     private void RotateWithCameraInYAxis()
     {
          transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
          ;
          return;
     }
}
