using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 ForwardBack(float walkSpeed, float direction)
    {
        Vector3 movement = direction*this.transform.forward * walkSpeed;
        return movement;
    }

    public Vector3 Strafe(float walkSpeed,float direction) {
        Vector3 movement = direction * this.transform.right * walkSpeed;
        return movement;
    }
    public float Jump(float jumpHeight,float gravity) {
        float yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        return yVelocity;
    }
}
