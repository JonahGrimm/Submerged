using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedDoorButton : PoweredInteractable
{
    public GameObject[] connectedObjects;
    private Coroutine timedDelay;
    public int timePowered = 6;
    private AudioSource source;
    private static List<TimedDoorButton> allButtons = new List<TimedDoorButton>();

    private void Start()
    {
        source = GetComponent<AudioSource>();
        PoweredInteractableInitialize();
        timedDelay = null;
        allButtons.Add(this);
    }

    public static void StopAllTimers()
    {
        foreach (TimedDoorButton b in allButtons)
        {
            if (b.timedDelay != null)
            {
                b.StopCoroutine(b.timedDelay);
                b.source.Stop();
                b.source.loop = false;
            }
        }
    }

    public void Interaction(GameObject playerObject)
    {
        //Debug.Log("DoorButton Interaction() is being used!");

        source.clip = interactSoundEffect;
        source.Play();
        source.loop = false;

        if (IsPowered)
        {
            ButtonActivated ba;
            foreach (GameObject go in connectedObjects)
            {
                ba = go.GetComponentInChildren<ButtonActivated>();
                ba.Activate(playerObject, true);
            }

            if (timedDelay != null)
                StopCoroutine(timedDelay);

            timedDelay = StartCoroutine("Wait");
        }
        else
        {
            //Debug.Log("The button is not powered and would not activate.");
        }
    }

    IEnumerator Wait()
    {
        source.clip = onPowerClip;
        source.Play();
        source.loop = true;

        yield return new WaitForSeconds(timePowered);     

        ButtonActivated ba;
        foreach (GameObject go in connectedObjects)
        {
            ba = go.GetComponentInChildren<ButtonActivated>();

            //This is disgusting...
            ba.Activate(gameObject, false);
        }

        source.Stop();
        source.loop = false;
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
