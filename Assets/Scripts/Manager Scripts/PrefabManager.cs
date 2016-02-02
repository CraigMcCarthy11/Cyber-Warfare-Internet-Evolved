using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

    private static PrefabManager thisInstance;

    public GameObject aiScriptPrefab;

    #region Singleton Stuff
    /// <summary>
    /// Constructor that handles getting and setting the instance
    /// this is using the singleton pattern
    /// </summary>
    public static PrefabManager instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<PrefabManager>();

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
}
