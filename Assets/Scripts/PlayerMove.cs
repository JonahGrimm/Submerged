using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    //References
    private CharacterController cc;
    private Player p;
    private Animator anim;
    public GameObject characterModel;

    //Internal Vars
    private float joystickAngle;
    private Vector3 movementVector;
    private float velocity = 0f;

    //Designer Vars
    public float accel = 1f;
    public float walkMaxSpeed = 5f;
    public float runMaxSpeed = 10f;
    public float m_RotationStrength = 5f;
    public Vector3 inputMovementVector;

    //Animator Vars
    private int a_Movespeed = Animator.StringToHash("Movespeed");

    void Start()
    {
        p = ReInput.players.GetPlayer(0);
        cc = GetComponent<CharacterController>();
        anim = characterModel.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float hor = p.GetAxis("Move Horizontal");
        float ver = p.GetAxis("Move Vertical");
        bool run = p.GetButton("Run");

        inputMovementVector = new Vector3(hor, 0f, ver);

        if (inputMovementVector != Vector3.zero)
        {
            UpdateJoystickAngle();
            LeftStickInputToWorld();

            if (run)
            {
                if (velocity < runMaxSpeed)
                {
                    velocity += accel;
                    if (velocity > runMaxSpeed)
                        velocity = runMaxSpeed;
                }
            }
            else
            {
                if (velocity < walkMaxSpeed)
                    velocity += accel;

                if (velocity > walkMaxSpeed)
                    velocity = walkMaxSpeed;
            }

        }
        else
        {
            movementVector = Vector3.zero;
            if (velocity > 0f)
            {
                velocity -= accel;
                if (velocity < 0f)
                {
                    velocity = 0f;
                }
            }
        }

        cc.Move(movementVector * velocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        anim.SetFloat(a_Movespeed, velocity);
        if (movementVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_RotationStrength * Time.deltaTime);
        }
    }

    void UpdateJoystickAngle()
    {
        //Get the initial angle the joystick is making normally
        joystickAngle = Vector3.Angle(Vector3.forward, inputMovementVector);
        //Perform a cross to see if it resides in the left or right hemisphere
        Vector3 cross = Vector3.Cross(Vector3.forward, inputMovementVector);
        //If in the left hemisphere convert from counterclockwise to clockwise
        if (cross.y < 0)
            joystickAngle = 180f + (180f - joystickAngle);
    }

    void LeftStickInputToWorld()
    {
        //Make a final angle that will be used for the final movementVector (relative to camera)
        float finalAngle = Camera.main.transform.eulerAngles.y + joystickAngle;
        //Construct a final movementVector (world space) based off of the final angle)
        movementVector = new Vector3(Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(finalAngle * Mathf.Deg2Rad));
        //Make sure magnitude is correct
        movementVector *= Mathf.Clamp01(inputMovementVector.magnitude);
    }

}
