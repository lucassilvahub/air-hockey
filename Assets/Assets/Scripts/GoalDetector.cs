using UnityEngine;

public class GoalDetector : MonoBehaviour
{
  [Header("Goal Area")]
  public bool isPlayerGoal = false;
  public float goalSize = 3f;

  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Ball"))
    {
      float ballY = other.transform.position.y;
      Debug.Log($"[DEBUG] Ball hit {gameObject.name} at Y: {ballY:F2}");

      // Auto-cria GameManager se não existir
      if (GameManager.instance == null)
      {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
        Debug.Log("[FIX] GameManager created automatically!");
      }

      // Só conta gol se estiver no meio da parede
      if (Mathf.Abs(ballY) <= goalSize / 2f)
      {
        Debug.Log($"[GOAL] {(isPlayerGoal ? "PLAYER" : "AI")} GOAL! Ball Y: {ballY:F2}, Goal Size: ±{goalSize / 2f:F2}");

        if (isPlayerGoal)
          GameManager.instance.AddScorePlayer();
        else
          GameManager.instance.AddScoreAI();
      }
      else
      {
        Debug.Log($"[NO GOAL] Ball outside goal area. Y: {ballY:F2}, Required: ±{goalSize / 2f:F2}");
      }
    }
  }
}