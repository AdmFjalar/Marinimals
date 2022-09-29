using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int points = 0;
    public int kills = 0;

    public bool isShielded = false;

    [SerializeField] private float shieldTime = 5;

    private bool healing = false;
    private float shieldTimer;

    private void Start()
    {
        isShielded = false;
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isShielded)
        {
            if (shieldTimer < 5)
            {
                shieldTimer += Time.deltaTime;
            }
            else if (shieldTimer >= 5)
            {
                shieldTimer = 0;
                isShielded = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (healing)
        {
            Heal(maxHealth);
            healing = false;
        }
    }

    /// <summary>
    /// Damages the entity.
    /// </summary>
    /// <param name="Damage"></param>
    public void TakeDamage(int Damage)
    {
        if (!isShielded)
        {
            Damage = Mathf.Clamp(Damage, 0, 1000000);
            health -= Damage;
            health = Mathf.Clamp(health, 0, maxHealth);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int HealingAmount)
    {
        HealingAmount = Mathf.Clamp(HealingAmount, 0, 1000000);
        health += HealingAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    /// <summary>
    /// "Kills" the entity.
    /// </summary>
    public virtual void Die()
    {
        //Die
        transform.parent.gameObject.SetActive(false);
        foreach (Gun g in transform.GetComponentsInChildren<Gun>())
        {
            g.transform.GetComponentInChildren<Animator>()?.ResetTrigger("Shoot");
        }
        GameManager.instance.FindPlayers();
        healing = true;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    TakeDamage(7);
    //}

    public void Shield()
    {
        isShielded = true;
    }
}
