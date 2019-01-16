using System;
using UnityEngine;

public class PowerStation : Interactable
{
    public Transform cubeSocket;                    //Physical socket to instantiate the display power cube
    public GameObject displayPowerCube;             //A reference to the gameobject that will be instantiated
    public AudioClip removeSoundEffect;             //Sound effect for when the cube is removed (interact sound effect supplied by Interactable)
    public PoweredInteractable[] connectedObjects;  //A reference to objects that will be affected by this station being powered/un-powered

    private GameObject activeCubeObject;            //A reference to the live gameobject cube if it exists
    private bool hasCube = false;                   //Status of whether the station is holding a cube or not
    private PlayerMove pm;                          //A reference to PlayerMove so it can give back cubes

    private void Start()
    {
        InteractableInitialize();
    }

    public void Interaction(GameObject playerObject)
    {
        AudioSource source = playerObject.GetComponent<AudioSource>();

        pm = playerObject.GetComponent<PlayerMove>();

        if (hasCube)
        {
            Destroy(activeCubeObject);
            hasCube = false;
            pm.PowerCubes++;

            source.clip = removeSoundEffect;
            source.Play();

            PowerNearbyObjects(false);

            return;
        }
        else if (pm.PowerCubes <= 0)
        {
            return;
        }

        pm.PowerCubes--;

        activeCubeObject = Instantiate(displayPowerCube, cubeSocket);

        hasCube = true;

        source.clip = interactSoundEffect;
        source.Play();

        PowerNearbyObjects(true);
    }

    void PowerNearbyObjects(bool status)
    {
        foreach (PoweredInteractable poweredObject in connectedObjects)
        {
            if (poweredObject != null)
            {
                if (status)
                    poweredObject.Power++;
                else
                    poweredObject.Power--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (connectedObjects.Length > 0)
        {
            for (int i = 0; i < connectedObjects.Length; i++)
            {
                if (connectedObjects[i] != null)              
                    Gizmos.DrawLine(transform.position + Vector3.up, connectedObjects[i].transform.position);
            }
        }
    }
}
