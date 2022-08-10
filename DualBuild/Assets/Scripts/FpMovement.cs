using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpMovement : MonoBehaviour
{
    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    private float xRot;
    private bool holdingBall = false;

    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Rigidbody playerBody;
    [Space]
    [SerializeField][Range(1.0f, 10.0f)] private float speed = 5.0f;
    [SerializeField][Range(1.0f, 8.0f)] private float sensitivity = 3.0f;
    [SerializeField][Range(3.0f, 20.0f)] private float jumpForce = 10.0f;
    [Space]
    [SerializeField] private bool lockCursor = true;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        //Grab the horizontal (A and D) and vertical (W and S) input, store it into a vector
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //Grab the mosue movement, store it in a vector
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        //Grab the base movement, multiply by speed
        //(multiply here to avoid multiplying the y-velocity by speed, which would make the player fall faster)
        Vector3 moveVector = transform.TransformDirection(playerMovementInput) * speed;

        //Update the velocity
        playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);

        //Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //If the feet object is touching the ground, then jump
            if (Physics.CheckSphere(feetTransform.position, 0.1f, floorMask))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void MovePlayerCamera()
    {
        //Look up and down
        xRot -= playerMouseInput.y * sensitivity;

        transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    public bool IsHoldingBall()
    {
        return holdingBall;
    }
    public void SetIsHoldingBall(bool holding)
    {
        holdingBall = holding;
    }
}
