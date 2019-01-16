using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineImpulseActivator : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    public bool activate;
    public Vector3 velocity;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (activate)
        {
            if (velocity != Vector3.zero)
                impulseSource.GenerateImpulse(velocity);
            else
                impulseSource.GenerateImpulse();

            activate = false;
        }
    }
}
