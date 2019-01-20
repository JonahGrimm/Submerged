using System;
using UnityEngine;

public class PoweredLight : PoweredInteractable
{
    private Light lightComponent;
    private AudioSource source;

    private void Start()
    {
        PoweredInteractableInitialize();
        lightComponent = GetComponent<Light>();
        source = GetComponent<AudioSource>();
    }

    public void OnPowered(bool status)
    {
        if (status)
        {
            lightComponent.enabled = true;
            source.clip = onPowerClip;
            source.Play();
        }
        else
        {
            lightComponent.enabled = false;
            source.clip = losePowerClip;
            source.Play();
        }
    }
}
