using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;
    public GameObject playerReference;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    public Vector3 lastPlayerPosition;
    public bool checkedLastPosition;
    // Start is called before the first frame update
    void Start()
    {
        checkedLastPosition = true;
        playerReference = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine() {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true) {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck() 
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0) 
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward,directionToTarget)<angle/2) 
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    lastPlayerPosition = target.position;
                }
                else 
                {
                    canSeePlayer = false;
                }
            } else 
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            checkedLastPosition = false;
        }
    }
}
