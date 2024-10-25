using UnityEngine;
using System.Collections.Generic;

public class DroneSwarmOptimization : MonoBehaviour
{
    public GameObject dronePrefab; // Assign your drone prefab in the Inspector
    public GameObject target; // The target object for drones to reach
    public int numberOfDrones = 10;
    public float inertia = 0.5f;
    public float cognitiveWeight = 1.0f;
    public float socialWeight = 1.0f;
    public float avoidanceDistance = 1.0f; // Distance for collision avoidance

    private List<Drone> drones = new List<Drone>();

    void Start()
    {
        // Instantiate drones
        for (int i = 0; i < numberOfDrones; i++)
        {
            GameObject droneObj = Instantiate(dronePrefab, Random.insideUnitSphere * 5, Quaternion.identity);
            Drone drone = new Drone(droneObj.transform);
            drones.Add(drone);
        }
    }

    void Update()
    {
        foreach (var drone in drones)
        {
            drone.UpdatePosition(target.transform.position, drones, inertia, cognitiveWeight, socialWeight, avoidanceDistance);
        }
    }
}

public class Drone
{
    private Transform transform;
    private Vector3 velocity;
    private Vector3 bestPosition;

    public Drone(Transform transform)
    {
        this.transform = transform;
        this.velocity = Random.insideUnitSphere;
        this.bestPosition = transform.position; // Initialize best position to current position
    }

    public void UpdatePosition(Vector3 targetPosition, List<Drone> allDrones, float inertia, float cognitiveWeight, float socialWeight, float avoidanceDistance)
    {
        // Update velocity based on PSO algorithm
        Vector3 directionToBest = bestPosition - transform.position;

        // Avoidance behavior
        foreach (var otherDrone in allDrones)
        {
            if (otherDrone != this && Vector3.Distance(transform.position, otherDrone.transform.position) < avoidanceDistance)
            {
                Vector3 awayFromOther = transform.position - otherDrone.transform.position;
                velocity += awayFromOther.normalized * 0.5f; // Adjust strength as needed
            }
        }

        // Update velocity
        velocity = inertia * velocity + cognitiveWeight * Random.value * directionToBest;

        // Update position
        transform.position += velocity * Time.deltaTime;

        // Update best position
        if (Vector3.Distance(transform.position, targetPosition) < Vector3.Distance(bestPosition, targetPosition))
        {
            bestPosition = transform.position;
        }
    }
}