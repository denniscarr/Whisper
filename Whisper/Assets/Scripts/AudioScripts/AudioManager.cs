using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    //we may not use this
    public GameObject audioPrefab;

    //populate this with shorter ASMR spoken segments.  These will be fed into a shuffle bag.
    public AudioClip[] scriptClips;
    //populate this with clips called "drone1-1," "drone 1-2" etc
    public AudioClip[] droneClips;

    public AudioSource playerLoseSource;

    //for going through droneClips
    int droneIndex = 0;

    //Assign the drone source from a different game object
    public AudioSource droneSource;

    //map drone source volume from droneSourceMinVolume to droneSourceMaxVolume based
    //on image coalescing
    public float droneSourceVolume;
    float droneSourceMinVolume = 0.05f;
    float droneSourceMaxVolume = 0.5f;


    //Attach 3 sources to start - we can add more if we want
    public List<AudioSource> asmrScriptSources;

    public int maxASMRBackgroundSources = 5;

    //we may vary this later, not sure;
    public float scriptSourcePitch;

    //we're going to use these timers to stagger audio sources by putting in 
    //variable length pauses between clips
    public float scriptSourceTimer;
    float nextScriptStartDelay = 10f;

    public float droneSourceTimer;
    public float nextDroneStartDelay = 2f;

    public static ShuffleBag<AudioClip> ASMRcue;

    public AudioMixerGroup spokenWordGroup;
    public AudioMixerSnapshot normalGameSnapshot, pausedSnapshot, gameOverSnapshot;

    int numLinks;


    void Awake() {

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        numLinks = 0;

        //AudioSource newSource = gameObject.AddComponent<AudioSource>();
        //newSource.playOnAwake = false;
        //newSource.loop = false;
        asmrScriptSources = new List<AudioSource>();
        // asmrScriptSources.Add(newSource);

        if (!IsValidToneCue()) {
            ASMRcue = new ShuffleBag<AudioClip>();

            AddScriptsToCue();
        }
    }

	// Use this for initialization
	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
        //update ASMR Timers
        for (int i = 0; i < asmrScriptSources.Count; i++) {
            //check if audiosource is not playing
            if (!asmrScriptSources[i].isPlaying) {
                //update timer
                scriptSourceTimer += Time.deltaTime;
                //check if timer is greater than start delay
                if (scriptSourceTimer >= nextScriptStartDelay) {
                    //play the next clip in the shuffle bag and reset the timer
                    asmrScriptSources[i].clip = ASMRcue.Next();
                    asmrScriptSources[i].Play();
                    scriptSourceTimer = 0f;
                    nextScriptStartDelay = 10f + Random.Range(-2f, 2f);
                }
            }
        }

        //Update Drone Timers
        if(!droneSource.isPlaying) {
            droneSourceTimer += Time.deltaTime;
            if(droneSourceTimer > nextDroneStartDelay) {
                //reset timer, update drone index, assign clip and play
                droneSourceTimer = 0f;
                droneIndex++;
                droneSource.clip = droneClips[droneIndex % droneClips.Length];
                droneSource.Play();
            }
        }

        //Update Drone Volume

        droneSourceVolume = MyMath.Map(numLinks, 0, 10, 0f, 0.5f);

	}

    bool IsValidToneCue() {
        return ASMRcue != null;
    }

    void AddScriptsToCue() {
        foreach (AudioClip asmr in scriptClips) {
            ASMRcue.Add(asmr);
        }
    }

    public void AddLink() {
        if (numLinks < 10) {
            numLinks++;
        }

        if (asmrScriptSources.Count < maxASMRBackgroundSources) {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            asmrScriptSources.Add(newSource);
            newSource.playOnAwake = false;
            newSource.loop = false;
            newSource.outputAudioMixerGroup = spokenWordGroup;
        }
    }

    public void RemoveLink() {
        numLinks--;
        if(asmrScriptSources.Count > 0) {
            asmrScriptSources[asmrScriptSources.Count - 1].Stop();
            AudioSource source = asmrScriptSources[asmrScriptSources.Count - 1];
            asmrScriptSources.Remove(source);
            Destroy(source);

        }

    }

    public void RemoveAllLinks() {
        numLinks = 0;
        if (asmrScriptSources.Count > 0) {
            for (int i = asmrScriptSources.Count - 1; i >= 0; i--) {
                AudioSource source = asmrScriptSources[i];
                source.Stop();
                asmrScriptSources.Remove(asmrScriptSources[i]);
                Destroy(source);
            }
        }
        asmrScriptSources.Clear();
    }

    public void PauseGame() {
        pausedSnapshot.TransitionTo(0.0f);
    }
    public void UnPauseGame() {
        normalGameSnapshot.TransitionTo(0.3f);
    }

    public void KillPlayer() {
        gameOverSnapshot.TransitionTo(0.3f);
        RemoveAllLinks();
        playerLoseSource.Play();
    }

    public void RestartGame() {
        numLinks = 0;
        normalGameSnapshot.TransitionTo(0.3f);
        if (!IsValidToneCue()) {
            ASMRcue = new ShuffleBag<AudioClip>();

            AddScriptsToCue();
        }
    }


}
