using UnityEngine;

public class fase1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o carro entrou no trigger
        RCC_CarControllerV4 car = other.GetComponentInParent<RCC_CarControllerV4>();
        if (car != null)
        {
            // Desativa o controle do jogador
            car.canControl = false;

            // Desliga o motor
            car.KillEngine();

            // Aciona o freio
            car.brakeInput = 1f;

            // Zera a aceleração pra garantir
            car.throttleInput = 0f;

            // Mensagem de debug
            Debug.Log("Fim da fase! Motor desligado, carro freado e controle desativado.");
        }
    }
}
