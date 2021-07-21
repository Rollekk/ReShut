using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject colliderPrefab;
    private GameObject trailCollider;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewTrail(Transform bulletTransform)
    {
        trailCollider = Instantiate(colliderPrefab, bulletTransform.position, bulletTransform.rotation);
        startPosition = trailCollider.transform.position;
        trailCollider.transform.localScale = bulletTransform.localScale;
        trailCollider.transform.up = -bulletTransform.forward;

        Destroy(trailCollider, 2f);
    }

    public void AddTrailToBullet(Transform bulletTransform)
    {
        if (trailCollider)
        {
            trailCollider.transform.position = bulletTransform.position;
            trailCollider.transform.localScale = new Vector3(trailCollider.transform.localScale.x, Vector3.Distance(trailCollider.transform.position, startPosition) / 2, trailCollider.transform.localScale.z);
        }
    }

    public GameObject GetTrailCollider() { return trailCollider; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
