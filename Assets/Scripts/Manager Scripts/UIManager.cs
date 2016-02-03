using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    private MenuMode menuMode;
    public List<RectTransform> menuPanels = new List<RectTransform>();

    public Text resourceText;

    // Use this for initialization
    void Start()
    {
        resourceText.text = "Internet Gas: ";
        SetMenuMode(MenuMode.BuildingSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Does the ray intersect any objects which are in the player layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Building building = hit.collider.gameObject.GetComponent<Building>();
                if (building == null)
                {
                    SetMenuMode(MenuMode.BuildingSpawn);
                }
                else
                {
                    switch (building.buildingType)
                    {
                        case BuildingType.HomeBase:
                            {
                                SetMenuMode(MenuMode.HomeBase);
                                break;
                            }
                        case BuildingType.InternetGasCollector:
                            {
                                SetMenuMode(MenuMode.Collector);
                                break;
                            }
                        case BuildingType.Barracks:
                            {
                                SetMenuMode(MenuMode.Barracks);
                                break;
                            }
                        case BuildingType.Turret:
                            {
                                SetMenuMode(MenuMode.Turret);
                                break;
                            }
                        default:
                            {
                                SetMenuMode(MenuMode.BuildingSpawn);
                                break;
                            }
                    }
                }
            }
        }
    }

    public void UpdateUserResource(float resources)
    {
        GameManager.Instance.internetGas += resources;

        resourceText.text = "Internet Gas: " + GameManager.Instance.internetGas;
    }

    public void SetMenuMode(MenuMode newMode)
    {
        Debug.Log(newMode);
        for (int i = 0; i < menuPanels.Count; i++)
        {
            menuPanels[i].gameObject.SetActive(false);
        }

        menuPanels[(int)newMode].gameObject.SetActive(true);
        menuMode = newMode;
    }
}
