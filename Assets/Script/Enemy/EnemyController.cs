using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    public GameObject boom;

    private void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform root = collision.transform.root;
        if (root.CompareTag("Player") || collision.tag.Contains("bullet"))
        {
            PlayerHealth player = root.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject); // 敌人撞人后消失
        }
    }


}
