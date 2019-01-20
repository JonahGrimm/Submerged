using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] sounds;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        int index = Random.Range(0, sounds.Length - 1);
        source.clip = sounds[index];
        source.Play();
    }
}
