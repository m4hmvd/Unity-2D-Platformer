using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int health = 3;
    public int lives = 3;
    public static int score = 0;
    public static bool hasHeart = false;

    // carried from one level to the next so Level 2 continues where Level 1 ended (-1 = start fresh)
    public static int carriedLives = -1;
    public static int carriedHealth = -1;

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    private SpriteRenderer sr;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;

    public TextMeshProUGUI scoreUI;
    public Image healthBar;
    public TextMeshProUGUI livesUI;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (carriedLives >= 0)
        {
            lives = carriedLives;
            health = Mathf.Clamp(carriedHealth, 1, maxHealth);
        }
        else
        {
            health = maxHealth;
        }
        healthBar.fillAmount = (float)health / maxHealth;
    }

    // call before loading the next level
    public void SaveForNextLevel()
    {
        carriedLives = lives;
        carriedHealth = health;
    }

    // call when a new game starts
    public static void ResetCarried()
    {
        carriedLives = -1;
        carriedHealth = -1;
    }

    void Update()
    {
        if (isImmune == true)
        {
            SpriteFlicker();
            immunityTime = immunityTime + Time.deltaTime;

            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled = true;
            }
        }

        scoreUI.text = "x " + score;
        if (livesUI != null) livesUI.text = "x " + lives;
    }

    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime = flickerTime + Time.deltaTime;
        }
        else
        {
            sr.enabled = !sr.enabled;
            flickerTime = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;

        health = health - damage;
        if (health < 0)
            health = 0;

        if (lives > 0 && health == 0)
        {
            FindObjectOfType<LevelManager>().RespawnPlayer();
            health = maxHealth;
            lives--;
        }
        else if (lives == 0 && health == 0)
        {
            Debug.Log("Gameover");
            SceneManager.LoadScene(3);   // GameOver scene (Build Settings number)
            return;
        }

        healthBar.fillAmount = (float)health / maxHealth;

        Debug.Log("Player Health:" + health + "  Lives:" + lives);

        isImmune = true;
        immunityTime = 0f;
    }
    public bool Heal(int amount)
    {
        if (health >= maxHealth) return false;

        health = health + amount;
        if (health > maxHealth)
            health = maxHealth;

        healthBar.fillAmount = (float)health / maxHealth;
        return true;
    }
}
