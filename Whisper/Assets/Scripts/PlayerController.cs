using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float speed = 10f;
    [SerializeField] float maxSpeed = 50f;

    [SerializeField] float maxDistanceFromAnchor = 1f;

    [SerializeField] GameObject anchorPointObject;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] GameObject focusPointPrefab;

    Vector3 inputVector;
    Vector3 velocity = Vector3.zero;

    Vector3 anchorPoint = Vector3.zero;

    GameObject lastFocusPoint;

	Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }
    ScrapingScript m_ScrapingScript { get { return GetComponentInChildren<ScrapingScript>(); } }
    ScreenAnalyzer screenAnalyzer;


    private void Start() {
        anchorPointObject.transform.parent = null;
        screenAnalyzer = FindObjectOfType<ScreenAnalyzer>();
    }


    private void Update() {
        // Handle basic input.
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");

        velocity += inputVector * speed * Time.deltaTime;

        // If position was just anchored.
        if (Input.GetKeyDown(KeyCode.Space)) {
            anchorPoint = transform.position;
            anchorPointObject.transform.position = anchorPoint;
            anchorPointObject.SetActive(true);
            lastFocusPoint = Instantiate(focusPointPrefab, anchorPoint, Quaternion.identity);
            lastFocusPoint.GetComponent<FocusPoint>().state = FocusPoint.State.Expanding;
            lineRenderer.enabled = true;
        }

        // If position is currently anchored
        bool isAnchored = Input.GetKey(KeyCode.Space);
        if (isAnchored) {
            //Audio
            GetComponent<PlayerAudioScript>().PlayRubbingSourceSound();

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, anchorPoint);

            Vector3 directionToAnchor = Vector3.Normalize(anchorPoint - transform.position);
            float distanceToAnchor = Vector3.Distance(anchorPoint, transform.position);
            float anchorForce = MyMath.Map(distanceToAnchor, 0f, maxDistanceFromAnchor, maxSpeed, maxSpeed * 2);
            velocity += directionToAnchor * anchorForce * Time.deltaTime;
        }

        // If position is no longer anchored
        if (Input.GetKeyUp(KeyCode.Space)) {
            anchorPointObject.SetActive(false);
            lineRenderer.enabled = false;
            if (lastFocusPoint.GetComponent<FocusPoint>().state != FocusPoint.State.LockedIn) lastFocusPoint.GetComponent<FocusPoint>().state = FocusPoint.State.Normal;

            //Audio
            GetComponent<PlayerAudioScript>().StopRubbingSourceSound();
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Only deccelerate if anchored.
        if (isAnchored) velocity *= 0.95f;

        // Update scraping audio values.
        m_ScrapingScript.speedValue = MyMath.Map(velocity.magnitude, maxSpeed * 0.5f, maxSpeed, 0f, 1f);

        // Test for overlapping sprites.
        if (isAnchored) {
            RaycastHit[] overlappingSprites = Physics.RaycastAll(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(transform.position)), 50f, 1 << 9);
            for (int i = 0; i < overlappingSprites.Length; i++) {
                if (overlappingSprites[i].collider.GetComponent<SpriteMovement>() != null) {
                    overlappingSprites[i].collider.GetComponent<SpriteMovement>().GetRubbed(anchorPoint);
                }
            }
        }
    }


    private void FixedUpdate() {

        // Clamp new position to screen.
        Vector3 nextPosition = transform.position + velocity;
        nextPosition = ScreenAnalyzer.ClampPointToScreen(nextPosition);

        // Move
        m_Rigidbody.MovePosition(nextPosition);

        // Update scraping audio values.
        screenAnalyzer.testPoint = Camera.main.WorldToScreenPoint(transform.position);
        m_ScrapingScript.overlapAmount = GetColorBrightness(screenAnalyzer.lastPixelColor);
    }


    float GetColorBrightness(Color color) {
        return (color.r + color.g + color.b) / 3;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name.Contains("Enemy")) {
            FindObjectOfType<HealthManager>().GetHurt();
            other.GetComponentInChildren<EnemyAudioScript>().HitPlayer();
            StartCoroutine(other.GetComponent<enemyMovementScript>().HitPlayer());
        }
        else if (other.name.Contains("Web")) {
            GetComponent<PlayerAudioScript>().StartStrandSound();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.name.Contains("Web")) {
            velocity *= 0.7f;
            other.GetComponent<WebLine>().RemoveHealthTouch();
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.name.Contains("Web")) {
            GetComponent<PlayerAudioScript>().EndStrandSound();
        }
    } 
}
