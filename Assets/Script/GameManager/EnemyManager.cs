using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float spawnInterval = 2f;

    [Header("Spawn Area (Use World Space)")]
    [SerializeField] Transform spawnAreaTopLeft;
    [SerializeField] Transform spawnAreaBottomRight;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {
        if (enemyPrefab.Length == 0 || !spawnAreaTopLeft || !spawnAreaBottomRight) return;

        int index = Random.Range(0, enemyPrefab.Length);
        GameObject selectedEnemy = enemyPrefab[index];

        float xMin = spawnAreaTopLeft.position.x;
        float xMax = spawnAreaBottomRight.position.x;
        float yMax = spawnAreaTopLeft.position.y;
        float yMin = spawnAreaBottomRight.position.y;

        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        Vector3 spawnPos = new Vector3(x, y, 0f);
        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }
}
