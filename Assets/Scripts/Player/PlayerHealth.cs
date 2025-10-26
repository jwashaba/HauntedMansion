using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this to damage the player
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Optional: play hit animation/sound here
        Debug.Log("Player hit! Current health: " + currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    // Optional: call this to heal the player
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player healed! Current health: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add death behavior: respawn, game over screen, disable controls, etc.
        GameManagerScript.Instance.LoadDeathScene();
        gameObject.SetActive(false);
    }
}