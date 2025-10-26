using UnityEngine;

[CreateAssetMenu(fileName = "NewCursedPower", menuName = "CursedPower")]
public class CursedPower : ScriptableObject
{
    [Header("Identity")]
    public string powerName;
    [TextArea] public string description;
    public Sprite icon;
    public string associatedDollName;

    [Header("Buffs")]
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float healthMultiplier = 1f;

    [Header("Debuffs")]
    public float defenseMultiplier = 1f;
    public float cooldownMultiplier = 1f;

    [Header("Optional Active Ability")]
    public GameObject activeAbilityPrefab;  // e.g. a dash, burst, heal effect
}