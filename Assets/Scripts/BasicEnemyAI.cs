using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    FieldOfView fov;
    CharacterController controller;
    public float walkSpeed = 2.5f;
    public float gravity = -9.807f;
    public float jumpHeight = 1;
    Vector3 velocity;
    public Vector3 lastPosition;
    public float attemptJumpTime = 0.1f;
    private float time = 0.0f;

    private float knockbackTime;
    private float totalStunTime;
    private Vector3 knockbackVelocity;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.fov = GetComponent<FieldOfView>();
        this.lastPosition = this.transform.position;
        Vector3 velocity = new Vector3(0, 0, 0);
        this.knockbackTime = -1;
    }


    // Update is called once per frame
    void Update()
    {
        if (knockbackTime < 0)
        {

            if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;
            Vector3 movement = new Vector3(0, 0, 0);
            if (fov.canSeePlayer)
            {
                Vector3 targetFlatPosition = new Vector3(fov.playerReference.transform.position.x, this.transform.position.y, fov.playerReference.transform.position.z);
                this.transform.LookAt(targetFlatPosition);
                movement += Movement.ForwardBack(walkSpeed, 1, this.transform);
                if (this.time > attemptJumpTime)
                {
                    AttemptJump();
                    time = 0f;
                }
                time += Time.deltaTime;
            }
            this.velocity.y += this.gravity * Time.deltaTime;
            controller.Move((movement + this.velocity) * Time.deltaTime);
        }
        else
        {
            this.GetComponent<CharacterController>().Move(this.knockbackVelocity * Time.deltaTime * knockbackTime / totalStunTime);
            knockbackTime -= Time.deltaTime;
        }
    }
    private void AttemptJump()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        float speedSquared = Mathf.Pow(this.lastPosition.x - this.transform.position.x, 2) + Mathf.Pow(this.lastPosition.z - this.transform.position.z, 2);
        if (controller.isGrounded && speedSquared < 0.03)
        {
            this.velocity.y = Movement.Jump(jumpHeight, gravity);
        }
        controller.Move((movement + this.velocity) * Time.deltaTime);
        this.lastPosition = this.transform.position;
    }
    public void Knockback(float knockback, Vector3 direction, float knockbackStunTime)
    {

        this.knockbackTime = knockbackStunTime;
        this.totalStunTime = knockbackStunTime;
        this.knockbackVelocity = direction * knockback;
    }
}
