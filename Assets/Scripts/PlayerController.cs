using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Camera playerCamera;
    public PlayerUIController playerUI;

    [Header("Health")]
    public float playerHP;

    [Header("Stamina")]
    public float playerStamina;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
