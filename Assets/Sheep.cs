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
    public GameObject bulletPrefab;
    public Transform firePoint; // empty child where bullets spawn
    public float bulletSpeed = 5f;
    public float minFireRate = 1f; // minimum seconds between shots
    public float maxFireRate = 3f; // maximum seconds between shots
    private float fireCooldown;

    private Transform player;

    private bool isTransformed = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (sheep != null) sheep.SetActive(true);
        if (wolfForm != null) wolfForm.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fireCooldown = Random.Range(minFireRate, maxFireRate);
    }

    void Update()
    {
        if (player == null) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            ShootAtPlayer();
            // Reset cooldown to a new random value
            fireCooldown = Random.Range(minFireRate, maxFireRate);
        }
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

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.Shoot(player.position);
        }
    }
    
    private void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        Destroy(gameObject);
    }
}