using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LockPositionToTarget : MonoBehaviour
{
    public Transform target;
    public bool duringCinematic;
    private PlayableDirector director;

    private void Start()
    {
        if (GameObject.Find("Cutscene Director"))
            director = GameObject.Find("Cutscene Director").GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (director !=null)
        {
            if (duringCinematic && director.state == PlayState.Playing)
                transform.position = target.position;

            if (director.state != PlayState.Playing)
                transform.position = target.position;
        }
        else
        {
            transform.position = target.position;
        }
    }
}
