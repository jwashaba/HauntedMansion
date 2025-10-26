using UnityEngine;
using System.Collections.Generic;

public class PlayerPowerManager : MonoBehaviour
{
    public static PlayerPowerManager Instance; // singleton instance

    public List<CursedPower> activePowers = new List<CursedPower>();
    
    private PlayerMovement movement;
    private PlayerHealth health;
    private Weapon weapon;

    private float baseMoveSpeed;
    private float baseDashSpeed;
    private float baseDashCooldown;
    private float baseMaxHealth;
    private float baseAttackDamage;
    private float baseAttackCooldown;

    void Awake()
    {
        // Setup singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        weapon = GetComponentInChildren<Weapon>();

        baseMoveSpeed = movement.moveSpeed;
        baseDashSpeed = movement.dashSpeed;
        baseDashCooldown = movement.dashCooldown;
        baseMaxHealth = health.maxHealth;
        baseAttackDamage = weapon.damage;
        baseAttackCooldown = movement.attackCooldown;
    }

    public void AddPower(CursedPower power)
    {
        activePowers.Add(power);
        ApplyAllPowers();
    }

    public void ApplyAllPowers()
    {
        float moveSpeed = baseMoveSpeed;
        float dashSpeed = baseDashSpeed;
        float dashCooldown = baseDashCooldown;
        float maxHealth = baseMaxHealth;
        float attackDamage = baseAttackDamage;
        float attackCooldown = baseAttackCooldown;

        foreach (var p in activePowers)
        {
            moveSpeed *= p.speedMultiplier;
            dashSpeed *= p.speedMultiplier;
            dashCooldown *= p.cooldownMultiplier;
            maxHealth *= p.healthMultiplier;
            attackDamage *= p.damageMultiplier;
            attackCooldown *= p.cooldownMultiplier;
        }

        // Apply to player
        movement.moveSpeed = moveSpeed;
        movement.dashSpeed = dashSpeed;
        movement.dashCooldown = dashCooldown;

        // Adjust health proportionally
        float healthPercent = health.currentHealth / health.maxHealth;
        health.maxHealth = maxHealth;
        health.currentHealth = maxHealth * healthPercent;

        weapon.damage = attackDamage;
        movement.attackCooldown = attackCooldown;
    }
}
