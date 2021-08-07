using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Components")]
    public Camera playerCamera;

    private PlayerController playerController;

    [Header("Sway")]
    private Vector3 initialHolderPosition;
    private float swayAmount = 0.1f;
    private float maxSwayAmount = 0.3f;
    private float smoothSwayAmount = 5.0f;

    [Header("Gun")]
    public Gun currentGun;

    private RaycastHit GunHit;
    private BulletController bullet;
    [SerializeField] private KeyCode shootKey;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.root.GetComponentInChildren<PlayerController>();

        playerCamera = playerController.playerCamera;

        initialHolderPosition = transform.localPosition;

        if (!currentGun) playerController.playerUI.ShowAmmunitionText(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGun)
        {
            if (Input.GetKeyDown(shootKey)) Shoot();

            AddGunSway();
        }
    }

    private void AddGunSway()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosToMove = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosToMove + initialHolderPosition, Time.deltaTime * smoothSwayAmount);
    }

    private void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out GunHit))
        {
            bullet = currentGun.Shoot(GunHit);
            playerController.playerUI.UpdateAmmunitionText(currentGun.currentAmmunition, currentGun.maxAmmunition);
        }
    }

    public void AddAmmunition()
    {
        if (bullet.canReturn)
        {
            currentGun.currentAmmunition++;
            playerController.playerUI.UpdateAmmunitionText(currentGun.currentAmmunition, currentGun.maxAmmunition);
        }
    }
}
