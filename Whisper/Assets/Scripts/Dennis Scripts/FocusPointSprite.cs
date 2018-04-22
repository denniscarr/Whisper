using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPointSprite : MonoBehaviour {

    float startingScale;
    float baseScale;
    float maxScale = 2f;

    float startingAlpha;
    float baseAlpha;
    float maxAlpha = 0.8f;

    [HideInInspector] public float currentIntensity = 0f;

    float alpha {
        get { return GetComponent<SpriteRenderer>().color.a; }
        set {
            Color newColor = GetComponent<SpriteRenderer>().color;
            newColor.a = value;
            GetComponent<SpriteRenderer>().color = newColor;
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
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
            Color newColor = sr.color;
            newColor.a = newAlpha;
            sr.color = newColor;
        }
    }

    public void TurnThisColor(Color color) {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
            sr.color = color;
            //sr.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        GetComponent<SpriteRenderer>().color = color;
    }
}
