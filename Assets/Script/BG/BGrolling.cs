using UnityEngine;

public class BGrolling : MonoBehaviour
{
    public Vector2 scollVelocity;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        material.mainTextureOffset += scollVelocity * Time.deltaTime;
    }
}
