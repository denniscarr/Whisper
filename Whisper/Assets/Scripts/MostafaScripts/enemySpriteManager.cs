using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpriteManager : MonoBehaviour {

    [SerializeField]
    GameObject spritePrefab;
    [SerializeField]
    int numberOfSprites;
    [SerializeField]
    int startingCongregationPoints = 4;

    [SerializeField]
    List<Sprite> imageList = new List<Sprite>();

    Vector3 anchorPoint;
    LineRenderer lineToAnchor;

    public static List<GameObject> sprites = new List<GameObject>();
    public static List<Vector3> congregationPoints = new List<Vector3>();


    private void Start() {
        SpawnSprites();

        for (int i = 0; i < startingCongregationPoints; i++) {
            AddCongregationPoint();
        }

        lineToAnchor = GetComponentInChildren<LineRenderer>();
    }


    void SpawnSprites() {
        for (int i = 0; i < numberOfSprites; i++) {
            SpawnSprite();
        }
    }


    void AddCongregationPoint() {
        congregationPoints.Add(RandomScreenPoint());
    }


    void SpawnSprite() {
        Vector3 newPosition = RandomScreenPoint();
        GameObject newSprite = Instantiate(spritePrefab, newPosition, Quaternion.identity);
        newSprite.GetComponent<SpriteRenderer>().sprite = imageList[Random.Range(0, imageList.Count - 1)];
        newSprite.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        sprites.Add(newSprite);
    }


    Vector3 RandomScreenPoint() {
        Vector3 position = new Vector3(Random.value, Random.value, Random.Range(3f, 20f));
        return Camera.main.ViewportToWorldPoint(position);
    }
}
