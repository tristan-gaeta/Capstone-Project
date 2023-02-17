using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContact : MonoBehaviour
{
    public float knockback;
    public float damage;
    public float stunTime;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            Vector3 direction =  other.transform.position-this.transform.position;
            Vector3 enemyKnockbackDirection = direction * -1;
            direction.y += 0.25f;
            otherGameObject.GetComponent<PlayerMovement>().Knockback(knockback, direction,stunTime);
            //pushes the enemy back after it hits the player, to prevent it comboing the player
            this.gameObject.GetComponent<BasicEnemyAI>().Knockback(knockback / 2, enemyKnockbackDirection, stunTime/2);
            DealDamage.DamageTarget(damage, other.gameObject);
        } else if (otherGameObject.CompareTag("Weapon")){
            Vector3 direction = this.transform.position - other.transform.position;
            //DealDamage.CauseKnockback(other.gameObject.GetComponent<Weapon>().knockback, direction, this.gameObject, other.gameObject.GetComponent<Weapon>().stunTime);
            //DealDamage.DamageTarget(other.gameObject.GetComponent<Weapon>().damage, this.gameObject);
        }
    }
}
