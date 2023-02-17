

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public static void CauseKnockback(float knockback, Vector3 direction, GameObject target,float knockbackStunTime)
    {
        float knockbackTime = knockbackStunTime;
        Vector3 velocity = direction * knockback;
        while (knockbackTime > 0)
        {
            target.GetComponent<CharacterController>().Move(velocity * Time.deltaTime*knockbackTime/knockbackStunTime);
            knockbackTime -= Time.deltaTime;
        }
    }
    public static void DamageTarget(float damage, GameObject target)
    {

    }
}

