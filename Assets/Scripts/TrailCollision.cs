using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour
{
    public GameObject colliderPrefab;
    public BulletController bulletToFollow;

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

    public void CreateNewTrail(BulletController bulletToFollow)
    {
        this.bulletToFollow = bulletToFollow;
        trailCollider = Instantiate(colliderPrefab, this.bulletToFollow.transform.position, this.bulletToFollow.transform.rotation);
        startPosition = trailCollider.transform.position;
        trailCollider.transform.localScale = this.bulletToFollow.transform.localScale;
        trailCollider.transform.up = -this.bulletToFollow.transform.forward;

        Destroy(trailCollider, 2f);
    }

    public void UpdateTrail(Vector3 bulletPosition)
    {
        if (trailCollider)
        {
            trailCollider.transform.position = bulletPosition;
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
