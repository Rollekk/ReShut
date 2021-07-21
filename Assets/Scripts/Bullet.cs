using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    [Header("Components")]
    public GunController gunController;
    public TrailController trailController;

    private TrailRenderer trailRenderer;
    private Camera playerCamera;

    private RaycastHit bulletHit;

    [Header("Bullet")]
    public float bulletSpeed = 2f;
    public float bulletCheckDistance = 2f;
    public float trailTime = 2f;

    public float bulletCooldown = 2.0f;
    private bool bMissed = false;
    public bool canReturn = true;

    private int reboundCounter = 0;
    public int maxRebounds;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        playerCamera = gunController.playerCamera;

        trailRenderer.emitting = false;
        trailRenderer.time = trailTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bMissed) CheckBulletFrontCollision();
        else ReturnAmmunition();

        if (reboundCounter >= maxRebounds) ReturnToPlayer(gunController.gunPoint.transform.position);
    }

    public void ReturnToPlayer(Vector3 playerPosition)
    {
        gameObject.transform.rotation = gunController.RotateToObject(playerPosition, transform.position);
        Destroy(trailController.GetTrailCollider());
        trailRenderer.emitting = false;
        bulletSpeed = 25f;
    }

    public void Rebound()
    {
        trailController.AddTrailToBullet(transform);

        if(reboundCounter == 0) transform.forward = Vector3.Reflect(playerCamera.transform.forward, bulletHit.normal);
        else transform.forward = Vector3.Reflect(transform.forward, bulletHit.normal);

        transform.position = bulletHit.point;

        reboundCounter++;
        trailRenderer.emitting = true;
    }

    public void ReturnAmmunition()
    {
        if (bulletCooldown > 0.0f)
        {
            bulletCooldown = bulletCooldown - Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void CheckBulletFrontCollision()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        trailController.AddTrailToBullet(transform);

        if (Physics.Raycast(transform.position, transform.forward, out bulletHit, bulletCheckDistance))
        {
            if (bulletHit.transform.tag == "Shield")
            {
                bMissed = true;
                Destroy(trailController.GetTrailCollider());
                transform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
            else if (bulletHit.transform.tag == "Player")
            {
                gunController.AddAmmunition();
                Destroy(gameObject);
            }
            else
            {
                Rebound();
                trailController.CreateNewTrail(transform);
            }
        }
    }

    private void OnDestroy()
    {
        trailRenderer.transform.parent = null;
        trailRenderer.autodestruct = true;

        if(bMissed) gunController.AddAmmunition();
    }
}
