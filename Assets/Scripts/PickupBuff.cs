using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupController
{
    public float rotationSpeed;
    public float floatSpeed = 0.1f;
    public float floatRange = 1f;

    public GunType buffType;
    private Material buffMaterial;
    private Color buffColor;

    private float t = 0.0f;
    private float maxLerp, minLerp;

    // Start is called before the first frame update
    void Start()
    {
        minLerp = transform.position.y;
        maxLerp = transform.position.y + floatRange;

        buffMaterial = GetComponent<MeshRenderer>().material;
        buffColor = buffMaterial.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
        
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(minLerp, maxLerp, t), transform.position.z);
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
        Debug.Log("Buff");
        BuffPlayerWeapon(playerController.gunController);
    }
}
