using UnityEngine;

public class PlayerSineDrift : MonoBehaviour
{
    [SerializeField] private float amplitudeX = 0.5f; // 左右摆动幅度
    [SerializeField] private float amplitudeY = 0.3f; // 上下漂动幅度
    [SerializeField] private float frequencyX = 1f;   // 左右摆动频率
    [SerializeField] private float frequencyY = 1.5f; // 上下漂动频率

    private Vector3 startPos;
    private float timeCounter;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        timeCounter += Time.deltaTime;

        float offsetX = Mathf.Cos(timeCounter * frequencyX) * amplitudeX;
        float offsetY = Mathf.Sin(timeCounter * frequencyY) * amplitudeY;

        transform.position = startPos + new Vector3(offsetX, offsetY, 0f);
    }
}
