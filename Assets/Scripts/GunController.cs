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
    public float spawnTargetDistance;
    public int maxTargetPoints;
    public LayerMask TargetLayer;
    public GameObject targetPrefab;
    public List<GunTarget> gunPointsList = new List<GunTarget>();

    private RaycastHit GunHit;

    [Header("Bullet")]
    private Bullet bullet;

    [Header("Shoot")]
    public KeyCode shootKey;
    public int currentAmmunition = 1;
    public GameObject bulletPrefab;

    [Header("Sway")]
    private Vector3 initialGunPosition;
    private float swayAmount = 0.1f;
    private float maxSwayAmount = 0.3f;
    private float smoothSwayAmount = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gunMaterials = GetComponentInChildren<MeshRenderer>().materials;
        playerController = transform.root.GetComponentInChildren<PlayerController>();

        initialGunPosition = transform.localPosition;

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

        if (gunPointsList.Count == 0 && bullet) bullet.ReturnToPlayer(gunPoint.transform.position);

        AddGunSway();
    }

    #region - Target -
    void CreateTarget()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out GunHit, spawnTargetDistance, TargetLayer))
        {
            if (gunPointsList.Count < maxTargetPoints && currentAmmunition > 0)
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

    public void ClearOneTargetPoint()
    {
        Destroy(gunPointsList[0].targetCube);
        gunPointsList.RemoveAt(0);
    }

    public void ClearAllTargetPoints()
    {
        for (int i = 0; i < gunPointsList.Count; i++)
            Destroy(gunPointsList[i].targetCube);
        gunPointsList.Clear();
    }

    #endregion

    void Shoot()
    {
        if (gunPointsList.Count > 0 && currentAmmunition > 0)
        {
            bullet = Instantiate(bulletPrefab, gunPoint.transform.position, RotateToObject(gunPointsList[0].targetPoint, gunPoint.transform.position)).GetComponentInChildren<Bullet>();
            bullet.gunController = this;

            bullet.bulletSpeed = Vector3.Distance(playerBody.transform.position, gunPointsList[0].targetPoint) * 6;
            currentAmmunition--;
            playerUI.UpdateAmmunitionText(currentAmmunition);
            bullet.canReturn = true;
        }
    }

    private void AddGunSway()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosToMove = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosToMove + initialGunPosition, Time.deltaTime * smoothSwayAmount);
    }

    public void AddAmmunition()
    {
        if (bullet.canReturn)
        {
            currentAmmunition++;
            playerUI.UpdateAmmunitionText(currentAmmunition);
        }
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
}
