using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float walkDistance = 5f;
    public float fireRate = 1f;

    public ParticleSystem muzzleFlash;
    public GameObject rocketPrefab;
    public Transform barrel;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToStart = true;
    private float nextTimeToFire = 0f;
    private Transform player;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * walkDistance;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Move();

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Move()
    {
        Vector3 destination = movingToStart ? startPosition : targetPosition;
        transform.position = Vector3.MoveTowards(transform.position, destination, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            movingToStart = !movingToStart;
        }
    }

    void Shoot()
    {
        if (player != null)
        {
            muzzleFlash.Play();

            GameObject rocket = Instantiate(rocketPrefab, barrel.position, barrel.rotation);

            Vector3 direction = (player.position - barrel.position).normalized;

            EnemyWeapon rocketController = rocket.GetComponent<EnemyWeapon>();
            if (rocketController != null)
            {
                rocketController.Shoot(direction);
            }
        }
    }
}
