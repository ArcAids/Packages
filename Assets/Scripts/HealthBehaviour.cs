using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void OnDamageTaken(float Damage)
    {
        currentHealth-=Damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}


interface ITakeDamage
{
    void OnDamageTaken(float Damage);
}
