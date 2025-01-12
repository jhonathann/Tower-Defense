using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class handles the behaviour of the towers
/// </summary>
public class TowerController : MonoBehaviour
{
    //References to the other controllers
    private TowerSelectionHandler towerSelectionHandler;
    private TowerAttackHandler towerAttackHandler;
    private TowerRotationHandler towerRotationHandler;
    public TowerState State { get; set; } = TowerState.Unplaced;

    /// <summary>
    /// Reference to the gameData
    /// </summary>
    public GameData gameData;

    //References to the parts of the tower
    public Part channeler;
    public Part structure;
    public Part source;

    // ReSharper disable once InconsistentNaming
    public Action TowerBeingRelocated;

    // ReSharper disable once InconsistentNaming
    public Action<Part> PartChanged;

    //References to the structure colliders
    private readonly List<Collider> hitZoneColliders = new();

    //Reference to the selectionCollider
    private Collider selectionCollider;

    //Reference to the GameObjectModels
    private GameObject channelerModel;
    private GameObject structureModel;
    private GameObject sourceModel;

    /// <summary>
    /// reference to the hitZoneGameObjects that show the range of the tower
    /// </summary>
    private readonly List<GameObject> hitZoneGameObjects = new();

    /// <summary>
    /// Name of the shader that the meshes of the HitZones are going to use
    /// </summary>
    private const string SHADER_HIT_ZONE_NAME = "Shader Graphs/Blinking";

    private void Awake()
    {
        // Adds the components to the tower gameObject
        towerSelectionHandler = this.gameObject.AddComponent<TowerSelectionHandler>();
        towerAttackHandler = this.gameObject.AddComponent<TowerAttackHandler>();
        towerRotationHandler = this.gameObject.AddComponent<TowerRotationHandler>();
    }

    private void Start()
    {
        //Set the gameObject mask so it doesn't trigger the onMouse events (used while placing the tower so the colliders don't overlap)
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        StoreParts();
        InstantiateModels();
        SetStructureStats();
        towerAttackHandler.Setup(this.channeler, this.source, this.gameData);
        selectionCollider = AddSelectionCollider();
        towerSelectionHandler.Setup(selectionCollider, hitZoneGameObjects, gameData.towerUiData.GetWorldUIPrefab());
        PartChanged = OnPartChange;
    }

    /// <summary>
    /// Stores the references to the parts of the tower
    /// </summary>
    private void StoreParts()
    {
        this.channeler = gameData.towerData.channeler;
        this.structure = gameData.towerData.structure;
        this.source = gameData.towerData.source;
    }

    /// <summary>
    /// Instantiate the models of the parts
    /// </summary>
    private void InstantiateModels()
    {
        channelerModel = Instantiate(gameData.towerData.GetChannelerPrefab(channeler),
            this.transform.position + Vector3.up * TowerStats.CHANNELER_HEIGHT, Quaternion.identity, this.transform);
        structureModel = Instantiate(gameData.towerData.GetStructurePrefab(structure),
            this.transform.position + Vector3.up * TowerStats.STRUCTURE_HEIGHT, Quaternion.identity, this.transform);
        sourceModel = Instantiate(gameData.towerData.GetSourcePrefab(source),
            this.transform.position + Vector3.up * TowerStats.SOURCE_HEIGHT, Quaternion.identity, this.transform);
    }

    private Collider AddSelectionCollider()
    {
        //Add the collider component
        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        //Sets the is trigger so it fires OnTrigger Events
        boxCollider.isTrigger = true;
        //Properly adjust the center of the collider
        boxCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
        //Properly adjust the size of the collider
        boxCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
        return boxCollider;
    }

    /// <summary>
    /// Creates the hitZones of the tower based on the structureType
    /// </summary>
    private void SetStructureStats()
    {
        //Clears both lists before starting (important when changing parts)
        hitZoneColliders.Clear();
        hitZoneGameObjects.Clear();
        switch (structure.specificTypeInfo)
        {
            case StructureType.Beam:
                CreateBeamHitZoneCollider();
                CreateBeamHitZoneMesh();
                break;
            case StructureType.Circular:
                CreateCircleHitZoneCollider();
                CreateCircleHitZoneMesh();
                break;
            case StructureType.Cross:
                CreateCrossHitZoneCollider();
                CreateCrossHitZoneMesh();
                break;
        }

        return;

        void CreateBeamHitZoneCollider()
        {
            //Add the collider component
            BoxCollider beamCollider = this.gameObject.AddComponent<BoxCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            beamCollider.isTrigger = true;
            //Properly adjust the center of the collider
            beamCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2,
                TowerStats.beamStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2);
            //Properly adjust the size of the collider
            beamCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.beamStructureStats[structure.rarity]);
            //Adds the collider to the references list
            hitZoneColliders.Add(beamCollider);
        }

        void CreateCircleHitZoneCollider()
        {
            //Add the collider component
            SphereCollider circularCollider = this.gameObject.AddComponent<SphereCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            circularCollider.isTrigger = true;
            //Properly adjust the center of the collider
            circularCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT, 0);
            //Properly adjust the radius of the collider
            circularCollider.radius = TowerStats.circularStructureStats[structure.rarity];
            //Adds the collider to the references list
            hitZoneColliders.Add(circularCollider);
        }

        void CreateCrossHitZoneCollider()
        {
            //Add Collider at the front
            //Add the collider component
            BoxCollider frontCrossCollider = this.gameObject.AddComponent<BoxCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            frontCrossCollider.isTrigger = true;
            //Properly adjust the center of the collider
            frontCrossCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2,
                TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2);
            //Properly adjust the size of the collider
            frontCrossCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.crossStructureStats[structure.rarity]);
            //Adds the collider to the references list
            hitZoneColliders.Add(frontCrossCollider);
            //Add Collider at the back
            //Add the collider component
            BoxCollider backCrossCollider = this.gameObject.AddComponent<BoxCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            backCrossCollider.isTrigger = true;
            //Properly adjust the center of the collider
            backCrossCollider.center = new Vector3(0, TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2,
                -1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TowerStats.TOWER_WIDTH / 2);
            //Properly adjust the size of the collider
            backCrossCollider.size = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.crossStructureStats[structure.rarity]);
            //Adds the collider to the references list
            hitZoneColliders.Add(backCrossCollider);
            //Add collider to the right
            //Add the collider component
            BoxCollider rightCrossCollider = this.gameObject.AddComponent<BoxCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            rightCrossCollider.isTrigger = true;
            //Properly adjust the center of the collider
            rightCrossCollider.center =
                new Vector3(TowerStats.crossStructureStats[structure.rarity] / 2 + TowerStats.TOWER_WIDTH / 2,
                    TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
            //Properly adjust the size of the collider
            rightCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity],
                TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
            //Adds the collider to the references list
            hitZoneColliders.Add(rightCrossCollider);
            //Add collider to the left
            //Add the collider component
            BoxCollider leftCrossCollider = this.gameObject.AddComponent<BoxCollider>();
            //Sets the is trigger so it fires OnTrigger Events
            leftCrossCollider.isTrigger = true;
            //Properly adjust the center of the collider
            leftCrossCollider.center =
                new Vector3(-1 * TowerStats.crossStructureStats[structure.rarity] / 2 - TowerStats.TOWER_WIDTH / 2,
                    TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2, 0);
            //Properly adjust the size of the collider
            leftCrossCollider.size = new Vector3(TowerStats.crossStructureStats[structure.rarity],
                TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
            //Adds the collider to the references list
            hitZoneColliders.Add(leftCrossCollider);
        }

        void CreateBeamHitZoneMesh()
        {
            //Creates the primitive
            GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //Sets the primitive as a child of the tower
            hitZone.transform.SetParent(this.transform);
            //Destroy the primitive's collider (created by default)
            Destroy(hitZone.GetComponent<Collider>());
            //Properly adjust the position
            hitZone.transform.position = this.transform.position +
                                         Vector3.forward * (TowerStats.beamStructureStats[structure.rarity] / 2 +
                                                            TowerStats.TOWER_WIDTH / 2) +
                                         Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
            //Properly adjust the scale
            hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.beamStructureStats[structure.rarity]);
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            //Adds the gameObject to the hitZoneGameObjects list so it can be accessed
            hitZoneGameObjects.Add(hitZone);
        }

        void CreateCircleHitZoneMesh()
        {
            GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hitZone.transform.SetParent(this.transform);
            Destroy(hitZone.GetComponent<Collider>());
            hitZone.transform.position = this.transform.position + Vector3.up * TowerStats.STRUCTURE_HEIGHT;
            hitZone.transform.localScale = Vector3.one * TowerStats.circularStructureStats[structure.rarity] * 2;
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            hitZoneGameObjects.Add(hitZone);
        }

        void CreateCrossHitZoneMesh()
        {
            //Add front zone
            GameObject hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hitZone.transform.SetParent(this.transform);
            Destroy(hitZone.GetComponent<Collider>());
            hitZone.transform.position = this.transform.position +
                                         Vector3.forward * (TowerStats.crossStructureStats[structure.rarity] / 2 +
                                                            TowerStats.TOWER_WIDTH / 2) +
                                         Vector3.up * (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
            hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.crossStructureStats[structure.rarity]);
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            hitZoneGameObjects.Add(hitZone);
            //Add back zone
            hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hitZone.transform.SetParent(this.transform);
            Destroy(hitZone.GetComponent<Collider>());
            hitZone.transform.position = this.transform.position +
                                         Vector3.back * (TowerStats.crossStructureStats[structure.rarity] / 2 +
                                                         TowerStats.TOWER_WIDTH / 2) + Vector3.up *
                                         (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
            hitZone.transform.localScale = new Vector3(TowerStats.TOWER_WIDTH, TowerStats.TOWER_HEIGHT,
                TowerStats.crossStructureStats[structure.rarity]);
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            hitZoneGameObjects.Add(hitZone);
            //Add right zone
            hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hitZone.transform.SetParent(this.transform);
            Destroy(hitZone.GetComponent<Collider>());
            hitZone.transform.position = this.transform.position +
                                         Vector3.right * (TowerStats.crossStructureStats[structure.rarity] / 2 +
                                                          TowerStats.TOWER_WIDTH / 2) + Vector3.up *
                                         (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
            hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity],
                TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            hitZoneGameObjects.Add(hitZone);
            //Add left zone
            hitZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hitZone.transform.SetParent(this.transform);
            Destroy(hitZone.GetComponent<Collider>());
            hitZone.transform.position = this.transform.position +
                                         Vector3.left * (TowerStats.crossStructureStats[structure.rarity] / 2 +
                                                         TowerStats.TOWER_WIDTH / 2) + Vector3.up *
                                         (TowerStats.STRUCTURE_HEIGHT + TowerStats.TOWER_HEIGHT / 2);
            hitZone.transform.localScale = new Vector3(TowerStats.crossStructureStats[structure.rarity],
                TowerStats.TOWER_HEIGHT, TowerStats.TOWER_WIDTH);
            //Sets the shader to the material
            hitZone.GetComponent<MeshRenderer>().material.shader = Shader.Find(SHADER_HIT_ZONE_NAME);
            hitZoneGameObjects.Add(hitZone);
        }
    }
    /// <summary>
    /// Method that handles the part change depending on the part that changed
    /// </summary>
    /// <param name="newPart">the new part</param>
    private void OnPartChange(Part newPart)
    {
        switch (newPart.type)
        {
            case PartType.Channeler:
                //Updates the reference to the part
                this.channeler = newPart;
                //Destroys the current instantiatedMode
                Destroy(channelerModel);
                //Instantiates the new model
                channelerModel = Instantiate(gameData.towerData.GetChannelerPrefab(channeler),
                    this.transform.position + Vector3.up * TowerStats.CHANNELER_HEIGHT, Quaternion.identity,
                    this.transform);
                //Updates the attacker values
                towerAttackHandler.Setup(this.channeler, this.source, this.gameData);
                //Updates the worldSpaceUI
                towerSelectionHandler.Setup(selectionCollider, hitZoneGameObjects,
                    gameData.towerUiData.GetWorldUIPrefab());
                break;
            case PartType.Structure:
                //Updates the reference to the part
                this.structure = newPart;
                //Destroys the current instantiatedModel
                Destroy(structureModel);
                //Instantiates the new model
                structureModel = Instantiate(gameData.towerData.GetStructurePrefab(structure),
                    this.transform.position + Vector3.up * TowerStats.STRUCTURE_HEIGHT, Quaternion.identity,
                    this.transform);
                //Destroys the previous colliders
                foreach (Collider hitZoneCollider in hitZoneColliders)
                {
                    Destroy(hitZoneCollider);
                }
                //Destroys the previous hitZone Models
                foreach (GameObject hitZone in hitZoneGameObjects)
                {
                    Destroy(hitZone);
                }
                //Sets the structure stats again (regenerates the adequate colliders and models)
                SetStructureStats();
                //Updates the worldSpaceUI
                towerSelectionHandler.Setup(selectionCollider, hitZoneGameObjects,
                    gameData.towerUiData.GetWorldUIPrefab());
                break;
            case PartType.Source:
                //Updates the reference to the part
                this.source = newPart;
                //Destroys the current instantiatedModel
                Destroy(sourceModel);
                //Instantiates the new model
                sourceModel = Instantiate(gameData.towerData.GetSourcePrefab(source),
                    this.transform.position + Vector3.up * TowerStats.SOURCE_HEIGHT, Quaternion.identity,
                    this.transform);
                //Updates the attacker values
                towerAttackHandler.Setup(this.channeler, this.source, this.gameData);
                //Updates the worldSpaceUI
                towerSelectionHandler.Setup(selectionCollider, hitZoneGameObjects,
                    gameData.towerUiData.GetWorldUIPrefab());
                break;
        }
    }

    private void Update()
    {
        if (State == TowerState.Unplaced)
        {
            towerRotationHandler.RotateTower?.Invoke();
        }

        if (State == TowerState.Placed)
        {
            towerAttackHandler.CheckForAttack?.Invoke();
        }
    }
}

/// <summary>
/// Enum of the TowerStates
/// </summary>
public enum TowerState
{
    Unplaced,
    Placed
}