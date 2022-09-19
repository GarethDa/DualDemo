using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(1.0f, 10.0f)] private float speed = 5.0f;
    [SerializeField] [Range(3.0f, 20.0f)] private float jumpForce = 10.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float groundDrag = 5.0f;
    [SerializeField] [Range(0.1f, 1.0f)] private float airSlowdown = 0.3f;
    [SerializeField] [Range(5.0f, 30.0f)] private float rotSpeed = 10.0f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;

    [Header("Rotation")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Camera playerCam;

    float horizontalInput;
    float verticalInput;

    //bool isJumping;

    Vector3 moveDir;

    Rigidbody rBody;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.freezeRotation = true;

        //player = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        
        //Rotate orientation
        Vector3 viewDir = transform.position - new Vector3(playerCam.transform.position.x, transform.position.y, playerCam.transform.position.z);
        orientation.forward = viewDir.normalized;

        //Rotate playerObj
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If the player has input a movement, spherically lerp between the current forward and the new direction
        if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, floorMask);

        MovePlayer();

        if (isGrounded) rBody.drag = groundDrag;

        else rBody.drag = 0;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //If the feet object is touching the ground, then jump
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        //Calculate direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If grounded, normal movement
        if (isGrounded) rBody.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);

        //Else if in the air, account for lack of drag (cube airSlowdown for a better range)
        else if (!isGrounded) rBody.AddForce(moveDir.normalized * speed * 10f * (airSlowdown * airSlowdown * airSlowdown) , ForceMode.Force);
    }
}
