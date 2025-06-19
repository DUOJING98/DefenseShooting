using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed; // ? ������
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // 每颗子弹扣 1
            }

            Destroy(gameObject);
        }
    }

}
