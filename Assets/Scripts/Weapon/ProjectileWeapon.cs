using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    AnimatorManager animatorManager;

    public ParticleSystem muzzleFlash;
    public WeaponData weaponData;
    public GameObject projectileWeapon;


    public GameObject rocketPrefab;
    public Transform barrel;

    private float nextTimeToFire = 0f;

    private void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / weaponData.fireRate;
            Shoot();
        }

        if (projectileWeapon.activeSelf)
        {
            animatorManager.PlayTargetAnimation("Aim", true);
        }
    }

    void Shoot()
    {

        muzzleFlash.Play();

        GameObject rocket = Instantiate(rocketPrefab, barrel.position, barrel.rotation);

        Vector3 direction = Camera.main.transform.forward;

        ProjectileBase rocketController = rocket.GetComponent<ProjectileBase>();
        if (rocketController != null)
        {
            rocketController.Shoot(direction);
        }
    }
}