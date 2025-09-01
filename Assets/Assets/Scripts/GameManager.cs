using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Text scorePlayerText;
    public Text scoreAIText;
    
    private int scorePlayer = 0;
    private int scoreAI = 0;
    
    public static GameManager instance;
    
    void Start()
    {
        instance = this;
        UpdateUI();
    }
    
    public void AddScorePlayer()
    {
        scorePlayer++;
        UpdateUI();
    }
    
    public void AddScoreAI()
    {
        scoreAI++;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        scorePlayerText.text = scorePlayer.ToString();
        scoreAIText.text = scoreAI.ToString();
    }
}