using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    private bool isHeld = false;

    private GameObject PlayerCam;
    private PlayerV2 PlayerObject;
    [SerializeField] [Range(500.0f, 1500.0f)] private float ThrowForce = 800.0f;

    void Start()
    {
        //Find the player camera and player objects
        PlayerCam = GameObject.Find("Player Camera");
        PlayerObject = GameObject.Find("PlayerV2").GetComponent<PlayerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the ball is being held
        if (isHeld)
        {
            //Reset velocity and angular velocity to zero
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //Update the position to be in front of the player
            transform.position = PlayerCam.transform.position + 2 * PlayerCam.transform.forward;

            //Look to see if the player is throwing the ball
            CheckBallThrow();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //If the player touches a ball that isn't being held, and the player isn't already holding a ball
        if (!isHeld && collision.gameObject.tag == "Player" && !PlayerObject.IsHoldingBall())
        {
            //Update the position of the ball to be in front of the players
            transform.position = PlayerCam.transform.position + 2 * PlayerCam.transform.forward.normalized;
            
            //Update the ball to be held and the player to be holding a ball
            isHeld = true;
            PlayerObject.SetIsHoldingBall(true);

            //Set the camera as a parent to the ball, ensuring it maintains the same rotation as the camera
            transform.SetParent(PlayerCam.transform);
        }
    }

    void CheckBallThrow()
    {
        //If the player clicks the LMB
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Throw the ball forward, multiplied by the throwing force
            GetComponent<Rigidbody>().AddForce(PlayerCam.transform.forward * ThrowForce);

            //The ball is no longer held, and the player is no longer holding a ball
            isHeld = false;
            PlayerObject.SetIsHoldingBall(false);

            //The ball shouldn't be a child of the camera anymore
            transform.SetParent(null);
        }
    }
}
