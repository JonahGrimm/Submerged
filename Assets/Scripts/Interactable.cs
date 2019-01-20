using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

enum InteractableType
{
    PowerCube,
    PowerStation,
    DoorButton,
    TimedDoorButton,
    None
}
//Interactable is just a "middle-man" class for the player object to communicate with the interactable
public class Interactable : MonoBehaviour
{
    private PowerCube pc;
    private PowerStation ps;
    private DoorButton db;
    private TimedDoorButton timd;
    private InteractableType it;
    public AudioClip interactSoundEffect;
    public Mesh meshForHighlightSelection;

    public void InteractableInitialize()
    {
        //In case I forget...
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if (GetComponent<PowerCube>() != null)
        {
            pc = GetComponent<PowerCube>();
            it = InteractableType.PowerCube;
        }
        else if (GetComponent<PowerStation>() != null)
        {
            ps = GetComponent<PowerStation>();
            it = InteractableType.PowerStation;
        }
        else if (GetComponent<DoorButton>() != null)
        {
            db = GetComponent<DoorButton>();
            it = InteractableType.DoorButton;
        }
        else if (GetComponent<TimedDoorButton>() != null)
        {
            timd = GetComponent<TimedDoorButton>();
            it = InteractableType.TimedDoorButton;
        }
        else
        {
            it = InteractableType.None;
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
            case InteractableType.DoorButton:
                db.Interaction(playerObject);
                return;
            case InteractableType.TimedDoorButton:
                timd.Interaction(playerObject);
                return;
            case InteractableType.None:
                Debug.Log("No behavior for this interactable found!");
                return;
        }
    }
}
