using UnityEngine;
using UnityEngine.SceneManagement;

public class SeatbeltChecker : MonoBehaviour
{
    public float minAceleracao = 0.1f;
    private RCC_CarControllerV4 car;

    void Start()
    {
        car = GameObject.FindAnyObjectByType<RCC_CarControllerV4>();
    }

    void Update()
    {
        // Regra só na FASE 1
        if (SceneManager.GetActiveScene().name != "f1")
            return;

        // Se acelerar sem cinto → GameOver
        if (car.throttleInput > minAceleracao)
        {
            if (!BeltHandleBehavior.cintoEncaixado)
            {
                Debug.Log("Game Over — acelerou sem cinto!");
                SceneController.Instance.LoadScene("GameOverCinto");
            }
        }
    }
}
