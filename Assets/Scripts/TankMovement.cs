using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{          
    [SerializeField]
    [Tooltip("Speed of a tank")]
    private float speedOfTank;  
    
    [SerializeField]
    [Tooltip("Rotation speed of a tank")]
    private float rotationSpeed; 

    private Rigidbody tankBody;

    void Start()
    {
        tankBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculates new position and rotation based on Input
        // If player moving BACK with tank, switch commands

        tankBody.MovePosition(tankBody.position + transform.forward * Input.GetAxis("Vertical") * speedOfTank);

        if (Input.GetAxis("Vertical") >= 0)
        {
            tankBody.MoveRotation(tankBody.rotation * Quaternion.Euler(0f, Input.GetAxis("Horizontal") * rotationSpeed, 0f));
        }
        else
        {
            tankBody.MoveRotation(tankBody.rotation * Quaternion.Euler(0f, (-1) * Input.GetAxis("Horizontal") * rotationSpeed, 0f));
        }
        
    }
}
