using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlur : MonoBehaviour {

    Material m_Material;

    float idleValue = 0.5f;
    float maxValue = -0.25f;
    float currentValue;

    float noiseTime = 0f;
    [SerializeField] float noiseSpeed = 1f;
    float noiseScale = 0.1f;

    [SerializeField] float blinkSpeed = 1f;

    Coroutine blinkCoroutine;

    private void Awake() {
        m_Material = GetComponent<MeshRenderer>().material;
        currentValue = idleValue;
    }

    private void Update() {
        noiseTime += noiseSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.B)) { Blink(); }

        float noisedValue = currentValue + MyMath.Map(Mathf.PerlinNoise(noiseTime, 0), 0f, 1f, -0.4f, 0.4f) * noiseScale;
        m_Material.SetFloat("_Cutoff", noisedValue);

        m_Material.mainTextureOffset = new Vector2(Random.Range(-10f, 10f), m_Material.mainTextureOffset.y);
    }

    public void Blink() {
        Debug.Log("blilnk");
        if (blinkCoroutine != null) { StopCoroutine(blinkCoroutine); }
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine() {

        noiseScale = 1f;

        // Close eye.
        yield return new WaitUntil(() => {
            if (currentValue <= maxValue + 0.1f) {
                return true;
            }

            else {
                currentValue = Mathf.Lerp(currentValue, maxValue, blinkSpeed);
                return false;
            }
        });

        // Open eye.
        yield return new WaitUntil(() => {
            if (currentValue >= idleValue - 0.1f) {
                return true;
            } else {
                currentValue = Mathf.Lerp(currentValue, idleValue, blinkSpeed);
                return false;
            }
        });

        noiseScale = 0.1f;

        yield return null;
    }
}
