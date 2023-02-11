using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 ForwardBack(float walkSpeed, float direction)
    {
        Vector3 movement = direction * this.transform.forward * walkSpeed;
        return movement;
    }

    public Vector3 Strafe(float walkSpeed, float direction)
    {
        Vector3 movement = direction * this.transform.right * walkSpeed;
        return movement;
    }

    public static Vector3 GetDirectionWASD(float horizontal, float vertical, float yRotation, out float magnitude)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        magnitude = Mathf.Clamp01(movement.magnitude);
        movement = Quaternion.AngleAxis(yRotation, Vector3.up) * movement;
        movement.Normalize();
        return movement;
    }

    public static float Jump(float jumpHeight, float gravity)
    {
        float yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        return yVelocity;
    }
}
