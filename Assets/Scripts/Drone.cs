using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Swarm Settings")]
    public float maxSpeed = 10f;
    public float separationDistance = 5f;
    public float alignmentDistance = 10f;
    public float cohesionDistance = 15f;

    [Header("Force Weights")]
    public float separationWeight = 1.5f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float obstacleAvoidanceWeight = 2.0f;

    private Vector3 velocity;
    private Vector3 acceleration;

    void Start()
    {
        velocity = Random.insideUnitSphere * maxSpeed;
    }

    void Update()
    {
        Vector3 separation = Separate();
        Vector3 alignment = Align();
        Vector3 cohesion = Cohere();
        Vector3 obstacleAvoidance = AvoidObstacles();

        acceleration = (separation * separationWeight +
                       alignment * alignmentWeight +
                       cohesion * cohesionWeight +
                       obstacleAvoidance * obstacleAvoidanceWeight);

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    private Vector3 Separate()
{
    Vector3 steer = Vector3.zero;
    int count = 0;
    
    Collider[] nearbyDrones = Physics.OverlapSphere(transform.position, separationDistance);
    
    foreach (Collider other in nearbyDrones)
    {
        if (other.gameObject != gameObject && other.CompareTag("Drone"))
        {
            Vector3 diff = transform.position - other.transform.position;
            float distance = diff.magnitude;
            
            // Weight by distance
            diff.Normalize();
            diff /= distance;
            steer += diff;
            count++;
        }
    }
    
    if (count > 0)
    {
        steer /= count;
    }
    
    if (steer.magnitude > 0)
    {
        steer.Normalize();
        steer *= maxSpeed;
        steer -= velocity;
    }
    
    return steer;
}
   private Vector3 Align()
{
    Vector3 sum = Vector3.zero;
    int count = 0;
    
    Collider[] nearbyDrones = Physics.OverlapSphere(transform.position, alignmentDistance);
    
    foreach (Collider other in nearbyDrones)
    {
        if (other.gameObject != gameObject && other.CompareTag("Drone"))
        {
            Drone drone = other.GetComponent<Drone>();
            sum += drone.velocity;
            count++;
        }
    }
    
    if (count > 0)
    {
        sum /= count;
        sum.Normalize();
        sum *= maxSpeed;
        Vector3 steer = sum - velocity;
        return steer;
    }
    
    return Vector3.zero;
}

private Vector3 Cohere()
{
    Vector3 sum = Vector3.zero;
    int count = 0;
    
    Collider[] nearbyDrones = Physics.OverlapSphere(transform.position, cohesionDistance);
    
    foreach (Collider other in nearbyDrones)
    {
        if (other.gameObject != gameObject && other.CompareTag("Drone"))
        {
            sum += other.transform.position;
            count++;
        }
    }
    
    if (count > 0)
    {
        sum /= count;
        return Seek(sum);
    }
    
    return Vector3.zero;
}

private Vector3 Seek(Vector3 target)
{
    Vector3 desired = target - transform.position;
    desired.Normalize();
    desired *= maxSpeed;
    
    Vector3 steer = desired - velocity;
    return steer;
}
   
   private Vector3 AvoidObstacles()
{
    Vector3 avoidanceForce = Vector3.zero;
    float rayLength = 10f;
    
    RaycastHit hit;
    Vector3[] rayDirections = new Vector3[]
    {
        transform.forward,
        Quaternion.Euler(0, 45, 0) * transform.forward,
        Quaternion.Euler(0, -45, 0) * transform.forward,
        Quaternion.Euler(45, 0, 0) * transform.forward,
        Quaternion.Euler(-45, 0, 0) * transform.forward
    };
    
    foreach (Vector3 direction in rayDirections)
    {
        if (Physics.Raycast(transform.position, direction, out hit, rayLength))
        {
            if (!hit.collider.CompareTag("Drone"))
            {
                avoidanceForce += (transform.position - hit.point).normalized;
            }
        }
    }
    
    return avoidanceForce;
}
}