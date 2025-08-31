using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Placar")]
    public Text scorePlayerText;
    public Text scoreAIText;
    public Text gameOverText;
    public Button resetButton;
    
    [Header("Configurações do Jogo")]
    public int maxScore = 5;
    
    private int scorePlayer = 0;
    private int scoreAI = 0;
    private bool gameOver = false;
    
    public static GameManager instance;
    
    // Referências para outros componentes
    private ballControl ballController;
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ballController = FindObjectOfType<ballControl>();
        
        // Inicializar UI
        UpdateScoreUI();
        gameOverText.gameObject.SetActive(false);
        
        // Configurar botão de reset
        resetButton.onClick.AddListener(ResetGame);
    }
    
    public void AddScorePlayer()
    {
        if (gameOver) return;
        
        scorePlayer++;
        UpdateScoreUI();
        CheckGameOver();
        
        if (!gameOver)
        {
            ballController.RestartGame();
        }
    }
    
    public void AddScoreAI()
    {
        if (gameOver) return;
        
        scoreAI++;
        UpdateScoreUI();
        CheckGameOver();
        
        if (!gameOver)
        {
            ballController.RestartGame();
        }
    }
    
    void UpdateScoreUI()
    {
        scorePlayerText.text = scorePlayer.ToString();
        scoreAIText.text = scoreAI.ToString();
    }
    
    void CheckGameOver()
    {
        if (scorePlayer >= maxScore)
        {
            GameOver("JOGADOR VENCEU!");
        }
        else if (scoreAI >= maxScore)
        {
            GameOver("IA VENCEU!");
        }
    }
    
    void GameOver(string winner)
    {
        gameOver = true;
        gameOverText.text = winner;
        gameOverText.gameObject.SetActive(true);
        
        // Parar a bola
        if (ballController != null)
        {
            ballController.StopBall();
        }
    }
    
    public void ResetGame()
    {
        scorePlayer = 0;
        scoreAI = 0;
        gameOver = false;
        
        UpdateScoreUI();
        gameOverText.gameObject.SetActive(false);
        
        if (ballController != null)
        {
            ballController.RestartGame();
        }
    }
    
    public bool IsGameOver()
    {
        return gameOver;
    }
}