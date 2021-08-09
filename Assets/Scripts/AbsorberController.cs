using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberController : Target
{
    [Header("Components")]
    private BulletController bullet;
    private TrailCollision trail;
    private MeshRenderer mesh;

    [Header("Absorber")]
    public bool isSpecific = false;
    public GunType neededGunType;
    
    [Space]
    public bool isActive = false;
    [SerializeField] private Color initialMaterialColor;
    private Light absorberLight;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();

        initialMaterialColor = mesh.sharedMaterial.GetColor("_EmissionColor");

        absorberLight = gameObject.GetComponent<Light>();

        absorberLight.color = initialMaterialColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnImpact(BulletController bullet)
    {
        this.bullet = bullet;

        ActivateAbsorber();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trail")
        {
            trail = other.GetComponentInChildren<TrailCollision>();
            bullet = trail.bulletToFollow;

            ActivateAbsorber();
        }
    }

    private void ResetMaterial()
    {
        mesh.material.SetColor("_EmissionColor", initialMaterialColor);
        absorberLight.color = initialMaterialColor;
        isActive = false;
    }

    private void ActivateAbsorber()
    {
        if (!isSpecific || bullet.gunController.currentGun.GetCurrentGunType() == neededGunType)
        {
            mesh.material.SetColor("_EmissionColor", bullet.trailColor);
            absorberLight.color = bullet.trailColor;
            isActive = true;
            Invoke("ResetMaterial", bullet.trailTime + 0.1f);
        }
    }
}
