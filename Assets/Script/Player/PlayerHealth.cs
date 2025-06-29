﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private Image hpBarFill; // ⬅ 血条 UI（填充图）

    public GameObject GameOverUI;
    private void Start()
    {
        Time.timeScale = 1;
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("GameOver");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Update()
    {
        if (currentHealth<=0)
        {
            Time.timeScale = 0;
            GameOverUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
