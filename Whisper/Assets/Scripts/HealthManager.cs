using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {

    [SerializeField] float invincibilityTime = 0.4f;
    float invincibilityTimer = 0f;
    [SerializeField] GameObject[] healthBoxes;
    public int health = 3;
    ScreenBlur blur;


    public bool isAlive;
    public GameObject endScreen;

    private void Awake() {
        blur = FindObjectOfType<ScreenBlur>();
        endScreen = GameObject.Find("DeathPanel");
        endScreen.SetActive(false);
        isAlive = true;
    }

    private void Update() {
        invincibilityTimer -= Time.deltaTime;

        if (isAlive == false) {
            if (Input.GetKeyDown(KeyCode.Escape)){
                restartScene();
            }
        }
    }

    public void GetHurt() {
        if (invincibilityTimer > 0f) { return; }

        blur.Blink();

        health--;
        Destroy(healthBoxes[health]);

        if (health <= 0) {

            //Application.Quit();
            endScreen.SetActive(true);
            GameObject player = GameObject.Find("Player");
            player.transform.position = new Vector3(1000, 1000, 1000);
            isAlive = false;
            player.GetComponent<PlayerAudioScript>().StopRubbingSourceSound();
            AudioManager.Instance.KillPlayer();
            //Invoke("restartScene", 1);

        }

        invincibilityTimer = invincibilityTime;
        
    }

    public void restartScene() {
        AudioManager.Instance.RestartGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
