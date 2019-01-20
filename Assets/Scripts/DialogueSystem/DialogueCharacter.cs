using System;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character", order = 1)]
public class DialogueCharacter : ScriptableObject
{
    public new string name;
    public TMP_FontAsset font;
    public AudioClip[] charSounds;
    public Color color;
    public float volume;
    public float minPitch;
    public float maxPitch;
}
