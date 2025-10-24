using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 1f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("Rigidbody2D missing on bullet!");
        }
    }

    public void Shoot(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.linearVelocity = direction * speed; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}