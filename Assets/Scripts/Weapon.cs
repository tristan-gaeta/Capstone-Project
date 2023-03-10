using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public float knockback;
    public int damage;
    public float stunTime;
    public GameObject parent;

    private void Start()
    {
    }
    private void Update()
    {
            this.GetComponent<BoxCollider>().enabled = parent.GetComponent<Animator>().GetBool("Is Attacking");
    }
}

