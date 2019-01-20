using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueBox : MonoBehaviour
{
    private TextMeshProUGUI textBox;
    private TextMeshProUGUI nameBox;
    private Coroutine dialogueRoutine;
    private DialogueText _dialogue;
    private string curActorName;
    private int s = 0;
    private float _volume;
    public float Volume
    {
        get
        {
            return _volume;
        }
        set
        {
            _volume = value;
            ChangeAudioVolume();
        }
    }

    //Audio
    private AudioClip[] charSounds;
    public AudioSource[] sources;

    //Delivery Speeds
    public float defaultDeliverySpeed = 0.065f;
    public float timeUntilDialogueFades = 3f;
    public int maxCharLength = 105;
    public float fadeTime = 2f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    private float deliverySpeed = 0.02f;
    private int hiddenCharCount = 0;

    void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();
        nameBox = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogueRoutine = null;
        Volume = .24f;
    }

    public void ReadText(DialogueText dialogue)
    {
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);
        _dialogue = dialogue;
        dialogueRoutine = StartCoroutine("DisplayText");
    }

    IEnumerator DisplayText()
    {
        int curActor = 0;
        ChangeSpeed();
        ChangeActor(curActor);
        textBox.text = "";
        textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 1);
        nameBox.color = new Color(nameBox.color.r, nameBox.color.g, nameBox.color.b, 1);
        hiddenCharCount = 0;
        for (int i = 0; i < _dialogue.text.Length; i++)
        {
            yield return new WaitForSeconds(deliverySpeed);

            if (_dialogue.text[i] == '<')
            {
                bool end = false;
                for (int j = 1; i + j < _dialogue.text.Length; j++)
                {
                    switch (_dialogue.text[i + j])
                    {
                        case ' ':
                            end = true;
                            AddText(_dialogue.text[i]);
                            break;

                        case '>':
                            end = true;

                            if (_dialogue.text.Substring(i, 3) == "<s=")
                            {
                                float speed = float.Parse(_dialogue.text.Substring(i + 3, j - 3));
                                ChangeSpeed(speed);
                            }
                            else if (_dialogue.text.Substring(i, 4) == "</s>")
                            {
                                ChangeSpeed();
                            }
                            else if (_dialogue.text.Substring(i, 3) == "<n>")
                            {
                                textBox.text = "";
                                hiddenCharCount = 0;
                            }
                            else if (_dialogue.text.Substring(i, 3) == "<w=")
                            {
                                float waitTime = float.Parse(_dialogue.text.Substring(i + 3, j - 3));
                                yield return new WaitForSeconds(waitTime);
                            }
                            else if (_dialogue.text.Substring(i, 3) == "<c=")
                            {
                                int nextChar = int.Parse(_dialogue.text.Substring(i + 3, j - 3));
                                ChangeActor(nextChar);
                            }
                            else
                            {
                                //Add element plus the following character
                                hiddenCharCount += j + 1;
                                AddText(_dialogue.text.Substring(i, j + 1));
                            }
                            i += j;
                            break;
                    }

                    if (end)
                        break;
                }
            }
            else
            {
                AddText(_dialogue.text[i]);
            }
        }

        yield return new WaitForSeconds(timeUntilDialogueFades);

        float timeElapsed = 0f;
        Color startColor = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 1);
        Color alphaColor = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 0);
        Color startNameColor = new Color(nameBox.color.r, nameBox.color.g, nameBox.color.b, 1);
        Color alphaNameColor = new Color(nameBox.color.r, nameBox.color.g, nameBox.color.b, 0);

        while (timeElapsed < fadeTime)
        {
            timeElapsed += Time.deltaTime;
            textBox.color = Color.Lerp(startColor, alphaColor, timeElapsed / fadeTime);
            nameBox.color = Color.Lerp(startNameColor, alphaNameColor, timeElapsed / fadeTime);
            yield return null;
        }
        textBox.color = alphaColor;
        nameBox.color = alphaNameColor;
    }

    void AddText(char c)
    {
        sources[s].clip = charSounds[UnityEngine.Random.Range(0, charSounds.Length)];
        sources[s].Play();

        s++;
        if (s >= sources.Length)
            s = 0;

        if (textBox.text.Length + 1 > maxCharLength + hiddenCharCount)
        {
            textBox.text = "";
            hiddenCharCount = 0;
        }

        textBox.text += c;
    }

    void AddText(string str)
    {
        sources[s].clip = charSounds[UnityEngine.Random.Range(0, charSounds.Length)];
        sources[s].pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        sources[s].Play();

        s++;
        if (s >= sources.Length)
            s = 0;

        if (textBox.text.Length + str.Length > maxCharLength + hiddenCharCount)
        {
            textBox.text = "";
            hiddenCharCount = 0;
        }

        textBox.text += str;
    }

    void ChangeActor(int index)
    {
        textBox.font = _dialogue.actors[index].font;
        curActorName = _dialogue.actors[index].name;
        charSounds = _dialogue.actors[index].charSounds;
        textBox.color = _dialogue.actors[index].color;
        nameBox.text = curActorName;
        Volume = _dialogue.actors[index].volume;
        minPitch = _dialogue.actors[index].minPitch;
        maxPitch = _dialogue.actors[index].maxPitch;
        timeUntilDialogueFades = _dialogue.timeUntilFade;
    }

    private void ChangeSpeed()
    {
        deliverySpeed = defaultDeliverySpeed;
    }

    private void ChangeSpeed(float speed)
    {
        deliverySpeed = speed;
    }

    private void ChangeAudioVolume()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = Volume;
        }
    }
}
