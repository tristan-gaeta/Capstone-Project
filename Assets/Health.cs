using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int maxHealth;
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        print("I've taken damage");
        health = health- damage;
    }
}
