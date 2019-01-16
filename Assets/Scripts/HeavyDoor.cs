using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDoor : PoweredInteractable
{
    // References
    private Animator _animator = null;
    private AudioSource _gateSnd = null;

    void Start()
    {
        PoweredInteractableInitialize();

        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Gate2":
                    _animator = child.GetComponent<Animator>(); break;
                case "Gate2_Sound":
                    _gateSnd = child.GetComponent<AudioSource>(); break;
            }
        }
    }

    public void OnPowered(bool status)
    {
        //From HeavyStation Package
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float time = stateInfo.normalizedTime;
        time = (time < 1.0f && (stateInfo.IsName("G1_Open") || stateInfo.IsName("G1_Close"))) ? 1 - time : 0.0f;

        if (status)
        {
            _gateSnd.clip = onPowerClip;
            _gateSnd.Play();
            _animator.Play("G1_Open", -1, time);
        }
        else
        {
            _gateSnd.clip = losePowerClip;
            _gateSnd.Play();
            _animator.Play("G1_Close", -1, time);
        }
    }
}
