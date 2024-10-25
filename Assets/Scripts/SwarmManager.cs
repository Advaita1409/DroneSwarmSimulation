using UnityEngine;
using System.Collections.Generic;

public class SwarmManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public int numberOfDrones = 10;
    public float spawnRadius = 10f; // Added Spawn Radius

    private List<DroneController> drones = new List<DroneController>();

    void Start()
    {
        SpawnDrones();
    }

    void SpawnDrones()
    {
        for (int i = 0; i < numberOfDrones; i++)
        {
            // Generate a random position within a sphere defined by spawnRadius
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = Mathf.Clamp(randomPosition.y, 0, spawnRadius); // Ensure drones spawn above ground

            GameObject droneObj = Instantiate(dronePrefab, randomPosition, Quaternion.identity);
            // Pass the drone object to the DroneController
            DroneController drone = droneObj.GetComponent<DroneController>();
            drone.Initialize(this); 
            drones.Add(drone);
        }
    }

    void Update()
    {
        UpdateSwarmBehavior();
    }

    void UpdateSwarmBehavior()
    {
        foreach (var drone in drones)
        {
           /* // Calculate swarm behaviors
            Vector3 separation = CalculateSeparation(drone);
            Vector3 alignment = CalculateAlignment(drone);
            Vector3 cohesion = CalculateCohesion(drone);

            // Apply behaviors
            drone.ApplyForce(separation * 1.5f);
            drone.ApplyForce(alignment * 1.0f);
            drone.ApplyForce(cohesion * 1.0f);*/
        }
    }

    Vector3 CalculateSeparation(DroneController drone)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (var other in drones)
        {
            if (other != drone)
            {
                float distance = Vector3.Distance(drone.transform.position, other.transform.position);
                if (distance < drone.detectionRadius)
                {
                    Vector3 diff = drone.transform.position - other.transform.position;
                    diff.Normalize();
                    diff /= distance;
                    steer += diff;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            steer /= count;
        }

        return steer;
    }

    Vector3 CalculateAlignment(DroneController drone)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var other in drones)
        {
            if (other != drone)
            {
                float distance = Vector3.Distance(drone.transform.position, other.transform.position);
                if (distance < drone.detectionRadius)
                {
                    sum += other.GetComponent<Rigidbody>().linearVelocity;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * drone.maxSpeed;
            Vector3 steer = sum - drone.GetComponent<Rigidbody>().linearVelocity;
            steer = Vector3.ClampMagnitude(steer, drone.steeringForce);
            return steer;
        }

        return Vector3.zero;
    }

    Vector3 CalculateCohesion(DroneController drone)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var other in drones)
        {
            if (other != drone)
            {
                float distance = Vector3.Distance(drone.transform.position, other.transform.position);
                if (distance < drone.detectionRadius)
                {
                    sum += other.transform.position;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= count;
            return (sum - drone.transform.position).normalized * drone.maxSpeed;
        }

        return Vector3.zero;
    }
}

