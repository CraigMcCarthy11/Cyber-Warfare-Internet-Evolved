using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour {

    public int inventorySpace = 50;
    public int currentInventory = 0;

    public GameObject lastResource;

	// Use this for initialization
	void Start () {
        UnitData unitData = this.gameObject.GetComponent<UnitData>();
        unitData.unitType = UnitType.Worker;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void GoToLastResource()
    {
        //This should be called when a worker hits the base and needs to repeat the cycle
        UIManager.instance.UpdateUserResource(currentInventory);
        currentInventory = 0;
        this.gameObject.GetComponent<UnitData>().thisNavAgent.speed = 7f;
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(lastResource.transform.position);
    }

    public IEnumerator StartGatheringResourceCycle(GameObject resource)
    {
        lastResource = resource;
        yield return new WaitForSeconds(1.5f);
        
        //Set inventory increment
        if (currentInventory != inventorySpace)
        {
            this.gameObject.GetComponent<UnitData>().thisNavAgent.speed = 3.5f;
            currentInventory = inventorySpace;
        }

        this.gameObject.GetComponent<PlayerUnitAIMove>().MoveToHomeBase();
    }
}
