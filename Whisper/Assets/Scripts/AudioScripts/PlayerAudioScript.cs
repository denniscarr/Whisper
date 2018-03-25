using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour {

    AudioSource rubbingSource;
    public AudioSource strandSource;
    public AudioClip rubbingClip;

    public AudioClip strandLoop;


	// Use this for initialization
	void Start () {
        rubbingSource = GetComponent<AudioSource>();
        rubbingSource.clip = rubbingClip;

        strandSource.volume = 0f;
        strandSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayRubbingSourceSound() {
        if (!rubbingSource.isPlaying) {
            rubbingSource.Play();
            rubbingSource.volume = 0.5f;
        }
    }

    public void StopRubbingSourceSound() {
        StartCoroutine("StopRubbingSource");
    }

    public void StartStrandSound() {
        StartCoroutine("BeginStrandSound");
    }
    public void EndStrandSound() {
        strandSource.volume = 0f;
    }

    IEnumerator StopRubbingSource() {
        while (rubbingSource.volume >= 0.01f) {
            rubbingSource.volume -= Time.deltaTime;
            yield return null;
        }
        rubbingSource.Stop();
    }

    IEnumerator BeginStrandSound() {
        while (strandSource.volume < 1.0f) {
            strandSource.volume += Time.deltaTime * 2f;
            yield return null;
        }
    }
}
