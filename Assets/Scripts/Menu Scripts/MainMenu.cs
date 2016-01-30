using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartedGame()
    {
        //Gets the faction string
        string playerFaction = PlayerPrefs.GetString("Player Faction");
        //If its set, we can start the game
        if (playerFaction != null){
            Application.LoadLevel(1);
        }
    }
}