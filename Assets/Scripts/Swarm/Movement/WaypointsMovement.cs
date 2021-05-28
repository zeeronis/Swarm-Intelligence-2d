using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float arriveDistance;
    [SerializeField] private Transform[] waypoints;

    private int waypointIndex;


    private void Awake()
    {
        float dist = float.MaxValue;
        for (int i = 0; i < waypoints.Length; i++)
        {
            float newDist = Vector2.Distance(transform.position, waypoints[i].position);
            if (dist > newDist)
            {
                dist = newDist;
                waypointIndex = i;
            }
        }
    }

    private void Update()
    {
        Vector3 direction = (waypoints[waypointIndex].position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector2.Distance(waypoints[waypointIndex].position, transform.position) <= arriveDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }
}
