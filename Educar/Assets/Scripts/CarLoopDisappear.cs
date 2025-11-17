using UnityEngine;
using System.Collections;

public class CarLoopFullPath : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float reachDistance = 1f;
    public float waitTime = 3f;

    private int currentWaypoint = 0;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isMoving = false;
    private bool isLooping = false;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Nenhum waypoint atribuído!");
            enabled = false;
            return;
        }

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Inicia com tempo de espera antes de começar o movimento
        StartCoroutine(InitialDelayBeforeStart());
    }

    IEnumerator InitialDelayBeforeStart()
    {
        Debug.Log("Esperando antes de iniciar o trajeto...");
        yield return new WaitForSeconds(waitTime);
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0 || isLooping) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                StartCoroutine(LoopBackToStart());
            }
        }
    }

    IEnumerator LoopBackToStart()
    {
        isLooping = true;
        isMoving = false;

        Debug.Log("Desaparece o carro");
        SetCarVisible(false);

        yield return new WaitForSeconds(waitTime);

        Debug.Log("Reseta posição e rotação");
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        currentWaypoint = 0;

        Debug.Log("Reaparece o carro");
        SetCarVisible(true);
        isMoving = true;
        isLooping = false;
    }

    void SetCarVisible(bool visible)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = visible;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = visible;
    }
}
