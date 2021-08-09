using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BulletController : MonoBehaviour
{
    [Header("Components")]
    public GunController gunController;

    private Camera playerCamera;
    private RaycastHit bulletHit;
    public ParticleSystem reboundParticles;

    [Header("Bullet")]
    public float bulletSpeed = 2f;
    public float bulletCooldown = 2.0f;
    public bool canReturn = true;

    private bool bMissed = false;
    private Vector3 previousPosition;

    [Header("Trail")]
    public TrailCollision trailCollision;
    public Color trailColor;
    public float trailTime = 2f;

    private TrailRenderer trailRenderer;

    [Header("Rebound")]
    public int maxRebounds;

    private int reboundCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.material.SetColor("_EmissionColor", trailColor);

        playerCamera = gunController.playerCamera;

        trailRenderer.emitting = false;
        trailRenderer.time = trailTime;

        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bMissed) CheckBulletFrontCollision();
        else ReturnAmmunition();
    }

    private void CheckBulletFrontCollision()
    {
        previousPosition = transform.position;

        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        if(trailRenderer.emitting) trailCollision.UpdateTrail(transform.position);

        if (Physics.Raycast(previousPosition, (transform.position - previousPosition).normalized, out bulletHit, (transform.position - previousPosition).magnitude))
        {
            if (bulletHit.transform.tag == "Shield" || bulletHit.transform.tag == "Bullet_Hit")
            {
                bMissed = true;
                transform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

                if (bulletHit.transform.tag == "Bullet_Hit") bulletHit.transform.GetComponentInChildren<Target>().OnImpact(this);
            }
            else if (bulletHit.transform.tag == "Player")
            {
                gunController.AddAmmunition();
                Destroy(gameObject);
            }
            else if (bulletHit.transform.tag != "Bullet_Ignore")
            {
                if(reboundCounter >= maxRebounds) ReturnToPlayer(gunController.currentGun.gunPoint.transform.position);
                else
                {
                    Rebound();
                    trailCollision.CreateNewTrail(this);
                }
            }
        }
    }

    public void ReturnToPlayer(Vector3 playerPosition)
    {
        gameObject.transform.rotation = gunController.currentGun.RotateToObject(playerPosition, transform.position);
        trailRenderer.emitting = false;
        bulletSpeed = 25f;
    }

    public void Rebound()
    {
        if(reboundCounter == 0) transform.forward = Vector3.Reflect(playerCamera.transform.forward, bulletHit.normal);
        else transform.forward = Vector3.Reflect(transform.forward, bulletHit.normal);

        transform.position = bulletHit.point;

        reboundCounter++;
        trailRenderer.emitting = true;

        CreateReboundParticle();
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

    private void CreateReboundParticle()
    {
        ParticleSystem ps = Instantiate(reboundParticles, bulletHit.point, Quaternion.LookRotation(bulletHit.normal));
        ps.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", trailColor);
        ps.Play();
    }

    private void OnDestroy()
    {
        trailRenderer.transform.parent = null;
        trailRenderer.autodestruct = true;

        if(bMissed) gunController.AddAmmunition();
    }
}
