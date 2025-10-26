using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnCollision : MonoBehaviour
{
    [SerializeField] private string gameOverSceneName = "GameOver";

    private void OnCollisionEnter(Collision collision)
    {
        // Exemplo: se quiser restringir apenas ao carro do jogador, descomente a linha abaixo
        // if (!collision.gameObject.CompareTag("PlayerCar")) return;

        Debug.Log("Colisão detectada! Indo para a tela de Game Over...");
        SceneManager.LoadScene(gameOverSceneName);
    }
}