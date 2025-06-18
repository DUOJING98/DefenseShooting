using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;

    [SerializeField] float startSpawnInterval = 2f; // 初始间隔
    [SerializeField] float minSpawnInterval = 0.2f; // 最快间隔
    [SerializeField] float intervalDecreaseRate = 0.1f; // 每秒减少多少
    private float spawnInterval;
    private float elapsedTime = 0f;

    [SerializeField] float spawnDistance = 12f;

    private Camera cam;
    private float timer = 0f;


    private void Start()
    {
        cam = Camera.main;
        spawnInterval = startSpawnInterval;

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        // 每秒减少一次生成间隔，但不会低于最小值
        spawnInterval = Mathf.Max(minSpawnInterval, startSpawnInterval - intervalDecreaseRate * elapsedTime);

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {
        if (enemyPrefab.Length == 0) return;

        int enemyCount = Random.Range(1, 5); // 每次生成 1~4 个敌人

        for (int i = 0; i < enemyCount; i++)
        {
            int index = Random.Range(0, enemyPrefab.Length);
            GameObject selectedEnemy = enemyPrefab[index];

            float camH = 2f * cam.orthographicSize;
            float camW = camH * cam.aspect;

            float yMin = cam.transform.position.y - camH / 2;
            float yMax = cam.transform.position.y + camH / 2;
            float y = Random.Range(yMin, yMax);

            float x = cam.transform.position.x + camW / 2 + spawnDistance;

            Vector3 spawnPos = new Vector3(x, y, 0f);
            Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
        }
    }

}
