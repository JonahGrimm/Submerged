using System;
using UnityEngine;

public class PoweredDialogueActivator : PoweredInteractable
{
    public DialogueText textObject;
    private Collider col;
    public bool activate;           //This variable is reserved for cutscenes and debugging primarily
    private bool activated = false;

    private void Start()
    {
        col = GetComponent<Collider>();
        PoweredInteractableInitialize();
        col.enabled = false;
        activated = false;
    }

    public void OnPowered(bool status)
    {
        if (status && !activated)
        {
            activated = true;
            col.enabled = true;
        }
    }

    private void Update()
    {        
        if (activate)
        {
            activate = false;
            SendText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SendText();
            col.enabled = false;
        }
    }

    private void SendText()
    {
        //Disgusting. But it works... I'd definitely revise this for bigger projects/multiplayer games
        DialogueBox db = GameObject.Find("DialogueBox").GetComponent<DialogueBox>();
        db.ReadText(textObject);
    }
}
