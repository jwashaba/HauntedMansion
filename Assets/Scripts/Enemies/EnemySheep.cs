using UnityEngine;

public class EnemySheepDoll : EnemyBase
{
    [Header("Transformation Settings")]
    public float transformHealth = 3f; // Health threshold for transformation
    public GameObject sheep;           // "Sheep" form child
    public GameObject wolfForm;        // "Wolf" form child
    private bool isTransformed = false;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float minFireRate = 1f;
    public float maxFireRate = 3f;
    private float fireCooldown;

    private Transform player;

    // Override Awake to keep base setup for currentHealth
    protected override void Awake()
    {
        base.Awake(); // sets currentHealth = maxHealth
    }

    private void Start()
    {
        if (sheep != null) sheep.SetActive(true);
        if (wolfForm != null) wolfForm.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        fireCooldown = Random.Range(minFireRate, maxFireRate);
    }

    private void Update()
    {
        if (player == null) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            ShootAtPlayer();
            fireCooldown = Random.Range(minFireRate, maxFireRate);
        }
    }

    // Override TakeDamage so we can add transformation logic
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); // handles reducing HP and dying

        if (!isTransformed && currentHealth <= transformHealth)
        {
            TransformIntoWolf();
        }
    }

    private void TransformIntoWolf()
    {
        isTransformed = true;

        if (sheep != null) sheep.SetActive(false);
        if (wolfForm != null) wolfForm.SetActive(true);

        Debug.Log($"{gameObject.name} transformed into wolf form!");
    }

    private void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.Shoot(player.position);
        }
    }

    // Optional: override Die for special effects
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        base.Die(); // destroys the GameObject by default
    }
}