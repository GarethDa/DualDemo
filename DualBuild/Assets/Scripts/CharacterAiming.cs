using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] [Range(10.0f, 40.0f)] private float turnSpeed = 15.0f;
    [SerializeField] [Range(0.1f, 1.0f)] private float aimDuration = 0.3f;

    [SerializeField] Transform followTarget;
    [SerializeField] Camera playerCam;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    Animator animator;
    int isAimingVariable = Animator.StringToHash("isAiming");

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isAiming = Input.GetMouseButton(1);
        animator.SetBool(isAimingVariable, isAiming);

        /*
        if (isAiming)
        {
            transform.forward = playerCam.transform.forward;
        }
        */
    }

    private void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        followTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }


}
