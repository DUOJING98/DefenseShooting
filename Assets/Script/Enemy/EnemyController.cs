using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    private void Update()
    {
        transform.Translate(Vector2.left*moveSpeed*Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        //Destroy(gameObject);
    }
}
