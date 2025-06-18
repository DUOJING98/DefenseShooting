using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    private Animator animator;
    private bool isDying = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //Explode();
        }
    }

    void Explode()
    {
        //isDying = true;

        //if (animator != null)
        //{
        //    animator.SetTrigger("Explode");
        //}

        Destroy(gameObject, 0.5f); // 动画播放完再销毁
    }
}
