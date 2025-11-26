using UnityEngine;

using UnityEngine.SceneManagement;


public class RedLightDetectorFase5 : MonoBehaviour
{
    public TrafficLightControllerFase5 trafficLight;

    private string gameOverSceneName = "GameOverAtravessouSinal";

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("colidiu");a
        //if (other.CompareTag("carro")) {
        Debug.Log("colidiu com o carro");
        if (trafficLight.isRedLightActive)
        {
            Debug.Log("Infração: carro passou no sinal vermelho!");
            SceneManager.LoadScene(gameOverSceneName);

        }
        else
        {
            Debug.Log("não pego");
        }
        //}
    }
}
