﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager thisInstance;
    public GameObject homeBase;
    public EnumManager.Faction playerFaction;

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
        SetPlayerFaction();
        PersonalUnitManager.instance.SetUnitsStart();
        SetHomeBase();
        SetStartingInternetGas();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetHomeBase()
    {
        homeBase = GameObject.FindGameObjectWithTag("AllyBuilding");
    }

    void SetPlayerFaction()
    {
        //Gets the faction string and sets it to the enum
        string FactionString = PlayerPrefs.GetString("Player Faction");

        if (FactionString == "Horizon")
            playerFaction = EnumManager.Faction.Horizon;
        if (FactionString == "EightyAndTee")
            playerFaction = EnumManager.Faction.EightyAndTee;
        if (FactionString == "DiccsInternet")
            playerFaction = EnumManager.Faction.DiccsInternet;
        if (FactionString == "GoggleThread")
            playerFaction = EnumManager.Faction.GoggleThread;
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
