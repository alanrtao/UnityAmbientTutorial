using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using FMOD.Studio;
using FMODUnity;

public class SingularInstrument : Instrument
{
    float underlying_gain;

    public override void Invoke()
    {
        try
        {
            // instantiate a new event and play it
            EventInstance e = RuntimeManager.CreateInstance(reference);

            // random pan from -1 to 1
            e.setParameterByName("Pan", UnityEngine.Random.value * 2f - 1f);
            e.setVolume(underlying_gain);

            // random pitch, +- 12 semitones (basically +- 1 octave
            float note = Mathf.Round(UnityEngine.Random.value * 24 - 12);
            e.setPitch(Mathf.Pow(2, note / 12f));

            e.start();
            // stop caring about the event after playing, since it is singular
            e.release();
        } catch (Exception e)
        {
            // the note could not be played for some reason
            Debug.Log(e);
        }
    }

    public override void UpdateState(float t)
    {
        // update the volume of the instrument
        underlying_gain = gain * GetNoisyGain(t);
    }
}
