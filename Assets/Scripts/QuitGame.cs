using UnityEngine;
using Rewired;
using TMPro;

public class QuitGame : MonoBehaviour
{
    public TextMeshProUGUI quitText;
    private Player p;
    private bool quitGameState;     //false-Playing game  true-Asking to quit

    //Inputs
    private bool exitDown;
    private float hor;
    private float ver;
    private bool interact;

    private void Start()
    {
        p = ReInput.players.GetPlayer(0);
    }

    private void FixedUpdate()
    {
        exitDown = p.GetButtonDown("Exit");
        hor = p.GetAxis("Move Horizontal");
        ver = p.GetAxis("Move Vertical");
        interact = p.GetButtonDown("Interact");

        //Quit Game Checks
        if (exitDown && !quitGameState)
        {
            quitGameState = true;
            quitText.enabled = true;
        }
        else if (exitDown && quitGameState)
        {
            //Quit game
            Application.Quit();
        }
        else if (quitGameState && (hor != 0 || ver != 0 || interact != false))
        {
            quitGameState = false;
            quitText.enabled = false;
        }
    }
}
