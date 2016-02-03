using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    private static UIManager thisInstance;

    private MenuMode menuMode;
    public List<RectTransform> menuPanels = new List<RectTransform>();

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
        SetMenuMode(MenuMode.BuildingSpawn);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Does the ray intersect any objects which are in the player layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Building building = hit.collider.gameObject.GetComponent<Building>();
                if (building == null)
                {
                    SetMenuMode(MenuMode.BuildingSpawn);
                }
                else
                {
                    switch(building.buildingType)
                    {
                        case BuildingType.HomeBase:
                            {
                                SetMenuMode(MenuMode.HomeBase);
                                break;
                            }
                        case BuildingType.InternetGasCollector:
                            {
                                SetMenuMode(MenuMode.Collector);
                                break;
                            }
                        case BuildingType.Barracks:
                            {
                                SetMenuMode(MenuMode.Barracks);
                                break;
                            }
                        case BuildingType.Turret:
                            {
                                SetMenuMode(MenuMode.Turret);
                                break;
                            }
                        default:
                            {
                                SetMenuMode(MenuMode.BuildingSpawn);
                                break;
                            }
                    }
                }
            }
        }
	}

    public void UpdateUserResource(float resources)
    {
        GameManager.instance.internetGas += resources;

        resourceText.text = "Internet Gas: " + GameManager.instance.internetGas;
    }

    public void SetMenuMode(MenuMode newMode)
    {
        Debug.Log(newMode);
        for(int i = 0; i < menuPanels.Count; i++)
        {
            menuPanels[i].gameObject.SetActive(false);
        }
        
        menuPanels[(int)newMode].gameObject.SetActive(true);
        menuMode = newMode;
    }
}
