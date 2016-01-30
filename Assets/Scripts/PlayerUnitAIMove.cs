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
            Debug.Log("MEME 1");
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = RTSCamera.ScreenToRectSpaceInvert(camPos.y);
            if (RTSCamera.selection.Contains(camPos))
            {
                Debug.Log("MEME 2");
                MakeSelected();
            }
        }
    }

    public void MakeSelected()
    {
        Debug.Log("MEME 3");
        //If the survivor is selected and is NOT in selection
        isSelected = true;
        if (isSelected == true && !PersonalUnitManager.instance.selectedUnits.Contains(gameObject))
        {
            Debug.Log("MEME 4");
            //Add to selection and add selection
            PersonalUnitManager.instance.selectedUnits.Add(gameObject);
            //Add selected to text on UI
            //UIManager.instance.survivorsSelected.text = UIManager.instance.survivorsSelected.text + " " + gameObject.GetComponent<SurvivorClass>().name;

            Camera.main.GetComponent<RTSCamera>().target = gameObject.transform;
            gameObject.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        }
    }
}
