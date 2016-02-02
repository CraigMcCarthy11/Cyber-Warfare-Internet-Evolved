using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<UnitData>().unitType = UnitType.Shooter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
