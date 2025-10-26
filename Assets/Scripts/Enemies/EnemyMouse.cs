using UnityEngine;

public class EnemyMouse : EnemyBase
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint; // where bullets spawn
    public float bulletSpeed = 5f;
    public float minFireRate = 1f; // minimum seconds between shots
    public float maxFireRate = 3f; // maximum seconds between shots

    private float fireCooldown;
    private Transform player;

    protected override void Awake()
    {
        base.Awake(); // sets currentHealth = maxHealth
    }

    private void Start()
    {
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

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} squeaked its last squeak!");
        base.Die(); // destroys the game object by default
    }
}