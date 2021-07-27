using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { Normal, Ice, Fire, Poison, Electricity };

public class GunController : MonoBehaviour
{
    private GunType gunType = GunType.Normal;

    [Header("Components")]
    public GameObject gunPoint;
    public Camera playerCamera;

    private BulletController bullet;
    private Material[] gunMaterials;
    private GameObject playerBody;
    private PlayerUIController playerUI;
    private PlayerController playerController;

    private RaycastHit GunHit;

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
        if (Input.GetKeyDown(shootKey)) Shoot();

        AddGunSway();
    }

    void Shoot()
    {
        if (currentAmmunition > 0)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if(Physics.Raycast(ray, out GunHit))
            {
                bullet = Instantiate(bulletPrefab, gunPoint.transform.position, RotateToObject(GunHit.point, gunPoint.transform.position)).GetComponentInChildren<BulletController>();
                bullet.gunController = this;
                bullet.trailColor = gunMaterials[1].GetColor("_EmissionColor");

                currentAmmunition--;
                playerUI.UpdateAmmunitionText(currentAmmunition);
                bullet.canReturn = true;
            }
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

    public GunType GetCurrentGunType() { return gunType; }
}
