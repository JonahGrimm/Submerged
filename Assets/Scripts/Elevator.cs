using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : ButtonActivated
{    
    public AudioSource _elevatorSnd = null; //The source to play sounds
    public AudioClip loopClip;              //Sound that is played during a move
    public AudioClip startClip;             //Sound that is played at the start of a move
    public AudioClip endClip;               //Sound that is played after a move
    public Animator _animator;              //Reference to the animator component    
    public bool defaultPosition = false;    //Whether the default position of the elevator is up (true) or down (false)
    private bool currentPosition = false;   //The current position of the elevator
    private bool isMoving = false;          //Whether the elevator is moving

    public void OnPowered(bool status)
    {
        Debug.Log("Elevator OnPowered() is being used!");

        if (status)
        {
            _elevatorSnd.clip = onPowerClip;
            _elevatorSnd.Play();
        }
        else
        {
            _elevatorSnd.clip = losePowerClip;
            _elevatorSnd.Play();

            if (currentPosition == defaultPosition)
            {
                currentPosition = !defaultPosition;

                MovePlatform(defaultPosition);

                _elevatorSnd.clip = startClip;
                _elevatorSnd.Play();
            }
        }
    }

    public void Activation(GameObject playerObject)
    {
        Debug.Log("Elevator Activation() is being used!");

        if (currentPosition)
        {
            currentPosition = false;
            MovePlatform(true);
            _elevatorSnd.clip = startClip;
            _elevatorSnd.Play();
        }
        else
        {
            currentPosition = true;
            MovePlatform(false);
            _elevatorSnd.clip = startClip;
            _elevatorSnd.Play();
        }

        //isMoving = true;
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
