using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float lookForward = 0f;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Github test");
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        lookForward -= mouseY;
        lookForward = Mathf.Clamp(lookForward, -90f, 90f);

        //Move camera up & down, rotates just camera
        transform.localRotation = Quaternion.Euler(lookForward, 0f, 0f);
        //Move camera sideways, rotates whole player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
