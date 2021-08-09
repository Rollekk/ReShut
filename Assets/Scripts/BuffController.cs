using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffController : PickupController
{
    [Header("Components")]
    public GunType buffType;

    [Header("Float")]
    public float rotationSpeed;
    public float floatSpeed = 0.1f;
    public float floatRange = 1f;

    private float t = 0.0f;
    private float maxLerp, minLerp;

    private Material buffMaterial;
    private Color buffColor;

    // Start is called before the first frame update
    protected override void Start()
    {
        pickupTMP = transform.parent.GetComponentInChildren<TMP_Text>();
        base.Start();

        buffMaterial = GetComponentInChildren<MeshRenderer>().material;
        buffColor = buffMaterial.GetColor("_EmissionColor");

        transform.parent.GetComponentInChildren<Light>().color = buffColor;

        minLerp = transform.position.y;
        maxLerp = transform.position.y + floatRange;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(minLerp, maxLerp, t), transform.position.z);
        pickupTMP.transform.position = transform.position + new Vector3(0f, initialTextHeight, 0f);
        t += floatSpeed * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = maxLerp;
            maxLerp = minLerp;
            minLerp = temp;
            t = 0.0f;
        }
    }

    private void BuffPlayerWeapon(GunController gunController)
    {
        gunController.currentGun.ChangeGunType(buffType, buffColor);
    }

    public override void PickupInteraction(PlayerController playerController)
    {
        if (playerController.gunController.currentGun) BuffPlayerWeapon(playerController.gunController);
    }
}
