using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("carro")) {
            Debug.Log("bateu, animal");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }
}
