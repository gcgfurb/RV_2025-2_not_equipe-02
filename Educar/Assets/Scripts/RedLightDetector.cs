using UnityEngine;

using UnityEngine.SceneManagement;


public class RedLightDetector : MonoBehaviour
{
    public TrafficLightController trafficLight;

    private string gameOverSceneName = "GameOverAtravessouSinal";

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("colidiu");
        //if (other.CompareTag("carro")) {
            Debug.Log("colidiu com o carro");
            if (trafficLight.isRedLightActive)
            {
                Debug.Log("Infração: carro passou no sinal vermelho!");
                SceneManager.LoadScene(gameOverSceneName);

            } else {
                Debug.Log("não pego");
            }
        //}
    }
}
