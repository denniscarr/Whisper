using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 10f;

    private void FixedUpdate() {
        Vector3 nextPosition = transform.position;
        nextPosition += Random.insideUnitSphere * moveSpeed * Time.deltaTime;
        transform.position = nextPosition;
    }
}
