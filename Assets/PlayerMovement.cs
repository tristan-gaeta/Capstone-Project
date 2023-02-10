using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5; // m/s
    public float turnSpeed = 90; // degrees/s
    public float jumpHeight = 1; // m
    public float gravity = -9.807f; // earth's gravitational constant
    public Vector3 velocity;
    public CharacterController controller;
    public Movement movementHelper;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.movementHelper = GetComponent<Movement>();

    }

    // Update is called once per frame
    void Update()
    {
        // stop accumulation of gravity
        if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;

        // rotation
        //float rotation = Input.GetAxis("Horizontal");
       // if (rotation != 0)
        //{
          //  this.transform.Rotate(Vector3.up, rotation * this.turnSpeed * Time.deltaTime);
        //}

        // walking
        Vector3 movement = new Vector3(0,0,0);
        if (Input.GetAxis("Vertical")!=0)
        {
            movement += movementHelper.ForwardBack(this.walkSpeed, Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            movement += movementHelper.Strafe(this.walkSpeed, Input.GetAxis("Horizontal"));
        }
        //Vector3 movement = Input.GetAxis("Vertical") * this.transform.forward * this.walkSpeed;

        // jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            this.velocity.y = movementHelper.Jump(jumpHeight,gravity);
        }

        this.velocity.y += this.gravity * Time.deltaTime;
        controller.Move((movement + this.velocity) * Time.deltaTime);
    }
}
