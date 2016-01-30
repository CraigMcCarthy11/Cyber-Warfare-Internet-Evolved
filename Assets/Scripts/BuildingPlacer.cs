using UnityEngine;
using System.Collections;
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
public class BuildingPlacer : MonoBehaviour {

    public EventSystem evtSystem;
    public RectTransform selectionRect;
    public List<PlaceableStructure> placeableStructures = new List<PlaceableStructure>();
    private GameObject currentItem;
    private LayerMask mask; 

	// Use this for variable initialization
	void Start ()
    {
        // Get the correct layer for colliding with
        mask = LayerMask.NameToLayer("Terrain");

        // Spawn all the building buttons
        for (int i = 0; i < placeableStructures.Count; i++)
        {
            int indx = i;
            GameObject btnObj = Instantiate(placeableStructures[i].buttonPrefab) as GameObject;
            btnObj.transform.SetParent(selectionRect);

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
        currentItem = Instantiate(placeableStructures[index].structurePrefab) as GameObject;
        currentItem.transform.position = new Vector3(0, 100, 0);
        currentItem.transform.rotation = Quaternion.identity;

        // Begin dragging around the item
        StartCoroutine(UpdatePlacing());
    }

    /// <summary>
    /// Update method for placing a building
    /// </summary>
    IEnumerator UpdatePlacing()
    {
        // Always true loop, break to escape
        while (true)
        {
            // Dont bother continuing if current item is null;
            if (currentItem == null)
            {
                break;
            }
            
            // If we hit our ray on the layer, update the position of our building.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1 << mask.value;

            // Does the ray intersect any objects which are in the player layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                currentItem.transform.position = hit.point;
            }

            // If we click down...
            if(Input.GetMouseButtonDown(0))
            {
                // When over any form of UI
                if(evtSystem.IsPointerOverGameObject())
                {
                    // Disable placing
                    EndPlacing(false);
                    break;
                }
                else
                {
                    // Place our building
                    EndPlacing(true);
                    break;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// Unlinks our current object and either places or destroys it
    /// </summary>
    /// <param name="placeAtLocation"> determines if we cancel or finish placement </param>
    void EndPlacing(bool placeAtLocation)
    {
        // If we are not placing...
        if (!placeAtLocation)
        {
            // Blow that shit UP!!!
            Destroy(currentItem);
        }
        else
        {
            // Enable the collider for normal function
            currentItem.GetComponent<BoxCollider>().enabled = true;
        } 
        
        currentItem = null; // Stop holding the item
    }
}
