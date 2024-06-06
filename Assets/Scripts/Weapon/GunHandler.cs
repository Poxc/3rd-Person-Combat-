using UnityEngine;

public class GunHandler : MonoBehaviour
{
    AnimatorManager animatorManager;

    public WeaponData weaponData;
    public ParticleSystem hitParticle;
    public ParticleSystem muzzleFlash;
    public GameObject gunBarrel;
    public GameObject hitscanWeapon;

    private float nextTimeToFire = 0f;

    private void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();

    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / weaponData.fireRate;
            Shoot();
        }

        if (hitscanWeapon.activeSelf)
        {
            animatorManager.PlayTargetAnimation("Aim", true);
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        Ray weaponRay = new Ray(gunBarrel.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        

        if (Physics.Raycast(weaponRay, out hit, weaponData.range, ~weaponData.IgnoreLayers))
        {
            Debug.Log("Hit " + hit.transform.name + "!");
            hitParticle.Play();

            hitParticle.transform.position = hit.point;
            hitParticle.transform.forward = hit.normal;
            hitParticle.transform.Translate(hit.normal.normalized * 0.1f);

            HandleEnemyHit(hit);
        }
    }

    void HandleEnemyHit(RaycastHit hit)
    {
        EnemyHealth targetHealth = hit.transform.GetComponent<EnemyHealth>();
        if (targetHealth != null)
        {
            Debug.Log("Hit enemy: " + hit.transform.name);
            targetHealth.TakeDamage(weaponData.damage);
        }
    }
}
