using System;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    public DialogueText textObject;
    private Collider col;
    public bool activate;

    private void Start()
    {
        col = GetComponent<Collider>();
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
        DialogueBox db = GameObject.Find("DialogueBox").GetComponent<DialogueBox>();
        db.ReadText(textObject);
    }
}

