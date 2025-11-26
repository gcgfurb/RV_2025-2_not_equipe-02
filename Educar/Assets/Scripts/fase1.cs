using UnityEngine;

public class fase1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RCC_CarControllerV4 car = other.GetComponentInParent<RCC_CarControllerV4>();

        if (car != null)
        {
            car.canControl = false;

            Debug.Log("Fim da fase!");
            Debug.Log("Abrindo tela de Fase Concluída...");

            int faseAtual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            // Guarda qual fase estava sendo jogada
            PlayerPrefs.SetInt("LastPlayableSceneIndex", faseAtual);
            PlayerPrefs.Save();

            // Carrega a tela de fase concluída
            SceneController.Instance.LoadScene("FaseConcluida");
        }
    }
}
