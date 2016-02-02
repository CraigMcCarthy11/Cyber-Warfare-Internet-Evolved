using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<UnitData>().unitType = UnitType.Tank;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
