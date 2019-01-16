using System;
using UnityEngine;

enum PoweredType
{
    DoorButton,
    HeavyDoor,
    None
}
public class PoweredInteractable : Interactable
{
    public bool IsPowered
    {
        get
        {
            if (Power >= requiredPower)
                return true;
            else
                return false;
        }

    }
    public int Power
    {
        get
        {
            return _power;
        }
        set
        {
            int previousPower = _power;
            _power = value;

            //If the new total amount of power is greater than the required
            //AND the previous total amount of power was NOT greater than the required
            if (_power >= requiredPower && previousPower < requiredPower)
                OnPower(true);

            //If the new total amount of power is less than the required
            //AND the previous total amount of power was NOT less than the required
            if (_power < requiredPower && previousPower >= requiredPower)
                OnPower(false);
        }
    }
    public int _power = 0;
    public int requiredPower = 1;
    private DoorButton pdb;
    private HeavyDoor hd;
    private PoweredType pt;
    public AudioClip onPowerClip;
    public AudioClip losePowerClip;

    public void PoweredInteractableInitialize()
    {
        if (GetComponent<DoorButton>() != null)
        {
            pdb = GetComponent<DoorButton>();
            pt = PoweredType.DoorButton;
        }
        else if (GetComponent<HeavyDoor>() != null)
        {
            hd = GetComponent<HeavyDoor>();
            pt = PoweredType.HeavyDoor;
        }
        else
        {
            pt = PoweredType.None;
        }

        //Start() will only run on the "lowest-level" class
        //If Class A that derives from Class B does not implement Start(),
        //Class B Start() will be used.
        //If Class A & Class B both have Start(), Start() will be called on Class A because it is "lower"
        InteractableInitialize();
    }

    void OnPower(bool status)
    {
        Debug.Log("PoweredInteractable OnPower() is being used!");

        switch (pt)
        {
            case PoweredType.DoorButton:
                pdb.OnPowered(status);
                return;
            case PoweredType.HeavyDoor:
                hd.OnPowered(status);
                return;
            case PoweredType.None:
                Debug.Log("No OnPower behavior for this powered interactable object found!");
                return;
        }
    }
}
