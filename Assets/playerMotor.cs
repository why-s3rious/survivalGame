using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class playerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Rigidbody rb;

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private GameObject player;


    private void Start()
    {
        rb= GetComponent<Rigidbody>();
    }
    public void move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void rotate(Vector3 _rotate)
    {
        rotation = _rotate;
    }


    public void RotateCamera(Vector3 _rotateCamera)
    {
        cameraRotation = _rotateCamera;
    }
    // Run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); 
        }

       
    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
       if(camera != null)
        {
            camera.transform.Rotate(-cameraRotation);
            
        }
    }
}
