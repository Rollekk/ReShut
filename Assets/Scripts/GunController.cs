using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GunTarget
{
    public GameObject targetCube;
    public Vector3 targetPoint;

    public GunTarget(GameObject targetCube, Vector3 targetPoint)
    {
        this.targetCube = targetCube;
        this.targetPoint = targetPoint;
    }
}

public class GunController : MonoBehaviour
{
    public enum GunType { Normal, Ice, Fire, Poison, Electricity };
    private GunType gunType = GunType.Normal;

    [Header("Components")]
    public GameObject gunPoint;

    private Camera playerCamera;
    private Material[] gunMaterials;
    private GameObject playerBody;
    private PlayerUIController playerUI;
    private PlayerController playerController;

    [Header("Target")]
    public KeyCode targetKey;
    public float putTargetRange;
    public int maxTargetPoints;
    public LayerMask TargetLayer;
    public GameObject targetPrefab;

    public List<GunTarget> gunPointsList = new List<GunTarget>();

    private RaycastHit GunHit;

    [Header("Shoot")]
    public KeyCode shootKey;
    public GameObject bulletPrefab;

    private GameObject bulletObject;
    private Bullet bulletScript;

    [Header("Bullet")]
    public int currentAmmunition= 1;

    private float bulletCD = 2.0f;
    private bool bShouldBulletTimerTick = false;
    private bool bReturnBullet = true;

    // Start is called before the first frame update
    void Start()
    {
        gunMaterials = GetComponentInChildren<MeshRenderer>().materials;
        playerController = transform.root.GetComponentInChildren<PlayerController>();

        playerCamera = playerController.playerCamera;
        playerBody = playerController.transform.gameObject;
        playerUI = playerController.playerUI;
        playerUI.UpdateAmmunitionText(currentAmmunition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(targetKey)) CreateTarget();
        if (Input.GetKeyDown(shootKey)) Shoot();

        if (gunPointsList.Count == 0 && bulletScript)
        {
            bulletScript.ReturnToPlayer(gunPoint.transform.position);
        }

        if (bShouldBulletTimerTick) ReturnBulletAfterTime();
    }

    void CreateTarget()
    {
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out GunHit, putTargetRange, TargetLayer))
        {
            if (gunPointsList.Count < maxTargetPoints)
            {
                GameObject target = Instantiate(targetPrefab, GunHit.point, targetPrefab.transform.rotation);
                target.layer = LayerMask.NameToLayer("Target");
                gunPointsList.Add(new GunTarget(target.gameObject, GunHit.point));
            }
        }

        for (int i = 0; i < gunPointsList.Count - 1; i++)
        {
          gunPointsList[i].targetCube.transform.rotation = RotateToObject(gunPointsList[i + 1].targetPoint, gunPointsList[i].targetPoint);
        }

    }

    void Shoot()
    {
        if (gunPointsList.Count > 0 && currentAmmunition > 0)
        {
            bulletObject = Instantiate(bulletPrefab, gunPoint.transform.position, RotateToObject(gunPointsList[0].targetPoint, gunPoint.transform.position));

            bulletScript = bulletObject.GetComponentInChildren<Bullet>();
            bulletScript.gunController = this;

            bulletScript.bulletSpeed = Vector3.Distance(playerBody.transform.position, gunPointsList[0].targetPoint) * 6;
            currentAmmunition--;
            playerUI.UpdateAmmunitionText(currentAmmunition);
            bReturnBullet = true;
        }
    }

    public void ClearOneTargetPoint()
    {
        Destroy(gunPointsList[0].targetCube);
        gunPointsList.RemoveAt(0);
    }

    public void ClearAllTargetPoints()
    {
        for(int i = 0; i < gunPointsList.Count; i++)
        Destroy(gunPointsList[i].targetCube);
        gunPointsList.Clear();
    }

    public void UpdateAmmunitionUI()
    {
        if(bReturnBullet)
        {
            currentAmmunition++;
            playerUI.UpdateAmmunitionText(currentAmmunition);
            bReturnBullet = false;
        }
    }

    public void ReturnBulletAfterTime()
    {
        if(bulletCD > 0.0f)
        {
            bulletCD = bulletCD - Time.deltaTime;
        }
        else
        {
            UpdateAmmunitionUI();
            bShouldBulletTimerTick = false;
            bulletCD = 2.0f;
        }
    }

    public Quaternion RotateToObject(Vector3 target, Vector3 start)
    {
        Vector3 targetDirection = target - start;

        return Quaternion.LookRotation(targetDirection);
    }

    public void StartBulletCD(bool bShouldStart)
    {
        bShouldBulletTimerTick = bShouldStart;
    }

    public void ChangeWeaponType(GunType toThis, Color newColor)
    {
        gunType = toThis;
        gunMaterials[1].SetColor("_EmissionColor", newColor);
    }
}
