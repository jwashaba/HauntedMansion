using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    // public float damage = 1;
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Enemy enemy = collision.GetComponent<Enemy>();
    //     if (enemy != null)
    //     {
    //         Debug.Log("Hit");
    //         enemy.takeDamage(damage);
    //     }
    // }

    public float damage = 1f;

    // Track which enemies have been hit during the current active state
    private HashSet<Enemy> hitEnemiesThisActivation = new HashSet<Enemy>();

    private void OnEnable()
{
    // Clear hit tracking
    hitEnemiesThisActivation.Clear();

    // Pre-allocate collider array
    Collider2D[] hits = new Collider2D[10];

    // Setup contact filter
    ContactFilter2D filter = new ContactFilter2D();
    filter.ClearLayerMask(); // include all layers
    filter.useTriggers = true; // include triggers

    // Use the new static method instead of the obsolete instance method
    int numHits = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);

    for (int i = 0; i < numHits; i++)
    {
        Collider2D col = hits[i];
        Enemy enemy = col.GetComponentInParent<Enemy>();
        if (enemy != null && !hitEnemiesThisActivation.Contains(enemy))
        {
            enemy.takeDamage(damage);
            hitEnemiesThisActivation.Add(enemy);
            Debug.Log("Hit enemy immediately on enable: " + col.name);
        }
    }
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return; // only count hits while active

        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null && !hitEnemiesThisActivation.Contains(enemy))
        {
            enemy.takeDamage(damage);
            hitEnemiesThisActivation.Add(enemy);
            Debug.Log("Hit enemy on trigger enter: " + collision.name);
        }
    }

    private void OnDisable()
    {
        // Clear hit list when hitbox is turned off, ready for next activation
        hitEnemiesThisActivation.Clear();
    }
}
