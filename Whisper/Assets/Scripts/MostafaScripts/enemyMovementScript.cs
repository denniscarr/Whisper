using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyMovementScript : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    float minDistance;

    [SerializeField] float enemySpeed = 1f;
    float originalEnemySpeed;

    private float distance;

    private bool stoppedChasing;

    private Vector3 stopPos;

    private float randomMod;

    Vector3 anchorPoint = Vector3.zero;
    [SerializeField] GameObject anchorPointObject;
    LineRenderer lineRenderer;

    enum State { Free, Trapped }
    State state;

    float noiseTime = 0f;
    float noiseScale = 0f;

    float timerFloat = 0f;


    public GameObject gameManager;
    public GameObject health;
    public float score;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("Game Manager");
        health = GameObject.Find("Health");

        originalEnemySpeed = enemySpeed;

        score = gameManager.GetComponent<scoreManager>().score;

        lineRenderer = GetComponentInChildren<LineRenderer>();

        stopPos = this.transform.position;

        player = GameObject.Find("Player");

        if (minDistance == null) {
            minDistance = 2;
        }

        if (randomMod == 0) {
            randomMod = 1;
        }

        stoppedChasing = true;
    }

    private void FixedUpdate() {
        
        
    }

    void HealthyMovement() {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < minDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            stoppedChasing = false;

        } else {
            // transform.position = this.transform.position;

            if (stoppedChasing == false) {
                stopPos = this.transform.position;
                stoppedChasing = true;
            }

            transform.position = this.transform.position;
            //Invoke("perlinMovement", 1);
            perlinMovement();
        }

        Vector3 clampedPos = ScreenAnalyzer.ClampPointToScreen(transform.position);
        transform.position = clampedPos;
    }

    void VulnerableMovement() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, anchorPoint);
        perlinMovement();
    }

    // Update is called once per frame
    void Update () {

        switch(state) {
            case State.Free:
                noiseTime += Time.deltaTime;
                HealthyMovement();
                break;
            case State.Trapped:
                noiseTime += 2f * Time.deltaTime;
                VulnerableMovement();
                
                timerFloat = timerFloat + Time.deltaTime;
                if (timerFloat > 1) {
                    if (health.GetComponent<HealthManager>().isAlive == true) {
                        gameManager.GetComponent<scoreManager>().score++;
                        timerFloat = 0;
                    }
                }
                break;
        }

        noiseScale = Mathf.Lerp(noiseScale, 1, 0.1f);
	}

    void perlinMovement() {
        //.position = stopPos + new Vector3(Mathf.PerlinNoise(Time.timeSinceLevelLoad, 0), Mathf.PerlinNoise(0, Time.timeSinceLevelLoad), 0

        float x = Mathf.PerlinNoise(noiseTime, 0);
        x *= 2;
        x -= 1;
        x *= randomMod * noiseScale;

        float y = Mathf.PerlinNoise(0, noiseTime);
        y *= 2;
        y -= 1;
        y *= randomMod * noiseScale;

        Vector3 newPos = (stopPos + new Vector3(
            x, y, 0)
            );

        transform.position = newPos;

    }

    private void BecomeTrapped(GameObject web) {
        state = State.Trapped;

        // Get position of anchor point
        Vector3 directionToWeb = Vector3.zero;
        Vector3 enemyPosInWebSpace = web.transform.InverseTransformPoint(transform.position);
        enemyPosInWebSpace.z = 0;
        anchorPoint = web.transform.TransformPoint(enemyPosInWebSpace);

        anchorPointObject.SetActive(true);
        anchorPointObject.transform.position = anchorPoint;
        anchorPointObject.transform.parent = null;

        //GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.3f, 0.35f);
        GetComponentInChildren<TrailRenderer>().material.SetColor("_EmissionColor", Color.white);
        ParticleSystem.MainModule particleMain = GetComponentInChildren<ParticleSystem>().main;
        particleMain.startColor = new Color(0.9f, 0.9f, 0.9f);

        lineRenderer.enabled = true;
        stopPos = anchorPoint;
        randomMod = 1f;
    }

    public void BecomeFree() {
        state = State.Free;
        anchorPointObject.SetActive(false);
        lineRenderer.enabled = false;
        GetComponentInChildren<TrailRenderer>().material.SetColor("_EmissionColor", Color.black);
        ParticleSystem.MainModule particleMain = GetComponentInChildren<ParticleSystem>().main;
        particleMain.startColor = Color.black;
        randomMod = 2f;
        noiseScale = 0f;
    }

    public IEnumerator HitPlayer() {
        enemySpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        enemySpeed = originalEnemySpeed;
        yield return null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name.Contains("Web Line") && state == State.Free) {
            BecomeTrapped(other.gameObject);
            other.GetComponent<WebLine>().trappedEnemies.Add(gameObject);
            GetComponentInChildren<EnemyAudioScript>().OnTrappedInWeb();
        }
    }
}
