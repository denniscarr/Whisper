using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {

    public bool paused = false;

    public GameObject quitButton;


    void Start() {
        Pause(false);
        quitButton.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!paused) {
                Pause(true);
                quitButton.gameObject.SetActive(true);
            } else {
                Pause(false);
                quitButton.gameObject.SetActive(false);
            }
        }
    }

    void OnGUI() {
        if (paused) {
            quitButton.SetActive(true);
            if (Input.GetButtonDown("Submit")) {
                SceneManager.LoadScene(0);
                Pause(false);
            }
        }
    }

    void Pause(bool value) {
        if (value == false) {
            paused = false;
            //unpause game
            AudioManager.Instance.UnPauseGame();
            Time.timeScale = 1f;
        }
        else {
            //pause game
            paused = true;
            AudioManager.Instance.PauseGame();
            Time.timeScale = 0f;
        }
    }

    public void mainMenu() {
        SceneManager.LoadScene(0);
        Pause(true);
    }


    public void quitGame() {
        Application.Quit();

    }
    public void restart() {
        Pause(false);
        int thisBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(thisBuildIndex);
    }

}
