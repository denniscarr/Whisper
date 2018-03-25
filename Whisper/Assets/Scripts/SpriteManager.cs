using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    [SerializeField] GameObject spritePrefab;
    [SerializeField] int numberOfSprites;
    [SerializeField] int startingCongregationPoints = 4;

    [SerializeField] List<Sprite> imageList = new List<Sprite>();

    public static List<GameObject> sprites = new List<GameObject>();
    public static List<Vector3> congregationPoints = new List<Vector3>();


    private void Start() {
        SpawnSprites();

        for (int i = 0; i < startingCongregationPoints; i++) {
            AddCongregationPoint();
        }
    }


    void SpawnSprites() {
        for(int i = 0; i < numberOfSprites; i++) {
            SpawnSprite();
        }
    }


    void AddCongregationPoint() {
        congregationPoints.Add(ScreenAnalyzer.RandomScreenPoint());
    }


    void SpawnSprite() {
        Vector3 newPosition = ScreenAnalyzer.RandomScreenPoint();
        GameObject newSprite = Instantiate(spritePrefab, newPosition, Quaternion.identity);
        newSprite.GetComponent<SpriteRenderer>().sprite = imageList[Random.Range(0, imageList.Count - 1)];
        newSprite.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        sprites.Add(newSprite);
    }
}
