using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anyKeyToSTart : MonoBehaviour {
    int currentSceneIndex;
    int nextSceneIndex;
	// Use this for initialization
	void Start () {

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex + 1;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown) {
            SceneManager.LoadScene(nextSceneIndex);
        }
	}
}
