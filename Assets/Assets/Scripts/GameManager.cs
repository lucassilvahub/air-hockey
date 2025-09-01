using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int maxScore = 5;
    public float resetDelay = 1.5f;
    
    private int pScore = 0;
    private int aScore = 0;
    private bool gameEnded = false;
    
    public static GameManager instance;
    
    void Awake() => instance = this;
    
    public void AddScorePlayer()
    {
        if (gameEnded) return;
        
        pScore++;
        Debug.Log($"GOAL! Player: {pScore} | AI: {aScore}");
        
        if (pScore >= maxScore)
            EndGame("🎉 PLAYER WINS! 🎉");
        else
            Invoke(nameof(ResetBall), resetDelay);
    }
    
    public void AddScoreAI()
    {
        if (gameEnded) return;
        
        aScore++;
        Debug.Log($"GOAL! Player: {pScore} | AI: {aScore}");
        
        if (aScore >= maxScore)
            EndGame("🤖 AI WINS! 🤖");
        else
            Invoke(nameof(ResetBall), resetDelay);
    }
    
    void EndGame(string winner)
    {
        gameEnded = true;
        Debug.Log(winner);
        Time.timeScale = 0;
    }
    
    void ResetBall()
    {
        FindObjectOfType<ballControl>()?.LaunchBall();
    }
    
    public void RestartGame()
    {
        pScore = aScore = 0;
        gameEnded = false;
        Time.timeScale = 1;
        ResetBall();
        Debug.Log("🔄 Game Restarted");
    }
    
    void OnGUI()
    {
        // Score display
        GUI.Label(new Rect(50, 30, 200, 40), $"Player: {pScore}", GetStyle(20));
        GUI.Label(new Rect(50, 80, 200, 40), $"AI: {aScore}", GetStyle(20));
        
        // Game over screen
        if (gameEnded)
        {
            GUI.Box(new Rect(Screen.width/2 - 150, Screen.height/2 - 100, 300, 200), "");
            GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 50), 
                     pScore > aScore ? "PLAYER WINS!" : "AI WINS!", GetStyle(24));
            
            if (GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 + 20, 100, 30), "Restart"))
                RestartGame();
        }
    }
    
    GUIStyle GetStyle(int fontSize)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        style.normal.textColor = Color.white;
        return style;
    }
}