using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Precisa desta linha

public class SeguirAlvoXR : MonoBehaviour
{
    public Transform alvo; // Arraste o "Ponto_Ancora_Espelho" para aqui

    private XRBaseInteractable interactable;
    private Vector3 offsetLocal;
    private Quaternion rotOffsetLocal;

    void Awake()
    {
        // Encontra o componente XR para saber se está a ser segurado
        interactable = GetComponent<XRBaseInteractable>();

        // "Ouve" os eventos de agarrar e soltar
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    void Start()
    {
        // Calcula o offset inicial
        CalcularOffset();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Quando agarramos, não fazemos nada. 
        // O LateUpdate vai parar de o "puxar" para o carro.
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Quando soltamos, recalculamos o offset a partir da NOVA posição.
        // É isto que faz o espelho "ficar" onde o ajustámos.
        CalcularOffset();
    }

    void CalcularOffset()
    {
        if (alvo != null)
        {
            // Guarda a posição e rotação relativas ao "Ponto de Âncora"
            offsetLocal = alvo.InverseTransformPoint(transform.position);
            rotOffsetLocal = Quaternion.Inverse(alvo.rotation) * transform.rotation;
        }
    }

    // Usamos LateUpdate para seguir movimentos de física (como o carro)
    void LateUpdate()
    {
        // Só segue o alvo se NÃO estivermos a segurá-lo
        if (alvo != null && !interactable.isSelected)
        {
            // Aplica o offset guardado à posição/rotação atual do Ponto de Âncora
            transform.position = alvo.TransformPoint(offsetLocal);
            transform.rotation = alvo.rotation * rotOffsetLocal;
        }
    }

    // Limpa os "ouvintes" quando o objeto é destruído
    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnGrab);
        interactable.selectExited.RemoveListener(OnRelease);
    }
}