using System;
using UnityEngine;

public class PowerCube : Interactable
{
    private void Start()
    {
        InteractableInitialize();
    }

    public void Interaction(GameObject playerObject)
    {
        //Debug.Log("Power Cube behavior is being used!");
        PlayerMove pm = playerObject.GetComponent<PlayerMove>();
        pm.PowerCubes++;
        AudioSource source = playerObject.GetComponent<AudioSource>();
        source.clip = interactSoundEffect;
        source.Play();
        Destroy(transform.parent.gameObject);
    }
}
