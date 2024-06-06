using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public GameObject player;
    public WeaponData weaponData;
    public GameObject impactEffectPrefab;
    public GameObject hitboxPrefab;
    private Animator animator;
    private bool isAttacking;
    private bool canDamageEnemy = true;

    private void Start()
    {
        if (player == null)
            return;

        animator = player.GetComponent<Animator>();
        if (animator == null)
            return;
    }
    private void Update()
    {
        isAttacking = animator.GetBool("isAttacking");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            hitboxPrefab.SetActive(true);
            if (impactEffectPrefab != null)
            {
                GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
                Destroy(impactEffect, 3f);
            }

            if (canDamageEnemy)
            {
                Debug.Log("Attacked!");
                if (other.CompareTag("Enemy"))
                {
                    canDamageEnemy = false;
                    StartCoroutine(DamageCooldown());

                    EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(weaponData.handheldDamage);
                    }

                    isAttacking = false;
                }
            }
            Invoke("DisableHitbox", 1.4f);
        }
    }

    private void DisableHitbox()
    {
        hitboxPrefab.SetActive(false);
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(weaponData.coolDown);
        canDamageEnemy = true;
    }
}
