using UnityEngine;

public class RedLightTrigger : MonoBehaviour
{
    public TrafficLightController trafficLight;
    public float forcedRedDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("carro")){
        if (!trafficLight.isRedLightActive){
            trafficLight.ForceRedLight(forcedRedDuration);
            Debug.Log("vermelho");
        }
        //}
    }
}
