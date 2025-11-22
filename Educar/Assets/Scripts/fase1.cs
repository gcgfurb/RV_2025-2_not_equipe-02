using UnityEngine;
using UnityEngine.SceneManagement;

public class fase1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RCC_CarControllerV4 car = other.GetComponentInParent<RCC_CarControllerV4>();
        if (car != null)
        {
            car.canControl = false;

            Debug.Log("Fim da fase!");
            Debug.Log("Carregando próxima fase...");

            Time.timeScale = 1f;
            AudioListener.pause = false;

            int proximaFase = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(proximaFase);
        }
    }
}
