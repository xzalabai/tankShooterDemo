using UnityEngine;

public class HomingMissile : Missile
{
    [Tooltip("How quickly can missile perform turn")]
    private float rotateSpeed = 200f;        
    [Tooltip("Speed of missile")]
    private float speed = 5f;

    private GameObject target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // If target is already destroyed, missile is going to fall and destroy itself

        if (!target || !target.activeInHierarchy)
        {
            rb.useGravity = true;
            this.enabled = false;
            return;
        }

        DriveToTarget();
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void DriveToTarget()
    {
        // Missile is driven to target with a help of Cross (value that determines amount of rotation based on distance between 2 objects)

        Vector3 direction = (target.transform.position - rb.position);
        direction.Normalize();
        Vector3 rotateAmount = Vector3.Cross(direction, transform.forward);
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.forward * speed;
    }
}
