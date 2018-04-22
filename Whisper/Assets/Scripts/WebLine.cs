using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLine : MonoBehaviour {

    float health = 1f;
    float healthRemovedPerFrame = 0.05f;
    float healthRemovedPerFrameTouch = 0.4f;
    float startingWidth;
    float minWidth = 0.01f;
    float currentWidth;
    Color originalColor;

    public AudioClip[] breakLinkSound;

    public List<GameObject> trappedEnemies = new List<GameObject>();

    private void Awake() {
        originalColor = GetComponent<LineRenderer>().material.color;
    }

    private void Start() {
        startingWidth = GetComponent<LineRenderer>().widthMultiplier;
        currentWidth = startingWidth;
    }

    private void Update() {
        RemoveHealthNotTouch();
        float tempWidth = currentWidth + Random.Range(-0.1f, 0.1f);
        GetComponent<LineRenderer>().widthMultiplier = tempWidth;
        GetComponent<LineRenderer>().material.SetFloat("_Cutoff", Random.Range(0.16f, 0.86f));
        GetComponent<LineRenderer>().material.mainTextureOffset = new Vector2(Random.Range(-1, 1), 1);
        GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(Random.Range(-10f, 10f), 1);
    }

    int numberOfPositions = 10;
    public void SetLinePositions(Vector3 beginning, Vector3 end, float thickness) {
        GetComponent<LineRenderer>().positionCount = numberOfPositions;
        GetComponent<LineRenderer>().SetPosition(0, beginning);
        for (int i = 1; i < numberOfPositions-1; i++) {
            Vector3 directionToEndPoint = Vector3.Normalize(end - beginning);
            Vector3 newPos = beginning + directionToEndPoint * (Vector3.Distance(beginning, end) / numberOfPositions) * i;
            GetComponent<LineRenderer>().SetPosition(i, newPos);
        }
        GetComponent<LineRenderer>().SetPosition(numberOfPositions-1, end);

        SetTransformByEndPoints(beginning, end, thickness);
    }

    public void SetTransformByEndPoints(Vector3 back, Vector3 front, float thickness) {

        transform.position = Vector3.Lerp(back, front, 0.5f);

        Vector3 lookDirection = (front - back);
        if (lookDirection != Vector3.zero) { transform.rotation = Quaternion.LookRotation(front - back, Vector3.up); }
        transform.Rotate(90f, 0f, 0f);

        transform.localScale = new Vector3(
            thickness,
            Vector3.Distance(back, front) * 0.5f,
            thickness
            );
    }

    void RemoveHealthNotTouch() {
        health -= healthRemovedPerFrame * Time.deltaTime;
        TestForDeath();
    }

    public void RemoveHealthTouch() {
        health -= healthRemovedPerFrameTouch * Time.deltaTime;
        TestForDeath();
    }

    void TestForDeath() {
        currentWidth = MyMath.Map(health, 0f, 1f, minWidth, startingWidth);
        if (currentWidth <= 0.1f) {
            GetComponent<LineRenderer>().material.color = Color.gray;
            GetComponent<LineRenderer>().material.SetColor("_EmissionColor", Color.gray);
        }
        else {
            GetComponent<LineRenderer>().material.color = originalColor;
            GetComponent<LineRenderer>().material.SetColor("_EmissionColor", originalColor);
        }

        if (health <= 0) {
            PlayBreakSound();
            FindObjectOfType<PlayerAudioScript>().EndStrandSound();
            Destroy(gameObject);
            foreach (GameObject enemy in trappedEnemies) {
                enemy.GetComponent<enemyMovementScript>().BecomeFree();
            }
        }
    }

    public void PlayBreakSound () {
        int soundIndex = Random.Range(0, breakLinkSound.Length - 1);

        AudioClip clipToPlay = breakLinkSound[soundIndex];
        //Debug.Log("playing break link sound " + clipToPlay.name);
        AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
        //GetComponent<AudioSource>().PlayOneShot(clipToPlay);

    }
}
