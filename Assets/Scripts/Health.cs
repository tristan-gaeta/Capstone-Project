using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int maxHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject gameoverScreen;
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (hearts.Length > 0)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
                if (i < maxHealth)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
            if (health <= 0)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                gameoverScreen.SetActive(true);
            }
        }
        else if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        health = health - damage;
    }
}
