using UnityEngine;

public class InverseTrafficLightControllerFase5 : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public TrafficLightControllerFase5 mainTrafficLight;

    void Update()
    {
        if (mainTrafficLight == null)
            return;

        string mainState = mainTrafficLight.CurrentLightState;

        switch (mainState)
        {
            case "Red":
                SetLightState(LightState.Green);
                break;

            case "Green":
                SetLightState(LightState.Red);
                break;

            case "Yellow":
                SetLightState(LightState.Yellow);
                break;
        }
    }

    private enum LightState { Green, Yellow, Red }

    private void SetLightState(LightState state)
    {
        redLight.SetActive(state == LightState.Red);
        yellowLight.SetActive(state == LightState.Yellow);
        greenLight.SetActive(state == LightState.Green);
    }
}
