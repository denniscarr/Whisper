using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportScore : MonoBehaviour {
    public GameObject gameManager;
    public GameObject healthHolder;

    public float score;
    public int health;

    public bool canFire;
    public bool hasFired;


    public string build;

    public float timer;


	// Use this for initialization
	void Start () {
        Tinylytics.AnalyticsManager.LogCustomMetric("Build", build);

        timer = 0;

        hasFired = false;
        canFire = false;

        gameManager = GameObject.Find("Game Manager");
        healthHolder = GameObject.Find("Health");
        
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime; 
      
        score = gameManager.GetComponent<scoreManager>().score;

        health = healthHolder.GetComponent<HealthManager>().health;

        if (health == 0 && hasFired == false) {
            canFire = true;
        }

        if (canFire == true) {
            sendScore();
        }
	}

    public void sendScore() {
        Debug.Log("Score Recorded");

        //Tinylytics.AnalyticsManager.LogCustomMetric("PlayerScore", score.ToString());

        
  
        Tinylytics.AnalyticsManager.LogCustomMetric("Score", score.ToString());
        Tinylytics.AnalyticsManager.LogCustomMetric("Seconds played", timer.ToString("F0"));

        canFire = false;
        hasFired = true;
    }
}
