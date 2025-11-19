using UnityEngine;

public class RCC_LogitechShifter : MonoBehaviour 
{
    private RCC_CarControllerV4 carController;
    private bool lastShifterPressed;

    void Start()
    {
        // Pega automaticamente o RCC_CarControllerV4 deste GameObject
        carController = GetComponent<RCC_CarControllerV4>();
        
        if (carController) {
            // Configura o carro para modo manual
            carController.semiAutomaticGear = false;
            carController.automaticClutch = false;
        }
    }

    void Update()
    {
        if (!carController)
            return;

        // Axis para a embreagem (normalmente é o pedal esquerdo do G29/G920)
        float clutch = Input.GetAxis("Clutch");
        carController.inputs.clutchInput = clutch;

        // Só permite troca de marcha se a embreagem estiver pressionada (ajuste o threshold se necessário)
        bool clutchPressed = clutch > 0.1f;

        if (clutchPressed) {
            // Lê os botões do H-shifter
            // Estes números podem precisar ser ajustados baseado no seu setup específico
            if (Input.GetKey(KeyCode.JoystickButton12)) // 1ª marcha
                ShiftTo(0);
            else if (Input.GetKey(KeyCode.JoystickButton13)) // 2ª marcha
                ShiftTo(1);
            else if (Input.GetKey(KeyCode.JoystickButton14)) // 3ª marcha
                ShiftTo(2);
            else if (Input.GetKey(KeyCode.JoystickButton15)) // 4ª marcha
                ShiftTo(3);
            else if (Input.GetKey(KeyCode.JoystickButton16)) // 5ª marcha
                ShiftTo(4);
            else if (Input.GetKey(KeyCode.JoystickButton17)) // 6ª marcha
                ShiftTo(5);
            else if (Input.GetKey(KeyCode.JoystickButton18)) // Ré
                ShiftTo(-1);
            else if (!IsAnyShifterButtonPressed()) {
                // Se nenhum botão estiver pressionado, coloca em neutro
                carController.NGear = true;
                lastShifterPressed = false;
            }
        } else {
            // Se a embreagem não está pressionada, não troca marcha e reseta flag
            lastShifterPressed = false;
        }
    }

    private void ShiftTo(int gear) {
        if (!lastShifterPressed) {
            carController.NGear = false;
            carController.GearShiftTo(gear);
            lastShifterPressed = true;
        }
    }

    private bool IsAnyShifterButtonPressed() {
        return Input.GetKey(KeyCode.JoystickButton13) || 
               Input.GetKey(KeyCode.JoystickButton14) ||
               Input.GetKey(KeyCode.JoystickButton15) || 
               Input.GetKey(KeyCode.JoystickButton16) ||
               Input.GetKey(KeyCode.JoystickButton17) || 
               Input.GetKey(KeyCode.JoystickButton18) ||
               Input.GetKey(KeyCode.JoystickButton19);
    }
}