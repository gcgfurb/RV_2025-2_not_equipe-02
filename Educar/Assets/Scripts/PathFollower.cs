using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float reachDistance = 1f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3f);
        }

        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0; 
            }
        }
    }
}
