using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls the Tower World Space UI
/// </summary>
public class TowerUIController : MonoBehaviour
{
    public GameData gameData;
    private TowerController towerController;
    private Part channeler;
    private Part structure;
    private Part source;
    private Canvas canvas;
    private Image channelerBackground;
    private Image channelerIcon;
    private Image structureBackground;
    private Image structureIcon;
    private Image sourceBackground;
    private Image sourceIcon;
    private Button moveTower;
    private Button channelerButton;
    private Button structureButton;
    private Button sourceButton;

    private void Start()
    {
        //Gets the reference to the tower
        towerController = GetComponentInParent<TowerController>();
        gameData = towerController.gameData;
        //Gets the parts references
        channeler = towerController.channeler;
        structure = towerController.structure;
        source = towerController.source;
        //Set the main camera to the canvas (to be able to use events)
        canvas = GetComponentInParent<Canvas>();
        canvas.worldCamera = Camera.main;
        SetImageReferences(this.GetComponentsInChildren<Image>());
        SetButtonReferences(this.GetComponentsInChildren<Button>());
        SetImages();
        SetOnClickFunctions();
    }
    /// <summary>
    /// Updates the panel with the latest parts (used when a part changed)
    /// </summary>
    public void Redraw()
    {
        channeler = towerController.channeler;
        structure = towerController.structure;
        source = towerController.source;
        SetImages();
    }

    /// <summary>
    /// Sets the appropriate image reference (necessary cause the GetComponentsInChildren function does not guarantee an order)
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
    private void SetImages()
    {
        channelerBackground.sprite = gameData.towerUiData.rarityToSprite[channeler.rarity];
        channelerIcon.sprite = gameData.towerUiData.specificTypeToSprite[channeler.specificTypeInfo];
        structureBackground.sprite = gameData.towerUiData.rarityToSprite[structure.rarity];
        structureIcon.sprite = gameData.towerUiData.specificTypeToSprite[structure.specificTypeInfo];
        sourceBackground.sprite = gameData.towerUiData.rarityToSprite[source.rarity];
        sourceIcon.sprite = gameData.towerUiData.specificTypeToSprite[source.specificTypeInfo];
    }

    private void SetButtonReferences(Button[] buttonReferences)
    {
        foreach (Button button in buttonReferences)
        {
            switch (button.name)
            {
                case ("MoveTower"):
                    moveTower = button;
                    break;
                case ("Channeler"):
                    channelerButton = button;
                    break;
                case ("Structure"):
                    structureButton = button;
                    break;
                case ("Source"):
                    sourceButton = button;
                    break;
            }
        }
    }

    private void SetOnClickFunctions()
    {
        //Add Listeners
        moveTower.onClick.AddListener(MoveTowerOnClick);
        channelerButton.onClick.AddListener(ChannelerOnClick);
        structureButton.onClick.AddListener(StructureOnClick);
        sourceButton.onClick.AddListener(SourceOnClick);
        return;

        void MoveTowerOnClick()
        {
            if (PortalController.WaveState is WaveState.Running)
            {
                GameData.DisplayInformation("Towers can only be moved in between waves", 2);
                return;
            }
            towerController.State = TowerState.Unplaced;
            towerController.TowerBeingRelocated?.Invoke();
            //Set the tower to be placed
            gameData.towerData.tower = towerController.gameObject;
            //Notify that there is a tower to be placed
            gameData.isTowerReady = true;
            //Change game state
            gameData.gameState.TrySetState(GameStateType.PlacingTower);
        }

        void ChannelerOnClick()
        {
            ChangePartsController.PartsChanged?.Invoke(channeler, towerController);
        }

        void StructureOnClick()
        {
            ChangePartsController.PartsChanged?.Invoke(structure, towerController);
        }

        void SourceOnClick()
        {
            ChangePartsController.PartsChanged?.Invoke(source, towerController);
        }
    }

    private void Update()
    {
        RotateWithCameraInYAxis();
    }

    /// <summary>
    /// Rotates the tower in the Y axis to always look at the camera
    /// </summary>
    private void RotateWithCameraInYAxis()
    {
        if (Camera.main is not null)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}