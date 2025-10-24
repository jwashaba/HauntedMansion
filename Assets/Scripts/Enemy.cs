using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health, maxHealth = 3f;

    // Bullet variables
    public GameObject bulletPrefab;
    public Transform firePoint; // empty child where bullets spawn
    public float bulletSpeed = 5f;
    public float minFireRate = 1f; // minimum seconds between shots
    public float maxFireRate = 3f; // maximum seconds between shots
    private float fireCooldown;

    private Transform player;
    

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fireCooldown = Random.Range(minFireRate, maxFireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;

        fireCooldown -= Time.deltaTime;
        if(fireCooldown <= 0f)
        {
            ShootAtPlayer();
            // Reset cooldown to a new random value
            fireCooldown = Random.Range(minFireRate, maxFireRate);
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {

            Destroy(gameObject);
        }
    }
    
    void ShootAtPlayer()
    {
        if(bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        if(b != null)
        {
            b.Shoot(player.position);
        }
    }
}
