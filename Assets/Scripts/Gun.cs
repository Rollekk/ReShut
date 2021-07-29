using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { Normal, Ice, Fire, Poison, Electricity };

public class Gun : PickupController
{
    private GunType gunType = GunType.Normal;

    [Header("Components")]
    public GameObject gunPoint;

    private GunController gunController;
    private BulletController bullet;
    private Material[] gunMaterials;

    [Header("Shoot")]
    public int currentAmmunition = 1;
    public int maxAmmunition = 1;
    public float bulletTrailtime = 2f;
    public GameObject bulletPrefab;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        gunMaterials = GetComponentInChildren<MeshRenderer>().materials;

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public BulletController Shoot(RaycastHit GunHit)
    {
        if (currentAmmunition > 0)
        {
            bullet = Instantiate(bulletPrefab, gunPoint.transform.position, RotateToObject(GunHit.point, gunPoint.transform.position)).GetComponentInChildren<BulletController>();
            bullet.gunController = gunController;
            bullet.trailColor = gunMaterials[1].GetColor("_EmissionColor");
            bullet.trailTime = bulletTrailtime;

            currentAmmunition--;
            bullet.canReturn = true;

            return bullet;
        }

        else return null;
    }

    public void ChangeGunType(GunType toThis, Color newColor)
    {
        gunType = toThis;
        gunMaterials[1].SetColor("_EmissionColor", newColor);
    }

    public Quaternion RotateToObject(Vector3 target, Vector3 start)
    {
        Vector3 targetDirection = target - start;

        return Quaternion.LookRotation(targetDirection);
    }

    public GunType GetCurrentGunType() { return gunType; }

    public override void PickupInteraction(PlayerController playerController)
    {
        gunController = playerController.gunController;

        playerController.playerUI.ShowAmmunitionText(true);
        playerController.playerUI.UpdateAmmunitionText(currentAmmunition, maxAmmunition);

        gunController.currentGun = this;

        transform.position = gunController.transform.position;
        transform.rotation = gunController.transform.rotation;
        transform.parent = gunController.transform;
    }
}
