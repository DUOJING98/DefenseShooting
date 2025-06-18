using UnityEngine;
using UnityEngine.SceneManagement; // 用于切换场景

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over!");
        // 停止游戏 / 弹出结束画面 / 回主菜单
        // 这里只是重载场景示例
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
