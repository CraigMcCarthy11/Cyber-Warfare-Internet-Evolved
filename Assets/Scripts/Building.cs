using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    public BuildingManager parentManager;
    public List<GameObject> intersectedObjects = new List<GameObject>();
    public List<GameObject> intersectedResources = new List<GameObject>();

    /// <summary>
    /// Update method for placing a building
    /// </summary>
    public virtual IEnumerator UpdatePlacing()
    {
        while(parentManager == null)
        {
            yield return null;
        }

        // Always true loop, break to escape
        while (true)
        {
            // If we hit our ray on the layer, update the position of our building.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 newPosition = transform.position;
            
            // Does the ray intersect any objects which are in the player layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, parentManager.terrainMask))
            {
                newPosition = hit.point;
            }

            transform.position = newPosition;
            
            // If we click down...
            if (Input.GetMouseButtonDown(0))
            {
                // When over any form of UI
                if (parentManager.evtSystem.IsPointerOverGameObject())
                {
                    // Disable placing
                    parentManager.EndPlacing(false);
                    break;
                }
                else
                {
                    if (intersectedObjects.Count < 1)
                    {
                        // Place our building
                        parentManager.EndPlacing(true);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (1 << col.gameObject.layer != parentManager.terrainMask)
        {
            intersectedObjects.Add(col.gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (intersectedObjects.Contains(col.gameObject))
        {
            intersectedObjects.Remove(col.gameObject);
        }
    }
}
