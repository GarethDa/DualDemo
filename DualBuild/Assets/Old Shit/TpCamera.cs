using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpCamera : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rBody;
    //[SerializeField] private GameObject cMachine;

    [SerializeField][Range(5.0f, 30.0f)] private float rotSpeed = 10.0f;

    //[SerializeField] [Range(0.0f, 50.0f)] private float offset = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //virtualCam = cMachine.GetComponent<CinemachineFreeLook>();
        //composer = virtualCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //Rotate playerObj
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If the player has input a movement, spherically lerp between the current forward and the new direction
        if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
        /*
        if (Input.GetKey(KeyCode.Mouse1))
        {
            virtualCam.m_Lens.FieldOfView = 40;
            composer.m_TrackedObjectOffset.x = offset;
        }

        else
        {
            virtualCam.m_Lens.FieldOfView = 50;
            composer.m_TrackedObjectOffset.x = 0;
        }
        */
    }
}
