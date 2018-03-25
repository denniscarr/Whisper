using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    [SerializeField] GameObject enemyPrefab;
    float spawnEnemyRate = 3f;
    float timer;

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnEnemyRate) {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy() {
        Vector3 newEnemyPosition = ScreenAnalyzer.RandomScreenPoint();
        newEnemyPosition.z = 0f;
        GameObject newEnemy = Instantiate(enemyPrefab, newEnemyPosition, Quaternion.identity);
    }
}
