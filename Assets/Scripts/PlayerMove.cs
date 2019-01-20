using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    //References
    private CharacterController cc;
    private Player p;
    private Animator anim;
    private Light lightComponent;
    private Transform mainCamera;
    public GameObject characterModel;
    public GameObject flashLightObject;
    public Image crossHair;

    //Internal Vars
    private float joystickAngle;
    private Vector3 movementVector;
    private float velocity = 0f;
    private float hor;
    private float ver;
    private bool _run;
    private bool Run
    {
        get
        {
            return _run;
        }
        set
        {
            _run = value;

            if (_run)
                FlashlightToggle(false);
            else if (PowerCubes > 0)
                FlashlightToggle(true);
        }
    }           //Uses a property to check upon every setter if running or not to toggle flashlight

    private RaycastHit interactHitInfo;
    private GameObject selectedInteractable;
    [HideInInspector]
    public int interactableLayerMask;
    private int powerCubes = 0;
    private Interactable interactable;

    //Global variables
    public int PowerCubes
    {
        get
        {
            return powerCubes;
        }
        set
        {
            powerCubes = value;
            if (powerCubes > 0)
                FlashlightToggle(true);
            else
                FlashlightToggle(false);
        }
    }

    //Designer Vars
    public float accel = 1f;
    public float walkMaxSpeed = 5f;
    public float runMaxSpeed = 10f;
    public float rotationStrength = 5f;
    public float runRotationStrength = 1f;
    public float gravity;
    public Vector3 inputMovementVector;
    public Material selectedMaterial;
    private float interactRange = 5f;

    //Animator Vars
    private int a_Movespeed = Animator.StringToHash("Movespeed");

    void Start()
    {
        //Initialization
        p = ReInput.players.GetPlayer(0);
        cc = GetComponent<CharacterController>();
        anim = characterModel.GetComponent<Animator>();
        lightComponent = flashLightObject.GetComponent<Light>();
        mainCamera = Camera.main.gameObject.transform;
        interactableLayerMask = LayerMask.GetMask("Interactable");
        FlashlightToggle(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        //Update inputs
        hor = p.GetAxis("Move Horizontal");
        ver = p.GetAxis("Move Vertical");
        Run = p.GetButton("Run");

        //Check if the player is looking at an interactable
        if (Physics.Raycast(mainCamera.position,mainCamera.forward, out interactHitInfo, interactRange,interactableLayerMask))
        {
            selectedInteractable = interactHitInfo.collider.gameObject;
            interactable = selectedInteractable.GetComponent<Interactable>();
            crossHair.color = Color.cyan;
        }
        //If not looking at an interactable
        else
        {
            selectedInteractable = null;
            interactable = null;
            crossHair.color = Color.white;
        }

        //If player is pressing "Interact" and is looking at an interactable
        if (p.GetButtonDown("Interact") && interactable != null)
        {
            //Interactable is a "middle-man" class that will call Interaction()
            //on behavior component that derives from Interactable
            interactable.Interact(this.gameObject);
        }

        //Player Movement
        inputMovementVector = new Vector3(hor, 0f, ver);
        if (inputMovementVector != Vector3.zero)
        {
            Vector3 lastMV = movementVector;

            UpdateJoystickAngle();
            LeftStickInputToWorld();

            //Speed cap for running
            if (Run)
            {
                if (velocity < runMaxSpeed)
                {
                    velocity += accel;
                    if (velocity > runMaxSpeed)
                        velocity = runMaxSpeed;
                }

                //Lerp target direction
                //Immediately updating rotation looks jerky
                movementVector = Vector3.Lerp(lastMV, movementVector, runRotationStrength * Time.fixedDeltaTime);
                crossHair.enabled = false;
            }
            //Speed cap for walking
            else
            {
                if (velocity < walkMaxSpeed)
                    velocity += accel;

                if (velocity > walkMaxSpeed)
                    velocity = walkMaxSpeed;

                //Lerp target direction
                //Immediately updating rotation looks jerky
                movementVector = Vector3.Lerp(lastMV, movementVector, rotationStrength * Time.fixedDeltaTime);
                crossHair.enabled = true;
            }


        }
        //If not moving, deaccelerate
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

            crossHair.enabled = true;
        }

        //Move player and apply gravity
        cc.Move(movementVector * velocity * Time.fixedDeltaTime);
        cc.Move(-transform.up * gravity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        //Update animator and smooth player rotation
        anim.SetFloat(a_Movespeed, velocity);
        if (movementVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationStrength * Time.deltaTime);
        }

        //Render faint glow on selected object
        if (interactable != null)
        {
            Vector3 scale = selectedInteractable.transform.lossyScale;

            Matrix4x4 matrix = Matrix4x4.TRS(selectedInteractable.transform.position, selectedInteractable.transform.rotation, scale);

            Graphics.DrawMesh(interactable.meshForHighlightSelection, matrix, selectedMaterial, 0);
        }
    }

    private void FlashlightToggle(bool status)
    {
        flashLightObject.SetActive(status);
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
