using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public float knockback;
    public int damage;
    public float stunTime;
    public float swingSpeed;
    public float weaponSize;
    private bool canAttack;
    public GameObject Sword;
    public float attackCooldown;
    void Start()
    {
        this.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) {
            SwordAttack();
        }
    }
    public void SwordAttack() {
        canAttack = false;
        Animator weaponAnimation = this.GetComponent<Animator>();
        weaponAnimation.SetTrigger("Attack");
        StartCoroutine(ResetWeapon(weaponAnimation));

    }
    IEnumerator ResetWeapon(Animator weaponAnimation) 
    {
    yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        weaponAnimation.ResetTrigger("Attack");
        {
    };
    }
}
