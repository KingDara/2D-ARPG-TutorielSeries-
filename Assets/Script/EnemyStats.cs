using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public int damage;
    protected int currentHealth;
    public int maxHealth;

    public GameObject healthBar;
    public Image life;

    public void InitilizeBar()
    {
        this.currentHealth = this.maxHealth;
        life.fillAmount = this.maxHealth;
    }

    public void UpdateHealthBar(int value)
    {
        life.fillAmount = (float)value / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Animator anim;
        anim = GetComponent<Animator>();
        anim.SetTrigger("Hit");
        UpdateHealthBar(currentHealth);
        healthBar.SetActive(true);
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
