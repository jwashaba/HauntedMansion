using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;
    private HashSet<EnemyBase> hitEnemiesThisActivation = new HashSet<EnemyBase>();

    private void OnEnable()
    {
        hitEnemiesThisActivation.Clear();

        Collider2D[] hits = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true
        };

        int numHits = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);

        for (int i = 0; i < numHits; i++)
        {
            EnemyBase enemy = hits[i].GetComponentInParent<EnemyBase>();
            if (enemy != null && !hitEnemiesThisActivation.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                hitEnemiesThisActivation.Add(enemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        EnemyBase enemy = collision.GetComponentInParent<EnemyBase>();
        if (enemy != null && !hitEnemiesThisActivation.Contains(enemy))
        {
            enemy.TakeDamage(damage);
            hitEnemiesThisActivation.Add(enemy);
        }
    }

    private void OnDisable()
    {
        hitEnemiesThisActivation.Clear();
    }
}