using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


[Serializable]
// Stores both the button and the object for a specific building/structure
public struct PlaceableStructure
{
    public GameObject buttonPrefab;
    public GameObject structurePrefab;
}

/// <summary>
/// Apparently places buildings
/// Spawns buttons based on the list in the inspector
/// Spawns structures when buttons are pressed
/// </summary>
public class BuildingManager : MonoBehaviour {

    public EventSystem evtSystem;
    public List<PlaceableStructure> placeableStructures = new List<PlaceableStructure>();
    private GameObject currentItem;
    public int terrainMask;
    public int resourceMask;

    public List<Building> buildings = new List<Building>();

	// Use this for variable initialization
	void Start ()
    {
        // Get the correct layer for colliding with
        terrainMask = 1 << LayerMask.NameToLayer("Terrain");
        resourceMask = 1 << LayerMask.NameToLayer("Resource");

        // Spawn all the building buttons
        for (int i = 0; i < placeableStructures.Count; i++)
        {
            int indx = i;
            GameObject btnObj = Instantiate(placeableStructures[i].buttonPrefab) as GameObject;
            btnObj.transform.SetParent(UIManager.instance.menuPanels[(int)MenuMode.BuildingSpawn]);

            // Set the delegate
            btnObj.GetComponent<Button>().onClick.AddListener(() => { BeginPlacing(indx); });
        }
    }

    /// <summary>
    /// Spawns our structure in response to a button click
    /// </summary>
    /// <param name="index"> this is the idex of the structure we will be placing </param>
    public void BeginPlacing(int index)
    {
        bool canPlace = CheckCost(placeableStructures[index].structurePrefab.GetComponent<Building>().buildingType);

        //bool canPlace = CheckCost(thisBuilding.buildingType);
        if (canPlace)
        {
            currentItem = Instantiate(placeableStructures[index].structurePrefab) as GameObject;
            currentItem.transform.position = new Vector3(0, 100, 0);
            currentItem.transform.rotation = Quaternion.identity;

            Building thisBuilding = currentItem.GetComponent<Building>();
            thisBuilding.parentManager = this;

            thisBuilding.faction = GameManager.instance.playerFaction;

            // Begin dragging around the item
            StartCoroutine(thisBuilding.UpdatePlacing());
        }
        else
        {
            //Play sound here that makes the player sad they arent rich enough
        }
    }

    /// <summary>
    /// Unlinks our current object and either places or destroys it
    /// </summary>
    /// <param name="placeAtLocation"> determines if we cancel or finish placement </param>
    public void EndPlacing(bool placeAtLocation)
    {
        // If we are not placing...
        if (!placeAtLocation)
        {
            // Blow that shit UP!!!
            DestroyImmediate(currentItem);
        }
        else
        {
            currentItem.GetComponent<BoxCollider>().isTrigger = false;
            buildings.Add(currentItem.GetComponent<Building>());
        }

        // Stop holding the item
        currentItem = null; 
    }

    /// <summary>
    /// We check the cost of the buildings here before we place it to make sure you are allowed to place them.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckCost(BuildingType type)
    {
        bool canPlace = false;
        //and its the home base
        /*if (type == EnumManager.BuildingType.HomeBase)
        {

        }*/
        if (type == BuildingType.InternetGasCollector)
        {
            if (GameManager.instance.internetGas >= 50)
            {
                canPlace = true;
                UIManager.instance.UpdateUserResource(-50f);
            }
        }
        else if (type == BuildingType.Barracks)
        {
            if (GameManager.instance.internetGas >= 200)
            {
                canPlace = true;
                UIManager.instance.UpdateUserResource(-200f);
            }
        }
        else if (type == BuildingType.Turret)
        {
            if (GameManager.instance.internetGas >= 350)
            {
                canPlace = true;
                UIManager.instance.UpdateUserResource(-350f);
            }
        }
        return canPlace;
    }
}
