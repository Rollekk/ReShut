using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody Rigidbody;
    [SerializeField] private CapsuleCollider CapsuleCollider;

    [Header("Player movement")]
    private float Forward;
    private float Sideways;
    public Vector3 moveVelocity;
    public float moveSpeed = 10f;
    private float defaultMoveSpeed;

    [Header("Checks")]
    public LayerMask groundMask;
    private bool shouldUncrouch;
    private bool isUnder = false;
    RaycastHit unCrouchHit;

    [Header("Gravity")]
    [SerializeField] Vector3 gravity;
    public float gravityStrength = 10f;
    public float holdDownForce = 3f;
    private float defaultHoldDownForce;
    private bool shouldGravity = true;
    public bool isOnGround;
    RaycastHit GroundHit;

    [Header("Jump")]
    public KeyCode jumpKey;
    public float jumpForce = 50f;
    public bool isInAir;
    public bool canDoubleJump;
    public bool canJump;

    [Header("Sprint")]
    public KeyCode sprintKey;
    public bool isSprinting;
    public float sprintSpeed;
    public float slowDownSpeed;
    public float maxMoveSpeed;

    [Header("Crouch && Slide")]
    public KeyCode crouchKey;
    public bool isCrouching;
    public bool isSliding;
    public float crouchSpeed;
    public float slideSlowDownSpeed;
    public float reducedCrouchHeight;
    private float defaultCrouch;


    // Start is called before the first frame update
    void Start()
    {
        CapsuleCollider = transform.GetComponentInChildren<CapsuleCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;

        defaultMoveSpeed = moveSpeed;
        defaultCrouch = CapsuleCollider.height;
        defaultHoldDownForce = holdDownForce;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        if (shouldUncrouch) UnCrouch();
    }

    // Update used for Physics
    void FixedUpdate()
    {
        PlayerPhysics();
        AddExtraGravity();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(jumpKey)) Jump();

        if (Input.GetKey(sprintKey) && !isCrouching && isOnGround &&  Forward != 0) Sprint();
        if (!Input.GetKey(sprintKey) && !isCrouching && isOnGround ) StopSprint();

        if (Input.GetKey(crouchKey) && !isSprinting && isOnGround) Crouch();
        if (Input.GetKeyUp(crouchKey) && !isSprinting ) UnCrouch();
    }

    void Jump()
    {
        float jump = jumpForce * gravityStrength;

        if (isOnGround)
        {
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
            canJump = false;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            canJump = false;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
        }
        else if (canJump)
        {
            canJump = false;
            canDoubleJump = true;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
            Rigidbody.AddForce(new Vector3(0f, jump, 0f));
        }
    }

    void Sprint()
    {
        isSprinting = true;
        if (moveSpeed < maxMoveSpeed) moveSpeed += sprintSpeed * Time.deltaTime;
    }

    void StopSprint()
    {
        isSprinting = false;
        if (moveSpeed > defaultMoveSpeed) moveSpeed -= slowDownSpeed * Time.deltaTime;
    }

    void Crouch()
    {
        if (moveSpeed > defaultMoveSpeed)
        {
            isSliding = true;
            isSprinting = false;
            isCrouching = true;
            if (moveSpeed > defaultMoveSpeed) moveSpeed -= slideSlowDownSpeed * Time.deltaTime;
            CapsuleCollider.height = reducedCrouchHeight;
        }
        else
        {
            isCrouching = true;
            moveSpeed = crouchSpeed;
            CapsuleCollider.height = reducedCrouchHeight;
        }
    }

    void UnCrouch()
    {
        if (!isUnder)
        {
            isCrouching = false;
            isSliding = false;
            shouldUncrouch = false;
            CapsuleCollider.height = defaultCrouch;
            moveSpeed = defaultMoveSpeed;
        }
        if (isUnder) shouldUncrouch = true;
    }

    //Player Physics and movement
    void PlayerPhysics()
    {
        Sideways = Input.GetAxisRaw("Horizontal");
        Forward = Input.GetAxisRaw("Vertical");

        moveVelocity = ((Sideways * transform.right + Forward * transform.forward) * moveSpeed) + Vector3.up * Rigidbody.velocity.y;
        Rigidbody.velocity = moveVelocity;

    }

    //Extra gravity for Unity Engine with slope Gravity
    void AddExtraGravity()
    {
        Ground();
    }

    void Ground()
    {
        isOnGround = Physics.Raycast(transform.position, -transform.up, out GroundHit, CapsuleCollider.height / 2, groundMask);
        isUnder = Physics.Raycast(transform.position, transform.up, out unCrouchHit, CapsuleCollider.height / 2, groundMask);
        if (!isOnGround && shouldGravity)
        {
            gravity = gravityStrength * -holdDownForce * Vector3.up;
            Rigidbody.AddForce(gravity);
            isInAir = true;
        }
        else isInAir = false;
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(ledgeCheck.position, ledgeBox);

    //    //Gizmos.color = Color.blue;
    //    //Gizmos.DrawSphere(headCheck.position, headDistance);

    //}
}
