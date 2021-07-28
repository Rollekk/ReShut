using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    private PlayerController playerController;

    [Header("Sway")]
    private Vector3 initialGunPosition;
    private float swayAmount = 0.1f;
    private float maxSwayAmount = 0.3f;
    private float smoothSwayAmount = 5.0f;

    [SerializeField] private KeyCode shootKey;

    public Gun currentGun;
    private RaycastHit GunHit;
    private BulletController bullet;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.root.GetComponentInChildren<PlayerController>();
        currentGun = GetComponentInChildren<Gun>();

        playerCamera = playerController.playerCamera;

        initialGunPosition = transform.localPosition;

        playerController.playerUI.currentGun = currentGun;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootKey)) Shoot();

        AddGunSway();
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

    private void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out GunHit))
        {
            bullet = currentGun.Shoot(GunHit);
            playerController.playerUI.UpdateAmmunitionText();
        }
    }

    public void AddAmmunition()
    {
        if (bullet.canReturn)
        {
            currentGun.currentAmmunition++;
            playerController.playerUI.UpdateAmmunitionText();
        }
    }
}
