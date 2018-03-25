using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use this for changing enemy drone pitch based on speed
/// Add me to enemy prefab, set audiosource to play on awake TRUE; Volume 0.1; full 3D
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EnemyDroneScript : MonoBehaviour {

    public float speedValue;
    public float speedMin;
    public float speedMax;

    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;
    AudioSource aSource;


    void Start() {
        aSource = GetComponent<AudioSource>();
    }

    void Update() {

        speedValue = GetComponent<Rigidbody>().velocity.magnitude;
        Mathf.Clamp(speedValue, speedMin, speedMax);
        float newPitch = RemapRange(speedValue, speedMin, speedMax, pitchMin, pitchMax);
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
