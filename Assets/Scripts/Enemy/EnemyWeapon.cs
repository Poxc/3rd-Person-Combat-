using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int projectileDamage = 20; 
    public GameObject hitParticle;

    public float rocketSpeed = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Shoot(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * rocketSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(projectileDamage);
            }
        }

        if (hitParticle != null)
        {
            GameObject impactEffect = Instantiate(hitParticle, transform.position, Quaternion.identity);

            Destroy(impactEffect, 3f);
        }
        Destroy(gameObject);
    }
}
