using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 locA, locB;
    public float travelTime;

    // Update is called once per frame
    void FixedUpdate()
    {
        float interpolation = Time.time % (2f * travelTime);

        if (interpolation < travelTime)
        {
            interpolation /= travelTime;
        }
        else
        {
            interpolation = 1 - (interpolation - travelTime) / travelTime;
        }
        this.transform.position = Vector3.Lerp(locA, locB, interpolation);
    }

    void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(this.transform);
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
