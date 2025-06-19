using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionPrefab;

    public void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
