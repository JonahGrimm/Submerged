using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text", menuName = "Dialogue/Text", order = 1)]
public class DialogueText : ScriptableObject
{
    [TextArea(3,50)]
    public string text = "<b>Bold.</b> <i>Italics.</i> <u>Underlined.</u> <color=red>Red text.</color> \nNew line. <n>New box";
    public DialogueCharacter[] actors;
    public float timeUntilFade;
}
