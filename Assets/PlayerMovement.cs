using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed = 5; // m/s

    [SerializeField]
    private float turnSpeed = 90; // degrees/s
    [SerializeField]
    private float jumpHeight = 1; // m
    [SerializeField]
    private float gravity = -9.807f; // earth's gravitational constant
    private float yVelocity;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Transform cameraTransform;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // stop accumulation of gravity
        if (this.controller.isGrounded && this.yVelocity < 0) this.yVelocity = 0f;

        // walking
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed;

        Vector3 direction = Movement.GetDirectionWASD(horizontal, vertical, this.cameraTransform.rotation.eulerAngles.y, out speed);
        speed *= this.walkSpeed;

        // jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            this.yVelocity = Movement.Jump(jumpHeight, gravity);
        }


        // add gravity
        this.yVelocity += this.gravity * Time.deltaTime;

        // apply movement
        Vector3 velocity = direction * speed;
        velocity.y = this.yVelocity;
        this.controller.Move(velocity * Time.deltaTime);

        // update rotation
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, this.turnSpeed * Time.deltaTime);
        }
    }


    // hide cursor on screen focus 
    void OnApplicationFocus(bool focused)
    {
        if (focused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
