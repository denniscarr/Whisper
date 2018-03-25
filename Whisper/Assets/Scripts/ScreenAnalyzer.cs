using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnalyzer : MonoBehaviour {

    Texture2D screen;
    bool testPixelTrigger = false;

    [SerializeField] float frequency = 0.5f;
    float timer = 0f;

    [HideInInspector] public Vector3 testPoint = Vector3.zero;
    [HideInInspector] public Color lastPixelColor;


    private void Start() {
        screen = new Texture2D(1, 1);
    }


    private void Update() {
        timer += Time.deltaTime;
        if (timer > frequency) {
            testPixelTrigger = true;
            timer = 0f;
        }
    }


    private void OnPostRender() {
        if (testPixelTrigger) {
            lastPixelColor = TestPixel();
            testPixelTrigger = false;
        }
    }

    Color TestPixel() {
        screen.ReadPixels(new Rect(testPoint.x, testPoint.y, 1, 1), 0, 0);
        return screen.GetPixel(0, 0);
    }

    public static Vector3 ClampPointToScreen(Vector3 point) {
        Vector3 returnPoint = Camera.main.WorldToViewportPoint(point);
        returnPoint.x = Mathf.Clamp(returnPoint.x, 0.01f, 0.99f);
        returnPoint.y = Mathf.Clamp(returnPoint.y, 0.01f, 0.99f);
        return Camera.main.ViewportToWorldPoint(returnPoint);
    }

    public static Vector3 RandomScreenPoint() {
        Vector3 position = new Vector3(Random.value, Random.value, Random.Range(3f, 20f));
        return Camera.main.ViewportToWorldPoint(position);
    }
}
