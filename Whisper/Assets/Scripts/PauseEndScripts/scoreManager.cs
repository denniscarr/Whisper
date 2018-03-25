using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {
    public float score;
    public Text scoresText;
    public Text finalScoresText;


    // Use this for initialization
    void Start () {
        if(scoresText == null) {
            scoresText = GameObject.Find("score").GetComponent<Text>();
        }

        if (finalScoresText == null) {
            finalScoresText = GameObject.Find("finalScore").GetComponent<Text>();
        }


        score = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

        scoresText.text = score.ToString();
        finalScoresText.text = score.ToString();

    }
}
