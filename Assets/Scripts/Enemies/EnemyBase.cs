using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float maxHealth = 5f;
    protected float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage, {currentHealth} HP left");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died");
        Destroy(gameObject);
    }
}