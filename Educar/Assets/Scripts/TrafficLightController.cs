using UnityEngine;
using System.Collections;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float greenTime = 5f;
    public float yellowTime = 2f;
    public float redTime = 5f;

    public bool isRedLightActive { get; private set; } = false;

    private float timer;
    private bool isForcedRed = false;

    private enum LightState { Green, Yellow, Red }
    private LightState currentState = LightState.Green;

    public string CurrentLightState => currentState.ToString();
    public bool IsYellow => currentState == LightState.Yellow;


    void Start()
    {
        SetLightState(currentState);
    }

    void Update()
    {
        if (isForcedRed) return; 

        timer += Time.deltaTime;

        switch (currentState)
        {
            case LightState.Green:
                if (timer >= greenTime)
                    ChangeState(LightState.Yellow);
                break;

            case LightState.Yellow:
                if (timer >= yellowTime)
                    ChangeState(LightState.Red);
                break;

            case LightState.Red:
                if (timer >= redTime)
                    ChangeState(LightState.Green);
                break;
        }
    }

    void ChangeState(LightState newState)
    {
        currentState = newState;
        timer = 0f;
        SetLightState(newState);
    }

    void SetLightState(LightState state)
    {
        redLight.SetActive(state == LightState.Red);
        yellowLight.SetActive(state == LightState.Yellow);
        greenLight.SetActive(state == LightState.Green);
        isRedLightActive = (state == LightState.Red);
    }

    public void ForceRedLight(float duration)
    {
        StartCoroutine(ForceRedCoroutine(duration));
    }

    private IEnumerator ForceRedCoroutine(float duration)
    {
        isForcedRed = true;
        SetLightState(LightState.Yellow);
        yield return new WaitForSeconds(2f);

        SetLightState(LightState.Red);
        Debug.Log("vermelho");
        yield return new WaitForSeconds(duration);

        //Debug.Log("nml");
        isForcedRed = false;
        ChangeState(LightState.Green);
    }
}
