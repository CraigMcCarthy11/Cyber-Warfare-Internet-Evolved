using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager thisInstance;
    public GameObject[] homeBases;
    public GameObject playerHomeBase;
    public GameObject scriptHolder;

    public Faction playerFaction;

    //Resources
    public float internetGas;

    #region Singleton Stuff
    /// <summary>
    /// Constructor that handles getting and setting the instance
    /// this is using the singleton pattern
    /// </summary>
    public static GameManager instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<GameManager>();

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
    void Start()
    {
        scriptHolder = this.gameObject;
        SetPlayerFaction();
        PersonalUnitManager.instance.SetUnitsStart();
        SetHomeBases();
        SetStartingInternetGas();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetHomeBases()
    {
        //Goes through all the home bases and sets this player base as the correct one on games start
        homeBases = GameObject.FindGameObjectsWithTag("AllyBuilding");
        for (int i = 0; i < homeBases.Length; i++)
        {
            if (homeBases[i].GetComponent<Building>().faction == playerFaction)
            {
                playerHomeBase = homeBases[i];
            }
            else
            {
                //We create the enemy building stuff
                //And set its script faction starting shit
                GameObject obj;
                homeBases[i].gameObject.tag = "EnemyBuilding";
                obj = GameObject.Instantiate(PrefabManager.instance.aiScriptPrefab, homeBases[i].gameObject.transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<EnemyAIFaction>().thisAIsFaction = homeBases[i].GetComponent<Building>().faction;
                obj.GetComponent<EnemyAIFaction>().myHomeBase = homeBases[i];
                obj.name = homeBases[i].GetComponent<Building>().faction.ToString() + " AI Scripts";
            }
        }
    }

    void SetPlayerFaction()
    {
        //Gets the faction string and sets it to the enum
        string FactionString = PlayerPrefs.GetString("Player Faction");

        if (FactionString == "Horizon")
            playerFaction = Faction.Horizon;
        if (FactionString == "EightyAndTee")
            playerFaction = Faction.EightyAndTee;
        if (FactionString == "DiccsInternet")
            playerFaction = Faction.DiccsInternet;
        if (FactionString == "GoggleThread")
            playerFaction = Faction.GoggleThread;
    }

    void SetStartingInternetGas()
    {
        internetGas = 50;
        UIManager.instance.resourceText.text = "Internet Gas: " + GameManager.instance.internetGas;

        //Gets the faction string and sets it to the enum
        //string FactionString = PlayerPrefs.GetString("Player Faction");

        /*if (FactionString == "Horizon")
            playerFaction = EnumManager.Faction.Horizon;
        if (FactionString == "EightyAndTee")
            playerFaction = EnumManager.Faction.EightyAndTee;
        if (FactionString == "DiccsInternet")
            playerFaction = EnumManager.Faction.DiccsInternet;
        if (FactionString == "GoggleThread")
            playerFaction = EnumManager.Faction.GoggleThread;*/
    }

}
