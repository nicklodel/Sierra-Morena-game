using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float speed = 20.0f;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;
    
    [Header("Ground Check")] 
    public float playerHeight;

    public LayerMask whatIsGround;

    private bool grounded;

    public float groundDrag;

    [Header("Keybinds")] 
    public KeyCode jumpKey = KeyCode.Space;
    
    public Transform orientation;

    private float horizontal;

    private float vertical;

    private Vector3 moveDirection;

    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        readyToJump = true;
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        MyInput();
        SpeedControl();

        if (grounded)
            playerRb.drag = groundDrag;
        else
            playerRb.drag = 0;
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput(){
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            grounded = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
}

    private void MovePlayer()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        
        if (grounded)
            playerRb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        else if(!grounded)
            playerRb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        if (flatvel.magnitude > speed)
        {
            Vector3 limitedvel = flatvel.normalized * speed;
            playerRb.velocity = new Vector3(limitedvel.x, playerRb.velocity.y, limitedvel.z);
        }
    }

    private void Jump()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        
        playerRb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collided!");
            grounded = true;
        }
    }
}
