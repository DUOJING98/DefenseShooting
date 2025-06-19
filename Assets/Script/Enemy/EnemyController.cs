using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    private void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform root = collision.transform.root;
        if (root.CompareTag("Player"))
        {
            PlayerHealth player = root.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject); // 敌人撞人后消失
        }
    }


}
