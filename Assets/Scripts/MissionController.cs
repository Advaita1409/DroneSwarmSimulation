using UnityEngine;

public class MissionController : MonoBehaviour
{
    public Vector3[] waypoints;
    public float waypointRadius = 5f;
    private int currentWaypoint = 0;

    void Update()
    {
        UpdateMission();
    }

    public Vector3 GetCurrentObjective()
    {
        return waypoints[currentWaypoint];
    }

    public void UpdateMission()
    {
        // Check if current waypoint is reached
        float distanceToWaypoint = Vector3.Distance(
            transform.position, 
            waypoints[currentWaypoint]
        );

        if (distanceToWaypoint < waypointRadius)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}