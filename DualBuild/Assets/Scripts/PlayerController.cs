using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerCamera = null;
    public bool lockCursor = true;

    [Range(1.0f, 8.0f)] public float mouseSensitivity = 3.5f;
    [Range(2.0f, 10.0f)] public float movementSpeed = 6.0f;
    [Range(0.0f, 0.5f)] public float moveSmoothTime = 0.25f;
    [Range(0.0f, 0.1f)] public float mouseSmoothTime = 0.03f;
    [Range(5.0f, 20.0f)] public float jumpSpeed = 10.0f;

    [Range(-25f, 0f)] public float gravity = -13f;

    float cameraPitch = 0.0f;
    float velY = 0.0f;
    CharacterController m_Controller = null;

    Vector2 inputCurrent = Vector2.zero;
    Vector2 currentVel = Vector2.zero;

    Vector2 currentMouseMovement = Vector2.zero;
    Vector2 currentMouseVel = Vector2.zero;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {

        m_Controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    
    void UpdateMouseLook()
    {
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        Vector2 targetMouseMovement = new Vector2(mX, mY);

        //Smooth the mouse movement to create a better-feeling transition between camera rotations
        currentMouseMovement = Vector2.SmoothDamp(currentMouseMovement, targetMouseMovement, ref currentMouseVel, mouseSmoothTime);

        //Vertical pitch is inverted, so subtract instead of add
        cameraPitch -= currentMouseMovement.y * mouseSensitivity;
        
        //Clamp to keep the camera view from going past straight up/down
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        //When dealing with vertical rotation, just rotate the camera
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        //When dealing with the horizontal movement, rotate the entire game object, which in turn rotates the camera since it is a child
        //Do this so that the game object's forward stays aligned with the camera (for player movement)
        transform.Rotate(Vector3.up * currentMouseMovement.x * mouseSensitivity);
    }
    

    void UpdateMovement()
    {
        float mX = Input.GetAxisRaw("Horizontal");
        float mY = Input.GetAxisRaw("Vertical");

        Vector2 inputTarget = new Vector2(mX, mY);

        //Normalize, since diagonal movement vectors will be larger
        inputTarget.Normalize();

        //Smooth between current velocity and target velocity.
        //Ensures that player doesn't go straight from 0 to 1 and back to 0 when enabling or disabling input
        inputCurrent = Vector2.SmoothDamp(inputCurrent, inputTarget, ref currentVel, moveSmoothTime);

        //print("Character is grounded");

        if (m_Controller.isGrounded)
        {
            if (Input.GetButton("Jump")) velY = jumpSpeed;

            else velY = 0.0f;
        }
        
        velY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * inputCurrent.y + transform.right * inputCurrent.x) * movementSpeed + Vector3.up * velY;

        m_Controller.Move(velocity * Time.deltaTime);
    }
}
