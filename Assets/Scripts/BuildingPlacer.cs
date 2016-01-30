using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[Serializable]
public struct PlaceableStructure
{
    public GameObject buttonPrefab;
    public GameObject structurePrefab;
}

public class BuildingPlacer : MonoBehaviour {

    public EventSystem evtSystem;
    public RectTransform selectionRect;
    public List<PlaceableStructure> placeableStructures = new List<PlaceableStructure>();
    public GameObject currentItem;
    private LayerMask mask; 

	// Use this for variable initialization
	void Start () {
        for(int i = 0; i < placeableStructures.Count; i++)
        {
            int indx = i;
            GameObject btnObj = Instantiate(placeableStructures[i].buttonPrefab) as GameObject;
            btnObj.transform.SetParent(selectionRect);

            btnObj.GetComponent<Button>().onClick.AddListener(() => { BeginPlacing(indx); });
        }
        
        mask = LayerMask.NameToLayer("Terrain");
    }

    public void BeginPlacing(int index)
    {
        currentItem = Instantiate(placeableStructures[index].structurePrefab) as GameObject;
        currentItem.transform.localScale = Vector3.one;
        currentItem.transform.position = new Vector3(0, 100, 0);
        currentItem.transform.rotation = Quaternion.identity;
        Debug.Log("Placing begin");
        StartCoroutine(UpdatePlacing());
    }

    IEnumerator UpdatePlacing()
    {
        while(true)
        {
            Debug.Log("Placing");
            // Dont bother continuing if current item is null;
            if(currentItem == null)
                break;
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, mask))
            {
                currentItem.transform.position = hit.point;
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(evtSystem.IsPointerOverGameObject())
                {
                    Debug.Log("Failed");
                    EndPlacing(false);
                    break;
                }
                else
                {
                    Debug.Log("Success");
                    EndPlacing(true);
                    break;
                }
            }
            yield return null;
        }
    }

    void EndPlacing(bool placeAtLocation)
    {
        Debug.Log("End");
        if (!placeAtLocation)
        {
            Destroy(currentItem);
        }
        else
        {
            currentItem.GetComponent<BoxCollider>().enabled = true;
        }
        currentItem = null;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
