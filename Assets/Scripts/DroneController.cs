/*using UnityEngine;

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

    
}*/

using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        MoveDrone();
    }

    void MoveDrone()
    {
        // Get input from keyboard
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Move the drone forward/backward
        Vector3 movement = transform.forward * moveVertical * speed * Time.deltaTime;
        transform.position += movement;

        // Rotate the drone left/right
        float rotation = moveHorizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }
}