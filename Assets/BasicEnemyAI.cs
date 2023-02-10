using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    FieldOfView fov;
    CharacterController controller;
    public float walkSpeed = 2.5f;
    public Movement movementHelper;
    public Vector3 velocity;
    public float gravity = -9.807f;
    public float jumpHeight = 1;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.movementHelper = GetComponent<Movement>();
        this.fov = GetComponent<FieldOfView>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;
        Vector3 movement = new Vector3(0, 0, 0);
        if (fov.canSeePlayer) {
            Vector3 targetFlatPosition = new Vector3(fov.playerReference.transform.position.x, this.transform.position.y, fov.playerReference.transform.position.z);
            this.transform.LookAt(targetFlatPosition);
            movement+=movementHelper.ForwardBack(walkSpeed, 1);
            if (controller.isGrounded&&this.transform.position.y+0.5< fov.playerReference.transform.position.y) {
                this.velocity.y = movementHelper.Jump(jumpHeight, gravity);
            }
            
        }
        this.velocity.y += this.gravity * Time.deltaTime;
        controller.Move((movement + this.velocity) * Time.deltaTime);
    }
}
