using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Drone Properties")]
    public float maxSpeed = 10f;
    public float steeringForce = 5f;
    public float detectionRadius = 5f;
    
    private Vector3 velocity;
    private Vector3 acceleration;
    
    void Start()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }

    void Update()
    {
        // Apply basic physics
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        
        // Reset acceleration
        acceleration = Vector3.zero;
        
        // Look in the direction of movement
        if (velocity != Vector3.zero)
        {
            transform.forward = velocity.normalized;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }
}