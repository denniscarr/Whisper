using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;
    Vector3 myCongregationPoint;
    float rubValue;
    float rubDecayRate = 2f;

    float noiseTime = 0f;
    float noiseSpeed = 0.001f;

    Vector3 basePosition;

    private void Start() {
        basePosition = transform.position;
        noiseTime = Random.Range(-1000f, 1000f);
    }

    private void Update() {
        rubValue -= rubDecayRate * Time.deltaTime;
        rubValue = Mathf.Clamp01(rubValue);
        noiseTime += noiseSpeed * Time.deltaTime;
    }

    private void FixedUpdate() {
        if (rubValue > 0) {
            Vector3 direction = Vector3.Normalize(myCongregationPoint - basePosition);
            direction = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f)) * direction;
            Vector3 nextPosition = basePosition + direction * moveSpeed * Time.deltaTime;
            basePosition = nextPosition;
        }

        else {
            basePosition += new Vector3(
                MyMath.Map(Mathf.PerlinNoise(noiseTime, 0f), 0f, 1f, -1f, 1f),
                MyMath.Map(Mathf.PerlinNoise(0f, noiseTime), 0f, 1f, -1f, 1f),
                0f) * moveSpeed * 0.5f * Time.deltaTime;
        }

        // Apply jitter.
        transform.position = basePosition + Random.insideUnitSphere * 0.05f;
    }

    void GetNewCongregationPoint() {
        myCongregationPoint = SpriteManager.congregationPoints[Random.Range(0, SpriteManager.congregationPoints.Count - 1)];
    }

    public void GetRubbed(Vector3 newCongregationPoint) {
        myCongregationPoint = newCongregationPoint;
        rubValue += 0.1f;
    }
}
