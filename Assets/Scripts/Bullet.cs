using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2f;
    public float bulletCheckDistance = 2f;
    public float trailTime = 2f;

    public GunController gunController;
    public TrailController trailController;

    private TrailRenderer trailRenderer;
    private RaycastHit bulletHit;

    public float bulletCooldown = 2.0f;
    private bool bMissed = false;
    public bool canReturn = true;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        trailRenderer.emitting = false;
        trailRenderer.time = trailTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bMissed) CheckBulletFrontCollision();
        else ReturnAmmunition();

    }

    public void ReturnToPlayer(Vector3 playerPosition)
    {
        gameObject.transform.rotation = gunController.RotateToObject(playerPosition, transform.position);
        Destroy(trailController.GetTrailCollider());
        trailRenderer.emitting = false;
        bulletSpeed = 50;
    }

    public void Rebound()
    {
        transform.position = gunController.gunPointsList[0].targetPoint;
        transform.rotation = gunController.gunPointsList[0].targetCube.transform.rotation;

        trailController.AddTrailToBullet(transform);

        if (gunController.gunPointsList.Count != 1) bulletSpeed = Vector3.Distance(gunController.gunPointsList[0].targetPoint, gunController.gunPointsList[1].targetPoint) * 6;
 
        gunController.ClearOneTargetPoint();
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
            if (bulletHit.transform.tag == "Target")
            {
                Rebound();
                trailController.CreateNewTrail(transform);
            }
            else if (bulletHit.transform.tag == "Player")
            {
                gunController.AddAmmunition();
                Destroy(gameObject);
            }
            else
            {
                bMissed = true;
                gunController.ClearAllTargetPoints();
                Destroy(trailController.GetTrailCollider());
                transform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
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
