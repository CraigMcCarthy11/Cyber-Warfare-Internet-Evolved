using UnityEngine;
using System.Collections;

public class FactionSelect : MonoBehaviour {

    public Faction chosenFaction;

    public void ButtonPressed()
    {
        PlayerPrefs.SetString("Player Faction", chosenFaction.ToString());
    }
}
