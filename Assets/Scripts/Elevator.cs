using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : ButtonActivated
{    
    public AudioSource _elevatorSnd = null; //The source to play sounds
    public AudioSource activationSource;
    public AudioClip loopClip;              //Sound that is played during a move
    public AudioClip startClip;             //Sound that is played at the start of a move
    public AudioClip endClip;               //Sound that is played after a move
    public Animator _animator;              //Reference to the animator component    
    public bool defaultPosition = false;    //Whether the default position of the elevator is up (true) or down (false)
    private bool currentPosition = false;   //The current position of the elevator

    public void OnPowered(bool status)
    {
        if (status)
        {
            activationSource.clip = onPowerClip;
            activationSource.Play();
        }
        else
        {
            activationSource.clip = losePowerClip;
            activationSource.Play();

            //For some reason... this is true when the currentPosition is NOT at the default position...
            if (currentPosition == defaultPosition)
            {
                currentPosition = !defaultPosition;

                MovePlatform(defaultPosition);

                activationSource.clip = startClip;
                activationSource.Play();
                _elevatorSnd.Play();
            }
        }
    }

    public void ToggleObj(GameObject playerObject)
    {
        if (currentPosition)
        {
            //Tell the method that we are currently in the up position
            MovePlatform(true);
            activationSource.clip = startClip;
            activationSource.Play();
            currentPosition = false;
            _elevatorSnd.Play();
        }
        else
        {
            //Tell the method that we are currently in the down position
            MovePlatform(false);
            activationSource.clip = startClip;
            activationSource.Play();
            currentPosition = true;
            _elevatorSnd.Play();
        }
    }

    public void Activation(GameObject playerObject, bool status)
    {
        //If we are currently in the default position and want to move opposite
        if (currentPosition != defaultPosition && status)
        {
            MovePlatform(!defaultPosition);
            activationSource.clip = startClip;
            activationSource.Play();
            currentPosition = defaultPosition;
            _elevatorSnd.Play();
        }
        //If we are NOT currently in the default position and want to move opposite
        else if (currentPosition == defaultPosition && !status)
        {
            MovePlatform(defaultPosition);
            activationSource.clip = startClip;
            activationSource.Play();
            currentPosition = !defaultPosition;
            _elevatorSnd.Play();
        }
    }

    void StoppedMoving()
    {
        _elevatorSnd.Stop();
    }

    //-------

    //From DotHskDoorSlide.cs
    private void Start()
    {
        Debug.Log("Elevator.cs used Start()!");

        ButtonActivatedInitialize();

        currentPosition = !defaultPosition;

        MovePlatform(defaultPosition);
    }


    //From DotHskDoorSlide.cs
    void MovePlatform(bool position)
    { // true - Up, false - Down
        string _anim = "Elevator_" + ((position) ? "up" : "dn");

        Debug.Log(_anim);

        if ((_animator != null))
        {
            AnimatorStateInfo _st = _animator.GetCurrentAnimatorStateInfo(0);
            if (!_st.IsName(_anim))
            {
                float _time = _st.normalizedTime;
                _time = (_time < 1.0f && (_st.IsName("Elevator_up") || _st.IsName("Elevator_dn"))) ? 1 - _time : 0.0f;
                 _animator.Play(_anim, -1, _time);
            }
        }
    }
}
