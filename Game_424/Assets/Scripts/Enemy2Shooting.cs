using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Shooting : MonoBehaviour
{
    public Transform shootPoint;
    public LayerMask layerMask;
    public TrailRenderer bulletTrail;
    public Transform target;
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);

    public Enemy2References enemy2References;

    private void Awake()
    {
        enemy2References = GetComponent<Enemy2References>();
    }

    private bool canShoot = true; // Add a flag to control shooting rate
    public float shootCooldown = 0.5f; // Cooldown between shots

    public void Shoot()
    {
        if (!canShoot)
            return;

        Vector3 direction = GetDirection();

        if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
        {
            Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 10f, Color.red, 1f);

            // Rotate the enemy to look at the target before shooting
            transform.LookAt(target);

            for (int i = 0; i < 2; i++) // Fire two bullets
            {
                TrailRenderer trail = Instantiate(bulletTrail, shootPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            StartCoroutine(ShootCooldown()); // Start the cooldown
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }
}
