using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;

    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float spawnDistance = 12f;

    private Camera cam;
    private float timer = 0f;


    private void Start()
    {
        cam = Camera.main;
    }

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
        if (enemyPrefab.Length == 0) return;
        int index = Random.Range(0, enemyPrefab.Length);
        GameObject selectedEnemy = enemyPrefab[index];

        float camH = 2f * cam.orthographicSize;
        float camW = camH * cam.aspect;

        float yMin = cam.transform.position.y - camH / 2;
        float yMax = cam.transform.position.y + camH / 2;
        float y =Random.Range(yMin, yMax);

        float x = cam.transform.position.x + camW / 2+spawnDistance;

        Vector3 spawnPos = new Vector3 (x, y, 0f);
        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }
}
