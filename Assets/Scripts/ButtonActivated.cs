using System;
using UnityEngine;

enum ButtonActivatedType
{
    Door,
    Elevator,
    None
}
public class ButtonActivated : MonoBehaviour
{
    private Door door;
    private Elevator ele;
    private ButtonActivatedType activatedType;
    public AudioClip activateSoundEffect;
    public AudioClip onPowerClip;
    public AudioClip losePowerClip;

    public void ButtonActivatedInitialize()
    {
        Debug.Log("ButtonActivated.cs used Start()!");


        if (GetComponent<Door>() != null)
        {
            door = GetComponent<Door>();
            activatedType = ButtonActivatedType.Door;
        }
        else if (GetComponent<Elevator>() != null)
        {
            ele = GetComponent<Elevator>();
            activatedType = ButtonActivatedType.Elevator;
        }
        else
        {
            activatedType = ButtonActivatedType.None;
        }

        //If universal functionality is needed, that will go here
    }

    public void Activate(GameObject playerObject)
    {
        Debug.Log("ButtonActivated is doing Activate()!");

        switch (activatedType)
        {
            case ButtonActivatedType.Door:
                door.Activation(playerObject);
                return;
            case ButtonActivatedType.Elevator:
                ele.Activation(playerObject);
                return;
            case ButtonActivatedType.None:
                Debug.Log("No Activation() behavior for this button activated object found!");
                return;
        }
    }

    public void OnPower(bool status)
    {
        Debug.Log("ButtonActivated is doing OnPower()!");

        switch (activatedType)
        {
            case ButtonActivatedType.Door:
                door.OnPowered(status);
                return;
            case ButtonActivatedType.Elevator:
                ele.OnPowered(status);
                return;
            case ButtonActivatedType.None:
                Debug.Log("No OnPowered behavior for this button for this object found!");
                return;
        }
    }
}

