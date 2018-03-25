using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use this for continuous scraping sound that changes pitch and volume
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ScrapingScript : MonoBehaviour {

    //use whatever values make sense for min and max
    public float overlapAmount;
    public float overlapMin;
    public float overlapMax;


    public float speedValue;
    public float speedMin;
    public float speedMax;


    public float pitchMin = 0.5f;
    public float pitchMax = 1.5f;

    public float volMin = 0.0f;
    public float volMax = 0.5f;
    AudioSource aSource;


    void Start() {
        aSource = GetComponent<AudioSource>();
    }

    void Update() {

        float newVolume = RemapRange(speedValue, speedMin, speedMax, volMin, volMax);
        aSource.volume = Mathf.Lerp(aSource.volume, newVolume, 0.5f);
        //aSource.volume = RemapRange(speedValue, speedMin, speedMax, volMin, volMax);

        float newPitch = RemapRange(overlapAmount, overlapMin, overlapMax, pitchMin, pitchMax);
        aSource.pitch = Mathf.Lerp(aSource.pitch, newPitch, Time.deltaTime);

    }

    /// <summary>
    /// Remaps a value from one range to another
    /// </summary>
    /// <returns>The remapped value</returns>
    /// <param name="inputVal">Input value.</param>
    /// <param name="lowIn">Input Low Bound</param>
    /// <param name="highIn">Input High Bound</param>
    /// <param name="lowOut">Output Low Bound</param>
    /// <param name="highOut">Output High Bound</param>
    public float RemapRange(float inputVal, float lowIn, float highIn, float lowOut, float highOut) {
        return (((inputVal - lowIn) / (highIn - lowIn)) * (highOut - lowOut) + lowOut);
    }

}
