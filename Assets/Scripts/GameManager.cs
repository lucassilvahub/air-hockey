using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    public int maxScore = 5;
    public float resetDelay = 2f;

    private int pScore = 0;
    private int aScore = 0;
    private string goalMessage = "";
    private float messageTime = 0;
    private bool gameEnded = false;
    private bool goalScored = false; // Evita duplo gol

    public static GameManager instance;

    void Awake() => instance = this;

    public void AddScorePlayer()
    {
        if (gameEnded || goalScored) return; // Evita gol duplo

        goalScored = true;
        pScore++;
        ShowGoal("PLAYER GOAL!");
        CheckWin();
    }

    public void AddScoreAI()
    {
        if (gameEnded || goalScored) return; // Evita gol duplo

        goalScored = true;
        aScore++;
        ShowGoal("AI GOAL!");
        CheckWin();
    }

    void ShowGoal(string message)
    {
        goalMessage = message;
        messageTime = Time.unscaledTime + resetDelay;

        // Para a bola mas NÃO congela o jogo
        var ball = FindObjectOfType<ballControl>();
        if (ball != null)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        // Agenda reset da bola
        Invoke(nameof(ResetBall), resetDelay);
    }

    void CheckWin()
    {
        if (pScore >= maxScore || aScore >= maxScore)
        {
            gameEnded = true;
            goalMessage = pScore >= maxScore ? "🎉 PLAYER VENCEU! 🎉" : "🤖 IA VENCEU! 🤖";
            messageTime = float.MaxValue;
            Time.timeScale = 0; // SÓ para quando o jogo acaba
        }
    }

    void ResetBall()
    {
        if (!gameEnded)
        {
            goalScored = false; // Libera para próximo gol
            var ball = FindObjectOfType<ballControl>();
            ball?.LaunchBall();
        }
    }

    public void RestartGame()
    {
        pScore = aScore = 0;
        gameEnded = false;
        goalScored = false;
        goalMessage = "";
        messageTime = 0;
        Time.timeScale = 1;

        CancelInvoke();

        var ball = FindObjectOfType<ballControl>();
        ball?.LaunchBall();

        Debug.Log("🔄 Game Restarted");
    }

    void OnGUI()
    {
        // Placar VERMELHO
        GUI.Label(new Rect(50, 30, 200, 40), $"Player: {pScore}", GetRedStyle(24));
        GUI.Label(new Rect(50, 80, 200, 40), $"AI: {aScore}", GetRedStyle(24));

        // Mensagem (só mostra se tiver mensagem E dentro do tempo OU se game over)
        bool showMessage = !string.IsNullOrEmpty(goalMessage) &&
                          (Time.unscaledTime < messageTime || gameEnded);

        if (showMessage)
        {
            float centerX = Screen.width / 2 - 100;
            float centerY = Screen.height / 2 - 50;

            GUI.Box(new Rect(centerX - 20, centerY - 20, 240, 100), "");
            GUI.Label(new Rect(centerX, centerY, 200, 60), goalMessage, GetCenterStyle());

            // Botão restart só se game over
            if (gameEnded)
            {
                if (GUI.Button(new Rect(centerX + 50, centerY + 50, 100, 30), "RESTART"))
                    RestartGame();
            }
        }
    }

    GUIStyle GetRedStyle(int size) // PLACAR VERMELHO
    {
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = size;
        style.normal.textColor = Color.red;
        return style;
    }

    GUIStyle GetCenterStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }
}