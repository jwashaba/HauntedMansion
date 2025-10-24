using UnityEngine;

public class EnemySheepDoll : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 6f;     // total health across both forms
    public float transformHealth = 3f; // when to transform
    private float currentHealth;

    [Header("Form References")]
    public GameObject sheep;      // "Sheep" child GameObject
    public GameObject wolfForm;   // "WolfForm" child GameObject

    private bool isTransformed = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (sheep != null) sheep.SetActive(true);
        if (wolfForm != null) wolfForm.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}");

        // Transform if health threshold reached and not yet transformed
        if (!isTransformed && currentHealth <= transformHealth)
        {
            TransformIntoWolf();
        }

        // Die when health reaches 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TransformIntoWolf()
    {
        isTransformed = true;

        if (sheep != null) sheep.SetActive(false);
        if (wolfForm != null) wolfForm.SetActive(true);

        Debug.Log($"{gameObject.name} has transformed into its wolf form!");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        Destroy(gameObject);
    }
}