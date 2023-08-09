using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;
    public int lives;
    //UI
    public RectTransform livesContainer; // Parent Transform containing the life images
    public GameObject lifePrefab; // Prefab for the life image
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public PauseMenu pauseMenu;
    
    //LSL
    public FlashController flashController;

    private bool isPaused = false;

    public static GameManager gameManagerInstance;

    void Awake()
    {
        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);

            pauseMenu = FindObjectOfType<PauseMenu>();
            if (pauseMenu == null)
            {
                Debug.LogError("PauseMenu not found!");
            }

            UpdateLives();
        }
    }

    void Update()
    {
        //Flash
        if (Input.GetKeyDown(KeyCode.R))
        {
            flashController.StartRandomizedFlash();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            flashController.StartEEGFlash();
        }

        //Pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: \n " + score;
        Debug.Log("Score: " + score);
    }

    public void AddLife(int heartValue)
    {
        lives += heartValue;
        OnLivesChanged();
        Debug.Log("Lives: " + lives);
    }

    public void LoseLife(int lifeLoss)
    {
        lives -= lifeLoss;
        OnLivesChanged();
        Debug.Log("Lives: " + lives);
        CheckGameOver();
    }

    private void UpdateLives()
    {
        // Remove existing life objects
        foreach (Transform child in livesContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new life objects
        for (int i = 0; i < lives; i++)
        {
            Instantiate(lifePrefab, livesContainer);
        }
    }

    public void OnLivesChanged()
    {
        UpdateLives();
    }

    public void CheckGameOver()
    {
        if (lives <= 0)
        {
            Debug.Log("Game Over");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    public void PauseGame()
    {
        if (PauseMenu.Instance == null)
        {
            Debug.LogError("PauseMenu not found!");
            return;
        }

        isPaused = true;
        PauseMenu.Instance.ShowMenu(); // Access PauseMenu through the Instance
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (PauseMenu.Instance == null)
        {
            Debug.LogError("PauseMenu not found!");
            return;
        }

        isPaused = false;
        PauseMenu.Instance.HideMenu(); // Access PauseMenu through the Instance
        Time.timeScale = 1;
    }
}
