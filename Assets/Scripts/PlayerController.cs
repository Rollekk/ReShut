using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Camera playerCamera;
    public PlayerUIController playerUI;
    public GunController gunController;

    [Header("Health")]
    public float playerHP;

    [Header("Stamina")]
    public float playerStamina;

    [Header("Pickup")]
    [SerializeField] private KeyCode pickupKey;
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private float pickupDistance;

    private RaycastHit cursorHit;
    private PickupController pb;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        gunController = transform.Find("PlayerCamera/GunHolder/").GetComponentInChildren<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out cursorHit, pickupDistance, pickupMask))
        {
            if (cursorHit.transform.tag == "Pickup")
            {
                pb = cursorHit.transform.GetComponentInChildren<PickupController>();

                pb.ShowPickupText(true);
                pb.UpdatePickupText(pickupKey, cursorHit.transform.name);

                if (Input.GetKeyDown(pickupKey))
                {
                    pb.PickupInteraction(this);
                }
            }
        }
        else if (pb)
        {
            pb.ShowPickupText(false);
            pb = null;
        }
    }
}

