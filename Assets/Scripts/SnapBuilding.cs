using System.Collections;
using UnityEngine;

public class SnapBuilding : Building {

    /// <summary>
    /// Update method for placing a building
    /// </summary>
    public override IEnumerator UpdatePlacing()
    {
        while (parentManager == null)
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
            Quaternion newRotation = Quaternion.identity;

            // Does the ray intersect any objects which are in the player layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, parentManager.terrainMask))
            {
                newPosition = hit.point;
            }

            int snappedToIndex = -1;
            if (intersectedResources.Count > 0)
            {
                float minDistance = -1;
                for(int i = 0; i < intersectedResources.Count; i++)
                {
                    float newDist = Vector3.Distance(intersectedResources[i].transform.position, newPosition);
                    if(newDist < minDistance || minDistance < 0)
                    {
                        snappedToIndex = i;
                        minDistance = newDist;
                    }
                }

                if (minDistance < 5)
                {
                    newPosition = intersectedResources[snappedToIndex].transform.position;
                    newRotation = intersectedResources[snappedToIndex].transform.rotation;
                }
            }

            transform.position = newPosition;
            transform.rotation = newRotation;

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
                    if (intersectedObjects.Count < 1 && intersectedResources.Count > 0)
                    {
                        if (snappedToIndex != -1)
                        {
                            intersectedResources[snappedToIndex].SetActive(false);
                        }
                        // Place our building
                        parentManager.EndPlacing(true);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if(1 << col.gameObject.layer == parentManager.resourceMask)
        {
            intersectedResources.Add(col.gameObject);
        }
        else if (1 << col.gameObject.layer != parentManager.terrainMask)
        {
            intersectedObjects.Add(col.gameObject);
        }
    }

    protected override void OnTriggerExit(Collider col)
    {
        if (intersectedResources.Contains(col.gameObject))
        {
            intersectedResources.Remove(col.gameObject);
        }
        if (intersectedObjects.Contains(col.gameObject))
        {
            intersectedObjects.Remove(col.gameObject);
        }
    }
}
