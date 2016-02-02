using UnityEngine;
using System.Collections;

public class EnemyAIFaction : MonoBehaviour {

    public Faction thisAIsFaction;
    public BuildingManager thisBuildingManager;
    public GameObject myHomeBase;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetThisFaction(Faction thisFaction)
    {
        thisAIsFaction = thisFaction;
    }

}
