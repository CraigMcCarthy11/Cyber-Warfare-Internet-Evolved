using UnityEngine;
using System.Collections;

public class FactionSelect : MonoBehaviour {

    public EnumManager.Faction chosenFaction;

    public void ButtonPressed()
    {
        PlayerPrefs.SetString("Player Faction", chosenFaction.ToString());

        Debug.Log(PlayerPrefs.GetString("Player Faction"));
    }
}
