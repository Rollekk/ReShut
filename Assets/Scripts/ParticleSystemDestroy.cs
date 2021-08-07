using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDestroy : MonoBehaviour
{
    private ParticleSystem particleSystemToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        particleSystemToDestroy = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleSystemToDestroy.IsAlive()) Destroy(gameObject);
    }
}
