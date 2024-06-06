using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon Data", order = 51)]
public class WeaponData : ScriptableObject
{
    [Header("HandheldWeapon")]
    public float handheldFireRate = 0.5f;
    public int handheldDamage = 20;
    public float nextFireTime = 0f;
    public float coolDown = 0.1f;
    public float comboWindow = 0.4f;

    [Header("HitscanWeapon")]
    public float fireRate = 5f;
    public float nextTimeToFire = 0f;
    public int damage = 10;  
    public float range = 100f;   
    public LayerMask IgnoreLayers;

    [Header("ProjectileWeapon")]
    public float projectileFireRate = 0.5f;
    public int projectileDamage = 90;
    public float projectileRange = 50f;
    public float projectileSpeed = 10f;
    public LayerMask projectileIgnoreLayers;
}

