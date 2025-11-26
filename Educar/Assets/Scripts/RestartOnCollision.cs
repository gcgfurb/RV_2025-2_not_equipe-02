using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnCollision : MonoBehaviour
{
    //[SerializeField] 
    private string gameOverSceneName = "GameOverBateuCarro";

    private void OnCollisionEnter(Collision collision)
    {
        // if (!collision.gameObject.CompareTag("PlayerCar")) return;

        Debug.Log("Colisão detectada! Indo para a tela de Game Over...");
        SceneManager.LoadScene(gameOverSceneName);
    }
}