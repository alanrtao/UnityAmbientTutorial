using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMOD.Studio;
using FMODUnity;

public abstract class Instrument : MonoBehaviour
{

    // get-only reference to the fmod event
    public EventReference reference;

    // the master volume
    public float gain;

    // the speed by which the volume varies
    [Range(0, 10)]
    public float variation;

    // the amplitude (percentage) by which the volume varies
    [Range(0, 1)]
    public float fluctuation = .2f;

    // update the current state of the instrument based on time
	// (and possibly other variables, but that is outside the scope)
    public abstract void UpdateState(float t);

    // start playing the instrument
    public abstract void Invoke();

    // a random starting point per object for perlin noise
    float noise_root = Random.value * 1000f;

    // returns the gain level after adding perlin noise
    // the final volume exists within the interval [1-fluctuation, 1+fluctuation]
    public float GetNoisyGain(float t)
    {
        return 1 + 2 * fluctuation * (Mathf.PerlinNoise(
            // the position of the sampling point on the noise field
            noise_root,
            t * variation
            ) - .5f);
    }
}