using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceController : MonoBehaviour
{
    public AudioClip[] ambiances;
    private AudioSource source;
    private int framesUntilNextSound;
    public int minSec = 15;
    public int maxSec = 40;

    private void Start()
    {
        SetDelay();
        source = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        SetDelay();
        source = GetComponent<AudioSource>();
    }

    void SetDelay()
    {
        framesUntilNextSound = Random.Range(60 * minSec, 50 * maxSec);
    }

    private void FixedUpdate()
    {
        framesUntilNextSound--;
        if (framesUntilNextSound <= 0)
        {
            SetDelay();
            int index = Random.Range(0, ambiances.Length - 1);
            source.clip = ambiances[index];
            source.Play();
        }
    }
}
