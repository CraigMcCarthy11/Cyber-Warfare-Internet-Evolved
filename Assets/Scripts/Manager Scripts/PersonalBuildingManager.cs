using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonalBuildingManager : MonoBehaviour {

    private static PersonalBuildingManager thisInstance;

    public List<GameObject> buildings = new List<GameObject>();
    //public List<GameObject> selectedUnits = new List<GameObject>();

    #region Singleton Stuff
    /// <summary>
    /// Constructor that handles getting and setting the instance
    /// this is using the singleton pattern
    /// </summary>
    public static PersonalBuildingManager instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<PersonalBuildingManager>();

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

    void Start()
    {

    }
}
