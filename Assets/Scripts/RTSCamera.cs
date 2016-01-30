using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour
{
    public static Rect selection = new Rect(0, 0, 0, 0);
    public int speed = 1;
    public int clippingSize = 25;
    public Texture2D selectionBox = null;
    public Transform target;

    public GameObject goToCanvasObject;
    public GameObject createdUI;

    public bool isLocked = false;

    private float doubleTapTime;
    //Creating a fake start click we can check against
    private Vector3 startClick = -Vector3.one;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            transform.Translate(0, 0, -speed, Space.Self);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(0, 0, +speed, Space.Self);
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
                transform.Translate(0, -speed, -speed, Space.Self);
            }
            if (recup.Contains(Input.mousePosition) || Input.GetKey("w"))
            {
                transform.Translate(0, speed, speed, Space.Self);
            }
            if (recleft.Contains(Input.mousePosition) || Input.GetKey("a"))
            {
                transform.Translate(-speed, 0, 0, Space.Self);
            }
            if (recright.Contains(Input.mousePosition) || Input.GetKey("d"))
            {
                transform.Translate(speed, 0, 0, Space.Self);
            }
            if (Input.GetKey("q"))
            {
                transform.Rotate(new Vector3(0, 1, -1) * speed, Space.Self);
            }
            if (Input.GetKey("e"))
            {
                transform.Rotate(new Vector3(0, -1, 1) * speed, Space.Self);
            }
        }
        else if (isLocked)
        {
            //While we are locked stay on our target
            Vector3 targetVector = target.transform.position;
            targetVector.y = transform.position.y;
            targetVector.z = target.transform.position.z - 21;
            transform.position = targetVector;
            /*if (Input.GetKey("q"))
            {
                //Orbit camera to left centered around unit
            }
            if (Input.GetKey("e"))
            {
                //Orbit camera to right centered around unit
            }*/
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
                    Destroy(createdUI);
                }
                createdUI = GameObject.Instantiate(goToCanvasObject, (hit.point + hit.normal * surfaceOffset), Quaternion.Euler(90, 0, 0)) as GameObject;
            }

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Destroy the placement
            if (createdUI != null)
            {
                DestroyImmediate(createdUI, true);
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
            startClick = Input.mousePosition;

        else if (Input.GetMouseButtonUp(0))
        {
            //We cant check the selection box containing 
            //unless the selection box is positive
            //so this fixes people clicking bottom right -> top left
            //By inversing the selection
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
            startClick = -Vector3.one;
        }
        //Creates the selection
        if (Input.GetMouseButton(0))
            selection = new Rect(startClick.x, ScreenToRectSpaceInvert(startClick.y), Input.mousePosition.x - startClick.x, ScreenToRectSpaceInvert(Input.mousePosition.y) - ScreenToRectSpaceInvert(startClick.y));
    }
    #endregion

    private void OnGUI()
    {
        //Draws the box on screen
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionBox);
        }
    }

    public static float ScreenToRectSpaceInvert(float y)
    {
        //We have to invert the mouse return because
        //unity screen space coordinates are opposite
        //than the numbers we return when raycasting
        return Screen.height - y;
    }
}
