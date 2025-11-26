using UnityEngine;
using UnityEngine.SceneManagement;

public class fimfase : MonoBehaviour
{
    private string end = "JogoConcluido";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fase finalizada! Parabéns.");
        SceneManager.LoadScene(end);
    }
}
