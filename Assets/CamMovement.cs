using UnityEngine;

public class CamMovement : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        this.transform.position = this.target.position + this.offset;
        this.transform.LookAt(this.target);
    }
}
