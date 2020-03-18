using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerMotor))]

public class playerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivily = 3f;
    private playerMotor motor;

    private void Start()
    {

        motor = GetComponent<playerMotor>();

    }
    private void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");


        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        //final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        //apply movement 
        motor.move(velocity);

        float yRot = Input.GetAxis("Mouse X");
       
        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSensitivily ;
        motor.rotate(_rotation);


        float xRot = Input.GetAxis("Mouse Y");

        Vector3 _cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensitivily;
        motor.RotateCamera(_cameraRotation);
    }

     
}
