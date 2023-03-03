using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    FieldOfView fov;
    CharacterController controller;
    public float walkSpeed = 2.5f;
    public float gravity = -9.807f;
    public float jumpHeight = 1;
    Vector3 velocity;
    public Rigidbody rigidbody;
    public Vector3 lastPosition;
    public float attemptJumpTime = 0.1f;
    public float timeToChooseDirection = 1.75f;
    private float time = 0.0f;
    NavMeshPath path;
    private float knockbackTime;
    private float totalStunTime;
    private Vector3 knockbackVelocity;
    public float timeUntilNewDirection;
    private bool directionChosen = false;
    private float rewritePathTime;
    private int pathIndex;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.fov = GetComponent<FieldOfView>();
        rigidbody = GetComponent<Rigidbody>();
        this.lastPosition = this.transform.position;
        timeUntilNewDirection = 0f;
        rewritePathTime = 0.5f;
        pathIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.speed = walkSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (knockbackTime < 0)
        {
            MakeDecision();
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
    private void MakeDecision() {
        int health = this.gameObject.GetComponent<Health>().health;
        int maxHealth = this.gameObject.GetComponent<Health>().maxHealth;
        bool afraid = health <= maxHealth / 2;
        if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;
        Vector3 movement = new Vector3(0, 0, 0);
        if (!afraid)
        {
            if (fov.canSeePlayer)
            {
                CalculatePath(fov.playerReference.transform.position);
                if (path != null && path.corners.Length > 0)
                {
                    FollowPath();
                    movement += Movement.ForwardBack(walkSpeed, 1, this.transform);

                }
                //Vector3 targetFlatPosition = new Vector3(fov.playerReference.transform.position.x, this.transform.position.y, fov.playerReference.transform.position.z);
                //this.transform.LookAt(targetFlatPosition);
                //movement += Movement.ForwardBack(walkSpeed, 1, this.transform);
                //if (this.time > attemptJumpTime)
                //{
                //    AttemptJump();
                //    time = 0f;
                //}
                //time += Time.deltaTime;
            }
            else if (!fov.checkedLastPosition)
            {
                CalculatePath(fov.lastPlayerPosition);
                if (path != null && path.corners.Length > 0)
                {
                    FollowPath();
                    movement += Movement.ForwardBack(walkSpeed, 1, this.transform);
                }
                fov.checkedLastPosition = 2f>Vector3.Distance(fov.lastPlayerPosition, transform.position);
            }
            else
            {
                IdleMovement(movement);

            }
        }
        else
        {
            RunAway();
            movement += Movement.ForwardBack(walkSpeed, 1, this.transform);
        }
        this.velocity.y += this.gravity * Time.deltaTime;
        controller.Move( (movement+this.velocity) * Time.deltaTime);
    }
    private void IdleMovement(Vector3 movement) {
        if (timeUntilNewDirection < 0)
        {
            if (!directionChosen)
            {
                this.transform.rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);
                timeUntilNewDirection = timeToChooseDirection;
                directionChosen = true;
            }
        }
        else
        {
            directionChosen = false;
            timeUntilNewDirection -= Time.deltaTime;
            movement += Movement.ForwardBack(walkSpeed, 1, this.transform);
        }
    }
    private void CalculatePath(Vector3 destination) {
        rewritePathTime += Time.deltaTime;
        if (rewritePathTime > 0.5f)
        {
            bool pathFound = NavMesh.CalculatePath(this.gameObject.transform.position, destination, NavMesh.AllAreas, path);
            rewritePathTime -= 0.5f;
            pathIndex = 0;
        }
    }
    private void FollowPath() {
        agent.SetPath(path);
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        Vector3 targetDir = agent.steeringTarget;
        this.transform.LookAt(new Vector3(targetDir.x, this.transform.position.y, targetDir.z));
    }
    private void RunAway() {
        Debug.Log("I need to get out of here!2");
        float playerX = fov.playerReference.transform.position.x;
        float playerZ = fov.playerReference.transform.position.z;
        float enemyX = this.transform.position.x;
        float enemyZ = this.transform.position.z;


        CalculatePath(new Vector3(enemyX+(enemyX-playerX), this.transform.position.y, enemyZ + (enemyZ - playerZ)));
        FollowPath();
    }
}

