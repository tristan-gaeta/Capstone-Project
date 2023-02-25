using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float turnSpeed; // degrees/s
    [SerializeField]
    private float jumpHeight; // m
    [SerializeField]
    private float jumpHorizontalSpeed;
    private float yVelocity;
    private CharacterController controller;
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float knockbackTime;
    private Animator animator;
    private float totalStunTime;
    private Vector3 knockbackVelocity;
    private GameObject weapon;

    // making these variables nullable prevents the player from constantly jumping.
    private float? lastGroundedTime;
    private float? lastJumpRequestTime;
    [SerializeField]
    private float jumpGracePeriod;
    private float stepOffset;
    private bool isJumping;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.controller = this.GetComponent<CharacterController>();
        this.stepOffset = controller.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackTime <= 0)
        {
            // stop accumulation of gravity
            if (this.controller.isGrounded && this.yVelocity < 0f) this.yVelocity = 0f;
            // running
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float magnitude;

            Vector3 direction = Movement.GetDirectionWASD(horizontal, vertical, this.cameraTransform.rotation.eulerAngles.y, out magnitude);
            //walking
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) magnitude *= 0.5f;
            this.animator.SetFloat("Input Magnitude", magnitude, 0.05f, Time.deltaTime);

            //  *** Jumping ***
            if (this.controller.isGrounded) this.lastGroundedTime = Time.time;
            if (Input.GetButtonDown("Jump")) this.lastJumpRequestTime = Time.time;

            // player is on the ground (within grace period)
            if (Time.time - this.lastGroundedTime <= this.jumpGracePeriod)
            {
                this.controller.stepOffset = this.stepOffset;
                this.animator.SetBool("Is Grounded", true);
                this.isGrounded = true;
                this.animator.SetBool("Is Jumping", false);
                this.isJumping = false;
                this.animator.SetBool("Is Falling", false);

                if (Time.time - this.lastJumpRequestTime <= this.jumpGracePeriod)
                {
                    this.yVelocity = Movement.Jump(jumpHeight, Physics.gravity.y);
                    this.animator.SetBool("Is Jumping", true);
                    this.isJumping = true;
                    this.lastJumpRequestTime = null;
                    this.lastGroundedTime = null;
                }
            }
            else
            {
                this.controller.stepOffset = 0;
                this.animator.SetBool("Is Grounded", false);
                this.isGrounded = false;

                if ((this.isJumping && this.yVelocity <= 0) || this.yVelocity < -7)
                {
                    this.animator.SetBool("Is Falling", true);
                }
            }

            // add gravity
            this.yVelocity += Physics.gravity.y * Time.deltaTime;

            // update rotation
            if (direction != Vector3.zero)
            {
                this.animator.SetBool("Is Moving", true);
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, this.turnSpeed * Time.deltaTime);
            }
            else
            {
                this.animator.SetBool("Is Moving", false);

            }
            if (this.isGrounded == false)
            {
                Vector3 velocity = direction * (magnitude * this.jumpHorizontalSpeed);
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

