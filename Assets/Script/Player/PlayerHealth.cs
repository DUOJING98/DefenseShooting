using UnityEngine;
using UnityEngine.SceneManagement; // �����л�����

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
        // ֹͣ��Ϸ / ������������ / �����˵�
        // ����ֻ�����س���ʾ��
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
