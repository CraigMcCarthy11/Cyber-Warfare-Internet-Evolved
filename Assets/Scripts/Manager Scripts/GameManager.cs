using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] homeBases;
    public GameObject playerHomeBase;
    public GameObject scriptHolder;

    public Faction playerFaction;

    //Resources
    public float internetGas;

    // Use this for initialization
    void Start()
    {
        scriptHolder = this.gameObject;
        SetPlayerFaction();
        PersonalUnitManager.Instance.SetUnitsStart();
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
                obj = GameObject.Instantiate(PrefabManager.Instance.aiScriptPrefab, homeBases[i].gameObject.transform.position, Quaternion.identity) as GameObject;
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
        UIManager.Instance.resourceText.text = "Internet Gas: " + GameManager.Instance.internetGas;

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
