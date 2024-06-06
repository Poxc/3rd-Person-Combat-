using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public WeaponData weaponData;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weaponData.projectileDamage);
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
