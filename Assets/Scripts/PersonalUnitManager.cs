using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonalUnitManager : MonoBehaviour {

    private static PersonalUnitManager thisInstance;

    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> selectedUnits = new List<GameObject>();

    #region Singleton Stuff
    /// <summary>
    /// Constructor that handles getting and setting the instance
    /// this is using the singleton pattern
    /// </summary>
    public static PersonalUnitManager instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<PersonalUnitManager>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(thisInstance.gameObject);
            }

            return thisInstance;
        }
    }

    void Awake()
    {
        if (thisInstance == null)
        {
            //If I am the first instance, make me the Singleton
            thisInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != thisInstance)
                Destroy(this.gameObject);
        }
    }
    #endregion

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
