using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

enum InteractableType
{
    PowerCube,
    PowerStation,
    LightSwitch,
    DoorButton
}
//Interactable is just a "middle-man" class for the player object to communicate with the interactable
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    private PowerCube pc;
    private PowerStation ps;
    private LightSwitch ls;
    private DoorButton db;
    private InteractableType it;
    public AudioClip interactSoundEffect;
    public Mesh mesh;

    public void InteractableInitialize()
    {
        //In case I forget...
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if (GetComponent<PowerCube>() != null)
        {
            Debug.Log(name + " found PowerCube.cs!");

            pc = GetComponent<PowerCube>();
            it = InteractableType.PowerCube;
        }
        else if (GetComponent<PowerStation>() != null)
        {
            Debug.Log(name + " found PowerStation.cs!");

            ps = GetComponent<PowerStation>();
            it = InteractableType.PowerStation;
        }
        else if (GetComponent<LightSwitch>() != null)
        {
            Debug.Log(name + " found LightSwitch.cs!");

            ls = GetComponent<LightSwitch>();
            it = InteractableType.LightSwitch;
        }
        else if (GetComponent<DoorButton>() != null)
        {
            Debug.Log(name + " found DoorButton.cs!");

            db = GetComponent<DoorButton>();
            it = InteractableType.DoorButton;
        }
        else
        {
            Debug.Log(name + " found nothing at all!");
        }

        //If universal functionality is needed, that will go here
    }

    public void Interact(GameObject playerObject)
    {
        Debug.Log("Interactable is doing Interact()!");

        switch (it)
        {
            case InteractableType.PowerCube:
                pc.Interaction(playerObject);
                return;
            case InteractableType.PowerStation:
                ps.Interaction(playerObject);
                return;
            case InteractableType.LightSwitch:
                ls.Interaction(playerObject);
                return;
            case InteractableType.DoorButton:
                db.Interaction(playerObject);
                return;
            default:
                Debug.Log("No behavior for this interactable found!");
                return;
        }
    }
}
