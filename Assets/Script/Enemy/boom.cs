using UnityEngine;

public class boom : MonoBehaviour
{
    float timeCnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeCnt += Time.unscaledDeltaTime;
        if (timeCnt >= 0.25f)
        {
            Destroy(gameObject);
        }
    }
}
