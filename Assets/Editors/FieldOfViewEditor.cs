using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        //draws the a circle around anything using FOV, that the maximum FOV of the object
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);
        Vector3 leftViewAngle = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 rightViewAngle = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + leftViewAngle * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + rightViewAngle * fov.radius);
        if (fov.canSeePlayer) {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.playerReference.transform.position);
        }
    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
