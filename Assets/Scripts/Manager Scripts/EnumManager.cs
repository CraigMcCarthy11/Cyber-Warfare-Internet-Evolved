using UnityEngine;
using System.Collections;

public class EnumManager : MonoBehaviour {

    public enum Status { Normal, Bleeding, Encumbered, Dead };

    public enum UnitType { Worker, Tank, Shooter, Vehicle };

    public enum Faction { Horizon, EightyAndTee, DiccsInternet, GoggleThread };

    public enum BuildingType { HomeBase, InternetGasCollector, Barracks, Turret };
}
