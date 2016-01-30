using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnitAIMove : MonoBehaviour {

    //Public Transform destination
    public bool isSelected = false;

    public NavMeshAgent agent;

	// Use this for initialization
	void Start () {
	    //Sets agent and sets destination
        agent = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<Renderer>().isVisible && Input.GetMouseButtonUp(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            //camPos.y = RTSCamera.Scre
        }
	}

    public void MakeSelected()
    {
        //If the survivor is selected and is NOT in selection
        isSelected = true;
        if (isSelected == true && !PersonalUnitManager.instance.selectedUnits.Contains(gameObject))
        {
            //Add to selection and add selection
            PersonalUnitManager.instance.selectedUnits.Add(gameObject);
            
            Camera.main.GetComponent<RTSCamera>().target = gameObject.transform;
            gameObject.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        }
    }
}
