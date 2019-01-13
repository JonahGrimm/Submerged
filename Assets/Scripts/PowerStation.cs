using System;
using UnityEngine;

public class PowerStation : Interactable
{
    public Transform cubeSocket;
    public GameObject displayPowerCube;
    private bool hasCube = false;
    private GameObject activeCubeObject;
    public AudioClip removeSoundEffect;
    public float powerRange = 5f;
    private PlayerMove pm;

    public Vector3 tempDirection;
    public float distance;

    private void Start()
    {
        InteractableInitialize();
    }

    public void Interaction(GameObject playerObject)
    {
        Debug.Log("Power Station Interaction() is being used!");

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
        Debug.Log("Power Station PowerNearbyObjects() is being used!");

        RaycastHit[] foundObjects = Physics.SphereCastAll(transform.position, powerRange, Vector3.forward, distance, pm.interactableLayerMask);

        Debug.Log("Found " + foundObjects.Length + " objects.");

        foreach (RaycastHit potentialObject in foundObjects)
        {
            var powerComponent = potentialObject.transform.gameObject.GetComponent<PoweredInteractable>();

            if (powerComponent != null)
            {
                if (status)
                    powerComponent.Power++;
                else
                    powerComponent.Power--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, powerRange);
    }
}
