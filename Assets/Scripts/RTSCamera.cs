using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour
{
    //Creating a fake start click that we can check against
    private Vector3 startClick = -Vector3.one;
    public RectTransform selectionBox;
    public static Rect selection = new Rect(0, 0, 0, 0);
    public int cameraSpeed = 1;
    public int clippingSize = 25;
    public Transform target;
    //Move to canvas objects
    public GameObject goToCanvasObject;
    public GameObject createdUI;

    public bool isLocked = false;

    private float doubleTapTime;

    // Use this for initialization
    void Start()
    {
        selectionBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Maybe combine these?
        HandleInput();
        CheckCamera();
        HandleLockedCameraAndMovement();
    }

    #region CAMERA FUNCTIONS
    void HandleLockedCameraAndMovement()
    {
        //Instantly sets camera to last survivor selected
        if (Input.GetKeyDown("space"))
        {
            isLocked = false;
            Vector3 targetVector = target.transform.position;
            targetVector.y = transform.position.y;
            targetVector.z = target.transform.position.z - 21;
            transform.position = targetVector;
            //Checks if you double tap it
            if (Time.time < doubleTapTime + .3f)
            {
                isLocked = true;
            }
            doubleTapTime = Time.time;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(0, 0, -cameraSpeed, Space.Self);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(0, 0, +cameraSpeed, Space.Self);
        }

        //As long as we are not following someone, we can use edge panning
        if (isLocked == false)
        {
            //Set the boxes on each side
            var recdown = new Rect(0, 0, Screen.width, clippingSize);
            var recup = new Rect(0, Screen.height - clippingSize, Screen.width, clippingSize);
            var recleft = new Rect(0, 0, clippingSize, Screen.width);
            var recright = new Rect(Screen.width - clippingSize, 0, clippingSize, Screen.height);
            //Move accordingly
            if (recdown.Contains(Input.mousePosition) || Input.GetKey("s"))
            {
                transform.Translate(0, -cameraSpeed, -cameraSpeed, Space.Self);
            }
            if (recup.Contains(Input.mousePosition) || Input.GetKey("w"))
            {
                transform.Translate(0, cameraSpeed, cameraSpeed, Space.Self);
            }
            if (recleft.Contains(Input.mousePosition) || Input.GetKey("a"))
            {
                transform.Translate(-cameraSpeed, 0, 0, Space.Self);
            }
            if (recright.Contains(Input.mousePosition) || Input.GetKey("d"))
            {
                transform.Translate(cameraSpeed, 0, 0, Space.Self);
            }
            if (Input.GetKey("q"))
            {
                transform.Rotate(new Vector3(0, 1, -1) * cameraSpeed, Space.Self);
            }
            if (Input.GetKey("e"))
            {
                transform.Rotate(new Vector3(0, -1, 1) * cameraSpeed, Space.Self);
            }
        }
        else if (isLocked)
        {
            //While we are locked stay on our target
            Vector3 targetVector = target.transform.position;
            targetVector.y = transform.position.y;
            targetVector.z = target.transform.position.z - 21;
            transform.position = targetVector;
        }
    }

    void HandleInput()
    {
        //If right click
        if (Input.GetMouseButtonDown(1))
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            //Move the player to the click if they are in selected
            if (Physics.Raycast(screenRay, out hit, 100))
            {
                //If we click on a weapon
                if (hit.collider.gameObject.tag == "Resource")
                {
                    //Move the character to this location
                    for (int i = 0; i < PersonalUnitManager.instance.selectedUnits.Count; i++)
                    {
                        PersonalUnitManager.instance.selectedUnits[i].GetComponent<PlayerUnitAIMove>().agent.SetDestination(hit.point);
                    }

                    GameObject resource = hit.collider.gameObject;
                    Debug.Log("Resource");
                }
                else
                {
                    for (int i = 0; i < PersonalUnitManager.instance.selectedUnits.Count; i++)
                    {
                        Debug.Log("Hit the ground! Moving selected to this location");
                        PersonalUnitManager.instance.selectedUnits[i].GetComponent<PlayerUnitAIMove>().agent.SetDestination(hit.point);
                    }
                    //Instantiate the UI where the current selection is moving too.
                    //Used for placing texture
                    float surfaceOffset = 0.1f;
                    if (createdUI != null)
                    {
                        DestroyImmediate(createdUI);
                    }

                    if (PersonalUnitManager.instance.selectedUnits.Count > 0)
                    {
                        createdUI = GameObject.Instantiate(goToCanvasObject, (hit.point + hit.normal * surfaceOffset), Quaternion.Euler(90, 0, 0)) as GameObject;
                    }
                }
            }

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Destroy the placement
            if (createdUI != null)
            {
                DestroyImmediate(createdUI);
            }
            RaycastHit hit;
            //On Left mouse click
            if (Physics.Raycast(screenRay, out hit, 100))
            {
                //If we hit a character
                if (PersonalUnitManager.instance.units.Contains(hit.collider.gameObject))
                {
                    //If a character is selected, remove this selection
                    if (PersonalUnitManager.instance.selectedUnits.Count != 0)
                    {
                        PersonalUnitManager.instance.ClearSelection();
                    }
                    //Add him to selected
                    hit.collider.gameObject.GetComponent<PlayerUnitAIMove>().MakeSelected();
                    Camera.main.GetComponent<RTSCamera>().target = hit.collider.gameObject.transform;
                }
                /*else if (BuildingManager.in)
                {

                }*/
                else //We hit something other than a character
                {
                    //If we hit the ground we clear the selection
                    PersonalUnitManager.instance.ClearSelection();
                }
            }
        }
    }

    private void CheckCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionBox.gameObject.SetActive(true);
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
            startClick = -Vector3.one;

            selectionBox.sizeDelta = new Vector2(0, 0); // ====== ADDED ====== 
            selectionBox.position = new Vector2(0,0); // ====== ADDED ======
            
        }

        //Creates the selection
        if (Input.GetMouseButton(0))
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
        if (selection.width < 0) 
        { 
            selection.x += selection.width; 
            selection.width = -selection.width; 
        } 
        if (selection.height < 0) 
        { 
            selection.y += selection.height; 
            selection.height = -selection.height; 
        } 
        selectionBox.position = new Vector2(selection.x, InvertMouseY(selection.y + selection.height)); // ====== ADDED ====== 
        selectionBox.sizeDelta = new Vector2(selection.width, selection.height); // ====== ADDED ====== }
    }
    #endregion

    public static float InvertMouseY(float y) { 
        return Screen.height - y; 
    }
}
