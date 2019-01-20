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

    public void Toggle(GameObject playerObject)
    {
        switch (activatedType)
        {
            case ButtonActivatedType.Door:
                door.ToggleObj(playerObject);
                return;
            case ButtonActivatedType.Elevator:
                ele.ToggleObj(playerObject);
                return;
            case ButtonActivatedType.None:
                Debug.Log("No Activation() behavior for this button activated object found!");
                return;
        }
    }

    public void Activate(GameObject playerObject, bool status)
    {
        switch (activatedType)
        {
            case ButtonActivatedType.Door:
                door.Activation(playerObject, status);
                return;
            case ButtonActivatedType.Elevator:
                ele.Activation(playerObject, status);
                return;
            case ButtonActivatedType.None:
                Debug.Log("No Activation() behavior for this button activated object found!");
                return;
        }
    }

    public void OnPower(bool status)
    {
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

