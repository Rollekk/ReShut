using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : MonoBehaviour
{
    public float rotationSpeed;
    public float floatSpeed = 0.1f;
    public float floatRange = 1f;

    public Material trailMaterial;
    public GunController.GunType buffType;
    private Material buffMaterial;
    private Color buffColor;

    private float t = 0.0f;
    private float maxLerp, minLerp;

    private GunController gunController;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gunController = other.transform.Find("PlayerCamera/GunHolder/").GetComponentInChildren<GunController>();
            BuffPlayerWeapon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void BuffPlayerWeapon()
    {
        gunController.ChangeGunType(buffType, buffColor);

        trailMaterial.SetColor("_EmissionColor", buffColor);
    }
}
