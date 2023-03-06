using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float turnSpeed; // degrees/s
    [SerializeField]
    private float jumpHeight; // m
    [SerializeField]
    private float jumpHorizontalSpeed;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float knockbackTime;
    [SerializeField]
    private float jumpGracePeriod;
    [SerializeField]
    private float fallThreshold;
    private float yVelocity;
    private CharacterController controller;
    private Animator animator;
    private float totalStunTime;
    private Vector3 knockbackVelocity;
    private GameObject weapon;

    // making these variables nullable prevents the player from constantly jumping.
    private float? lastGroundedTime;
    private float? lastJumpRequestTime;
    private float stepOffset;
    private bool isJumping;
    private bool isGrounded;
    private float maxSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.controller = this.GetComponent<CharacterController>();
        this.stepOffset = controller.stepOffset;
    }

    /**
    * Returns the users input movement direction as a normalized 
    * vector, and outputs the original magnitude clamped [0,1]
    */
    Vector3 GetMovementInput(out float magnitude)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontal, 0, vertical);
        // get clamped magnitude and allow keyboard walk
        magnitude = Mathf.Clamp01(input.magnitude);
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) magnitude *= 0.5f;
        // normalize movement direction
        input.Normalize();
        return input;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackTime <= 0)
        {
            /* Movement Input */
            float inputMagnitude;
            Vector3 inputDirection = this.GetMovementInput(out inputMagnitude);

            float targetSpeed = inputMagnitude * this.maxSpeed;
            this.animator.SetFloat("X Velocity", inputDirection.x * targetSpeed, 0.05f, Time.deltaTime);
            this.animator.SetFloat("Z Velocity", inputDirection.z * targetSpeed, 0.05f, Time.deltaTime);

            /* Jumping */
            if (this.controller.isGrounded)
            {
                this.lastGroundedTime = Time.time;
                // stop accumulation of gravity
                if (this.yVelocity < 0f) this.yVelocity = 0f;
            }
            if (Input.GetButtonDown("Jump")) this.lastJumpRequestTime = Time.time;

            // player is on the ground (within grace period)
            if (Time.time - this.lastGroundedTime <= this.jumpGracePeriod)
            {
                this.controller.stepOffset = this.stepOffset;
                this.isGrounded = true;
                this.isJumping = false;
                this.animator.SetBool("Is Falling", false);

                if (Time.time - this.lastJumpRequestTime <= this.jumpGracePeriod)
                {
                    this.yVelocity = Movement.Jump(jumpHeight, Physics.gravity.y);
                    this.isJumping = true;
                    this.lastJumpRequestTime = null;
                    this.lastGroundedTime = null;
                }
            }
            else
            {
                this.controller.stepOffset = 0;
                this.isGrounded = false;

                if ((this.isJumping && this.yVelocity <= 0) || this.yVelocity < this.fallThreshold)
                {
                    this.animator.SetBool("Is Falling", true);
                }
            }

            // update animator params
            this.animator.SetBool("Is Jumping", this.isJumping);
            this.animator.SetBool("Is Grounded", this.isGrounded);
            this.animator.SetBool("Is Moving", inputDirection != Vector3.zero);

            // add gravity
            this.yVelocity += Physics.gravity.y * Time.deltaTime;

            // update rotation
            Quaternion cameraRotation = Quaternion.AngleAxis(this.cameraTransform.eulerAngles.y, Vector3.up);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, cameraRotation, this.turnSpeed * Time.deltaTime);

            if (this.isGrounded == false)
            {
                Vector3 velocity = (cameraRotation * inputDirection) * (inputMagnitude * this.jumpHorizontalSpeed);
                velocity.y = this.yVelocity;
                this.controller.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            // this.GetComponent<CharacterController>().Move(this.knockbackVelocity * Time.deltaTime * knockbackTime / totalStunTime);
            knockbackTime -= Time.deltaTime;

        }
    }

    /**
        Player movement uses root motion of the animation 
        so that there is no apparent sliding motion
    */
    private void OnAnimatorMove()
    {
        // because the in-air animations have no xz velocity
        // we disable root motion during jumps/fall
        if (this.isGrounded)
        {
            Vector3 velocity = this.animator.deltaPosition;
            velocity.y = this.yVelocity * Time.deltaTime;
            this.controller.Move(velocity);
        }
    }

    public void Knockback(float knockback, Vector3 direction, float knockbackStunTime)
    {

        this.knockbackTime = knockbackStunTime;
        this.totalStunTime = knockbackStunTime;
        this.knockbackVelocity = direction * knockback;
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

