using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnitAIMove : MonoBehaviour
{
    //public Transform destination;
    public bool isSelected = false;

    public NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        //Sets agent and sets destination
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //If we can see the object and we are selecting it, add it to the list.
        if (gameObject.GetComponent<Renderer>().isVisible && Input.GetMouseButtonUp(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = RTSCamera.InvertMouseY(camPos.y);

            if (RTSCamera.selection.Contains(camPos))
            {
                MakeSelected();
            }
        }
    }

    public void MakeSelected()
    {
        //If the survivor is selected and is NOT in selection
        isSelected = true;
        if (isSelected == true && !PersonalUnitManager.instance.selectedUnits.Contains(gameObject) && gameObject.GetComponent<UnitData>().factionType == GameManager.instance.playerFaction)
        {
            //Add to selection and add selection
            PersonalUnitManager.instance.selectedUnits.Add(gameObject);
            //Add selected to text on UI
            //UIManager.instance.survivorsSelected.text = UIManager.instance.survivorsSelected.text + " " + gameObject.GetComponent<SurvivorClass>().name;

            Camera.main.GetComponent<RTSCamera>().target = gameObject.transform;
            //gameObject.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //When resource is entered
        if (col.gameObject.tag == "Resource")
        {
            //Checks to see if this is a worker, if it is, then start the resource gathering
            Worker script = this.gameObject.GetComponent<Worker>();
            if (script != null)
            {
                //StartCoroutine(script.StartGatheringResourceCycle(col.gameObject));
                Debug.Log("HEY DUMB SHIT WHY DONT YOU BUILD SOMETHING HERE XD!");
            }
        }
        //When ally building is entered
        else if (col.gameObject.tag == "AllyBuilding")
        {
            BuildingType type = col.gameObject.GetComponent<Building>().buildingType;         
            //and its the home base
            if (type == BuildingType.HomeBase)
            {
                Worker script = this.gameObject.GetComponent<Worker>();
                if (script != null && script.lastResource != null)
                {
                    script.GoToLastResource();
                }
            }
            else if (type == BuildingType.InternetGasCollector)
            {
                //Start getting resources
                Worker script = this.gameObject.GetComponent<Worker>();
                if (script != null)
                {
                    StartCoroutine(script.StartGatheringResourceCycle(col.gameObject));
                }
            }
            else if (type == BuildingType.Barracks)
            {
                Debug.Log("In the barracks");
            }
            else if (type == BuildingType.Turret)
            {
                Debug.Log("In the turret");
            }
        }
    }

    public void MoveToHomeBase()
    {
        //Move the character to this location
        this.gameObject.GetComponent<PlayerUnitAIMove>().agent.SetDestination(GameManager.instance.playerHomeBase.transform.position);
    }
}
