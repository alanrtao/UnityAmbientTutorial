using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using FMOD.Studio;
using FMODUnity;

public class Song : MonoBehaviour
{
    [Range(0, 1)]
    public float gain;

    public LoopedInstrument background, whisper;
    public SingularInstrument flute, vibraslap;

    float background_gain, whisper_gain, flute_gain, vibraslap_gain;

    // Start is called before the first frame update
    void Start()
    {
        // store initial variables
        background_gain = background.gain;
        whisper_gain = whisper.gain;
        flute_gain = flute.gain;
        vibraslap_gain = vibraslap.gain;
    }

    private void OnEnable()
    {
        // start looped instruments
        background.Invoke();
        whisper.Invoke();

        // start randomized singular note player
        StartCoroutine(NotePlayer(flute, 1, .3f));
        StartCoroutine(NotePlayer(vibraslap, .5f, .5f));
    }

    IEnumerator NotePlayer(SingularInstrument i, float interval, float chance)
    {
        while (enabled)
        {
            float t = Time.time;
            // pause until a fixed interval comes up
            while (Time.time - t <= interval)
            {
                yield return null;
            }
            // upon the interval, decide if a note should be played based on
            // random number
            if (UnityEngine.Random.value <= chance)
            {
                i.Invoke();
            }
        }
    }

    // stop all notes from playing
    private void OnDisable()
    {
        // halt looped instruments
        background.Halt(false);
        whisper.Halt(false);
        // stop note players
        StopAllCoroutines();
    }

    void FixedUpdate()
    {
        background.UpdateState(Time.time);
        background.gain = gain * background_gain;

        whisper.UpdateState(Time.time);
        whisper.gain = gain * whisper_gain;

        flute.UpdateState(Time.time);
        flute.gain = gain * flute_gain;

        vibraslap.UpdateState(Time.time);
        vibraslap.gain = gain * vibraslap_gain;
    }
}
