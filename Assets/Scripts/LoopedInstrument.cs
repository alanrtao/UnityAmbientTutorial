using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using FMOD.Studio;
using FMODUnity;

public class LoopedInstrument : Instrument
{
    // the actual instance of the event
    protected EventInstance event_instance;

    // sends a stop signal to the ongoing event
    public bool Halt(bool forced)
    {
        try
        {
            if (event_instance.isValid())
            {
                if (forced)
                {
                    event_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                } else
                {
                    event_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
                event_instance.release();
            } else
            {
                throw new UnityException("Trying to stop a non-existent event");
            }
        } catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        return true;
    }

    public override void Invoke()
    {
        // halt existing event before starting a new one
        Halt(false);

        event_instance = RuntimeManager.CreateInstance(reference);
        event_instance.start();
    }

    public override void UpdateState(float t)
    {
        //throw new System.NotImplementedException();
        if (event_instance.isValid())
        {
            event_instance.setVolume(gain * GetNoisyGain(t));
        }
    }
}
