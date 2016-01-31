using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour {

    public NavMeshAgent thisNavAgent;
    public int health = 100;
    public int inventorySpace = 50;
    public int currentInventory = 0;
    public int damage = 2;
    //public float speed = 3.5f;
    public UnitType unitType = UnitType.Worker;
    public Faction factionType;

    public GameObject lastResource;
	// Use this for initialization
	void Start () {
        factionType = GameManager.instance.playerFaction;
        thisNavAgent = this.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void GoToLastResource()
    {
        //This should be called when a worker hits the base and needs to repeat the cycle
        UIManager.instance.UpdateUserResource(currentInventory);
        currentInventory = 0;
        this.thisNavAgent.speed = 7f;
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(lastResource.transform.position);
    }

    public IEnumerator StartGatheringResourceCycle(GameObject resource)
    {
        lastResource = resource;
        yield return new WaitForSeconds(1.5f);
        
        //Set inventory increment
        if (currentInventory != inventorySpace)
        {
            this.thisNavAgent.speed = 3.5f;
            currentInventory = inventorySpace;
        }

        this.gameObject.GetComponent<PlayerUnitAIMove>().MoveToHomeBase();
    }
}
