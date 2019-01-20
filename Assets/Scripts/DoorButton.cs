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
        //Debug.Log("DoorButton Interaction() is being used!");

        AudioSource source = playerObject.GetComponent<AudioSource>();

        source.clip = interactSoundEffect;
        source.Play();

        if (IsPowered)
        {
            ButtonActivated ba;
            foreach (GameObject go in connectedObjects)
            {
                ba = go.GetComponentInChildren<ButtonActivated>();
                ba.Toggle(playerObject);
            }

        }
        else
        {
            //Debug.Log("The button is not powered and would not activate.");

        }
    }

    public void OnPowered(bool status)
    {
        //Debug.Log("DoorButton OnPowered() is being used!");

        ButtonActivated ba;
        foreach (GameObject go in connectedObjects)
        {
            ba = go.GetComponentInChildren<ButtonActivated>();
            ba.OnPower(status);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (connectedObjects.Length > 0)
        {
            for (int i = 0; i < connectedObjects.Length; i++)
            {
                if (connectedObjects[i] != null)
                    Gizmos.DrawLine(transform.position, connectedObjects[i].transform.position);
            }
        }
    }
}
