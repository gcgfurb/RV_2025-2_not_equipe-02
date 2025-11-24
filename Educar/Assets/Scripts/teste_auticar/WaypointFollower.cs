using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    public float detectionDistance = 5f;
    public float detectionRadius = 0.5f;
    public LayerMask obstacleLayer;

    private int currentIndex = 0;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 moveDirection = (target.position - transform.position).normalized;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = moveDirection;

        RaycastHit hit;
        bool obstacleInFront = false;

        if (Physics.SphereCast(origin, detectionRadius, direction, out hit, detectionDistance, obstacleLayer))
        {
            // Verifica se o objeto atingido está na layer "RCC_Vehicle"
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("RCC_Vehicle"))
            {
                obstacleInFront = true;
            }
        }

        float targetSpeed = obstacleInFront ? 0f : originalSpeed;
        speed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime * 5f);

        transform.position += moveDirection * speed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 gizmoOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 gizmoEnd = gizmoOrigin + transform.forward * detectionDistance;
        Gizmos.DrawLine(gizmoOrigin, gizmoEnd);
        Gizmos.DrawWireSphere(gizmoEnd, detectionRadius);
    }
}