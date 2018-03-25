using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// make me a child of the enemy prefab, give 3 some audio sources
/// </summary>
public class EnemyAudioScript : MonoBehaviour {

    public bool isTrappedInWeb;
    public AudioClip[] trappedInWebSounds;

    public AudioClip[] eatenSounds;

    public AudioClip[] damagePlayerSounds;

	// Use this for initialization
	void Start () {
        transform.position = transform.parent.position;
        isTrappedInWeb = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTrappedInWeb() {
        isTrappedInWeb = true;
        int index = Random.Range(0, trappedInWebSounds.Length - 1 );
        foreach(AudioSource aSource in GetComponents<AudioSource>()) {
            if (!aSource.isPlaying) {
                aSource.clip = trappedInWebSounds[index];
                aSource.Play();
                return;
            }
        }
    }

    public void HitPlayer() {
        
        //find audiosource, play damage player sound
        int index = Random.Range(0, damagePlayerSounds.Length - 1);
        foreach (AudioSource aSource in GetComponents<AudioSource>()) {
            if (!aSource.isPlaying) {
                aSource.clip = damagePlayerSounds[index];
                aSource.Play();
                return;
            }
        }
        
    }
}
