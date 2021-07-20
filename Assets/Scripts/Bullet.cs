using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2f;
    public float bulletCast = 2f;
    public float trailTime = 2f;

    public GunController gunController;
    public TrailController trailController;

    private TrailRenderer trailRenderer;
    private RaycastHit bulletHit;


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
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        trailController.AddTrailToBullet(transform);

        if (Physics.Raycast(transform.position, transform.forward, out bulletHit, bulletCast))
        {
            if (bulletHit.transform.tag == "Target")
            {
                Rebound();
                trailController.CreateNewTrail(transform);
            }
            else if (bulletHit.transform.tag == "Player")
            {
                gunController.UpdateAmmunitionUI();
                Destroy(gameObject);
            }
            else
            {
                gunController.StartBulletCD(true);
                gunController.ClearAllTargetPoints();
                Destroy(trailController.GetCylinder());
                Destroy(this.gameObject);
            }
        }
    }

    public void ReturnToPlayer(Vector3 playerPosition)
    {
        gameObject.transform.rotation = gunController.RotateToObject(playerPosition, transform.position);
        Destroy(trailController.GetCylinder());
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

    private void OnDestroy()
    {
        trailRenderer.transform.parent = null;
        trailRenderer.autodestruct = true;
    }

    //private void OnDrawGizmos()
    //{
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.position, 0.05f);
    //}
}
