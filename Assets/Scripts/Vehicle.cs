using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<UnitData>().unitType = UnitType.Vehicle;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
