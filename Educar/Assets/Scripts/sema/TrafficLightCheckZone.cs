using UnityEngine;
using System.Collections.Generic;

public class TrafficLightCheckZone : MonoBehaviour
{
    public TrafficLightControllerFase5 trafficLight;
    public InverseTrafficLightControllerFase5 inverseTrafficLight;

    private List<WaypointFollower> carsInZone = new List<WaypointFollower>();

    void Update()
    {
        bool isRed = false;

        if (trafficLight != null)
        {
            isRed = trafficLight.CurrentLightState == "Red";
        }
        else if (inverseTrafficLight != null && inverseTrafficLight.mainTrafficLight != null)
        {
            string mainState = inverseTrafficLight.mainTrafficLight.CurrentLightState;
            isRed = (mainState == "Green");
        }

        foreach (var car in carsInZone)
        {
            if (car != null)
                car.SetStop(isRed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponent<WaypointFollower>();
        if (car != null && !carsInZone.Contains(car))
        {
            carsInZone.Add(car);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var car = other.GetComponent<WaypointFollower>();
        if (car != null && carsInZone.Contains(car))
        {
            car.SetStop(false);
            carsInZone.Remove(car);
        }
    }
}
