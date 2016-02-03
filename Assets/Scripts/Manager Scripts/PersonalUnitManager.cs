using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonalUnitManager : Singleton<PersonalUnitManager>
{

    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> selectedUnits = new List<GameObject>();
   
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClearSelection()
    {
        //Clears render selection
        /*for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].GetComponentInChildren<Renderer>().material.shader = Shader.Find("Standard");
        }*/
        selectedUnits.Clear();
    }

    public void SetUnitsStart()
    {
        units.AddRange(GameObject.FindGameObjectsWithTag("AllyUnits"));
    }
}
