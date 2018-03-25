using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPointSphere : MonoBehaviour {

    float startingScale;
    float baseScale;
    float maxScale = 2f;

    float startingAlpha;
    float baseAlpha;
    float maxAlpha = 0.7f;

    [HideInInspector] public float currentIntensity = 0f;

    float alpha {
        get { return GetComponent<MeshRenderer>().material.color.a; }
        set {
            Color newColor = GetComponent<MeshRenderer>().material.color;
            newColor.a = value;
            GetComponent<MeshRenderer>().material.color = newColor;
        }
    }

    private void Start() {
        baseScale = transform.localScale.x;
        startingScale = baseScale;
        baseAlpha = alpha;
        startingAlpha = baseAlpha;
    }

    private void Update() {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)) * transform.rotation;

        baseScale = MyMath.Map(currentIntensity, 0f, 1f, startingScale, maxScale);
        float currentScale = baseScale * Random.Range(0.9f, 1.1f);
        transform.localScale = Vector3.one * currentScale;

        float newAlpha = MyMath.Map(baseScale, startingScale, maxScale, startingAlpha, maxAlpha);
        alpha = newAlpha;
        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>()) {
            Color newColor = mr.material.color;
            newColor.a = newAlpha;
            mr.material.color = newColor;
        }
    }

    public void TurnThisColor(Color color) {
        foreach (SkinnedMeshRenderer mr in GetComponentsInChildren<SkinnedMeshRenderer>()) {
            mr.material.color = color;
            mr.material.SetColor("_EmissionColor", color);
            mr.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        GetComponent<MeshRenderer>().material.color = color; 
    }
}
