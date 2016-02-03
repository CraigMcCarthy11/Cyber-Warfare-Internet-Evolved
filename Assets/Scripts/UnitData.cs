using UnityEngine;
using System.Collections;

public class UnitData : MonoBehaviour {

    public NavMeshAgent thisNavAgent;
    public int health = 100;
    public int damage;
    public UnitType unitType;
    public Faction factionType;

	// Use this for initialization
	void Start () {
        //factionType = GameManager.Instance.playerFaction;
        thisNavAgent = this.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*void SetUnitType()
    {
        switch (unitType)
        {
            case UnitType.Worker:
                print("Why hello there good sir! Let me teach you about Trigonometry!");
                break;
            case UnitType.Vehicle:
                print("Hello and good day!");
                break;
            case UnitType.Tank:
                print("Whadya want?");
                break;
            case UnitType.Shooter:
                print("Grog SMASH!");
                break;
        }
    }*/
}
