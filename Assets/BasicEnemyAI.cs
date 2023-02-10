using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    FieldOfView fov;
    CharacterController controller;
    public float walkSpeed = 2;
    public Movement movementHelper;
    public Vector3 velocity;
    public float gravity = -9.807f;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.movementHelper = GetComponent<Movement>();
        this.fov = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;
        Vector3 movement = new Vector3(0, 0, 0);
        if (fov.canSeePlayer) {
            this.transform.LookAt(fov.playerReference.transform);
            movement+=movementHelper.ForwardBack(walkSpeed, 1);
        }
        this.velocity.y += this.gravity * Time.deltaTime;
        controller.Move((movement + this.velocity) * Time.deltaTime);
    }
}
