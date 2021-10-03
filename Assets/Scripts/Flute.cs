using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using FMOD.Studio;
using FMODUnity;


public class Flute : SingularInstrument
{

    float last_note = 0;
    float root = 0;

    float[] scale = { 0, 2, 5, 7, 9 };
    float[] domain;

    private void Start()
    {
        domain = new float[scale.Length * 5];
        int c = 0;
        for (int i = -3; i < 2; i++)
        {
            for (int j = 0; j < scale.Length; j++, c++)
            {
                float octave = 12 * i;
                domain[c] = scale[j % scale.Length] + octave;
            }
        }
    }

    public void Flush () {
        // can start an octave higher, lower, or normal
        root = (Mathf.Round(UnityEngine.Random.value * 3) - 1) * 12;
        last_note = Mathf.Round(UnityEngine.Random.value + 1) * scale.Length;
    }

    public override void Invoke()
    {
        float note = last_note;
        if (note == 0) note = 1;
        else if (note == domain.Length - 1) note -= 1;
        else if (UnityEngine.Random.value < .5f) note--;
        else note++;

        //n note = Mathf.Clamp(note, 0, domain.Length - 1);
        last_note = note;
        note = root + domain[Mathf.RoundToInt(note)];
        float pan = UnityEngine.Random.value * 2 - 1;
        EventInstance ei = RuntimeManager.CreateInstance(reference);
        ei.setParameterByName("Pan", pan);
        ei.setVolume(underlying_gain);

        // random pitch, +- 12 semitones (basically +- 1 octave
        // float note = 2 * Mathf.Round(UnityEngine.Random.value * 12 - 6);
        ei.setPitch(Mathf.Pow(2, note / 12f));
        ei.start();
        ei.release();

        if (UnityEngine.Random.value < 0.5f)
        {
            // play another note at the same time, one octave lower
            ei = RuntimeManager.CreateInstance(reference);
            ei.setParameterByName("Pan", pan);
            ei.setVolume(underlying_gain);

            // random pitch, +- 12 semitones (basically +- 1 octave
            // float note = 2 * Mathf.Round(UnityEngine.Random.value * 12 - 6);
            ei.setPitch(Mathf.Pow(2, (note + 12) / 12f));
            ei.start();
            ei.release();
        }
    }  

}