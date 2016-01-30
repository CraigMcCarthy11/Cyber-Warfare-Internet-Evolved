using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager thisInstance;

    public Text resourceText; 
    #region Singleton Stuff
    /// <summary>
    /// Constructor that handles getting and setting the instance
    /// this is using the singleton pattern
    /// </summary>
    public static UIManager instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<UIManager>();

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
        resourceText.text = "Internet Gas: ";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateUserResource(float resources)
    {
        GameManager.instance.internetGas += resources;

        resourceText.text = "Internet Gas: " + GameManager.instance.internetGas;
    }
}
