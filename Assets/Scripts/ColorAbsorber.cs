using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAbsorber : MonoBehaviour
{
    [Header("Components")]
    private BulletController bullet;
    private TrailCollision trail;
    private MeshRenderer mesh;

    [Header("Color")]
    public bool isSpecific = false;
    public GunType neededGunType;

    public bool isActive = false;
    private Color initialMaterialColor;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();

        initialMaterialColor = mesh.sharedMaterial.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Trail")
        {
            trail = other.GetComponentInChildren<TrailCollision>();
            bullet = trail.bulletToFollow;

            if (isSpecific)
            {
                if (bullet.gunController.GetCurrentGunType() == neededGunType)
                {
                    ActivateAbsorber();
                }
            }
            else
            {
                ActivateAbsorber();
            }
        }
    }

    private void ResetMaterial()
    {
        mesh.material.SetColor("_EmissionColor", initialMaterialColor);
        isActive = false;
    }

    private void ActivateAbsorber()
    {
        mesh.material.SetColor("_EmissionColor", bullet.trailColor);
        isActive = true;
        Invoke("ResetMaterial", bullet.trailTime + 0.1f);
    }
}
