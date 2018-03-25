using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPoint : MonoBehaviour {

    [SerializeField] float onPointDistance = 0.1f;
    [SerializeField] int requiredSpritesOn = 10;

    float timer;
    float timeToForm = 2f;

    [SerializeField] FocusPointSphere sphere;
    [SerializeField] GameObject webLinePrefab;
    List<GameObject> webToObjects = new List<GameObject>();


    [SerializeField] AudioClip[] playerMakeLinkSound;
    [SerializeField] AudioClip[] breakLinkSound;
 
    float lockInPercent;

    public enum State { Normal, Expanding, LockedIn }
    public State state = State.Expanding;

    private void Start() {
        sphere = GetComponentInChildren<FocusPointSphere>();
    }

    private void Update() {
        switch (state) {
            case State.Normal:
                Normal();
                break;
            case State.Expanding:
                Expanding();
                break;
            case State.LockedIn:
                break;
        }

        sphere.currentIntensity = lockInPercent;
    }

    void Normal() {
        lockInPercent *= 0.99f;

        if (lockInPercent <= 0.01f) {
            DestroySelf();
        }
    }

    void Expanding() {
        //int numberOn = 0;
        //for (int i = 0; i < SpriteManager.sprites.Count; i++) {
        //    float distance = Vector3.Distance(transform.position, SpriteManager.sprites[i].transform.position);
        //    if (distance <= onPointDistance) {
        //        numberOn++;
        //    }
        //}

        timer += Time.deltaTime;

        lockInPercent = MyMath.Map(timer, 0, timeToForm, 0f, 1f);
        lockInPercent = Mathf.Clamp01(lockInPercent);

        if (lockInPercent >= 1f) {
            lockInPercent = 0.5f;

            Color newColor = sphere.gameObject.GetComponent<MeshRenderer>().material.color;
            newColor.r = 0f;
            newColor.g = 0f;
            newColor.b = 0f;
            newColor.a = 1f;
            sphere.GetComponent<MeshRenderer>().material.color = newColor;

            sphere.TurnThisColor(newColor);

            newColor = sphere.gameObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
            newColor.r = 0f;
            newColor.g = 0f;
            newColor.b = 0f;
            sphere.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", newColor);
            sphere.gameObject.layer = LayerMask.NameToLayer("Player");

            FocusPoint[] otherFocusPoints = FindObjectsOfType<FocusPoint>();
            foreach (FocusPoint focusPoint in otherFocusPoints) {
                if (focusPoint == this) { continue; }
                if (focusPoint.state != State.LockedIn) { continue; }
                if (webToObjects.Contains(focusPoint.gameObject)) { continue; }
                GameObject newWebLine = Instantiate(webLinePrefab);
                newWebLine.GetComponent<WebLine>().SetLinePositions(transform.position, focusPoint.transform.position, 0.2f);
                newWebLine.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                newWebLine.GetComponent<LineRenderer>().SetPosition(1, focusPoint.transform.position);
                webToObjects.Add(focusPoint.gameObject);
            }

            state = State.LockedIn;

            //AUDIO
            MakeLinkSound();
            AudioManager.Instance.AddLink();
        }
    }

    public void MakeLinkSound() {

        int soundIndex = Random.Range(0, playerMakeLinkSound.Length - 1);

        GetComponent<AudioSource>().clip = playerMakeLinkSound[soundIndex];
        GetComponent<AudioSource>().Play();
    }



    public void DestroySelf() {
      
        Destroy(gameObject);
    }
}
