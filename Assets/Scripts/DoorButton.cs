using System;
using UnityEngine;

public class DoorButton : PoweredInteractable
{
    public GameObject[] connectedObjects;

    private void Start()
    {
        PoweredInteractableInitialize();
    }

    public void Interaction(GameObject playerObject)
    {
        Debug.Log("DoorButton Interaction() is being used!");

        AudioSource source = playerObject.GetComponent<AudioSource>();

        source.clip = interactSoundEffect;
        source.Play();

        if (IsPowered)
        {
            ButtonActivated ba;
            foreach (GameObject go in connectedObjects)
            {
                ba = go.GetComponent<ButtonActivated>();
                ba.Activate(playerObject);
            }

        }
        else
        {
            Debug.Log("The button is not powered and would not activate.");

        }
    }

    public void OnPowered(bool status)
    {
        Debug.Log("DoorButton OnPowered() is being used!");

        ButtonActivated ba;
        foreach (GameObject go in connectedObjects)
        {
            ba = go.GetComponent<ButtonActivated>();
            ba.OnPower(status);
        }
    }
}
