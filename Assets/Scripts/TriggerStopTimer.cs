using System;
using UnityEngine;

public class TriggerStopTimer : MonoBehaviour
{
    private Collider c;

    private void Start()
    {
        c = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TimedDoorButton.StopAllTimers();
        }
    }
}
