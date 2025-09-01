using UnityEngine;

public class GoalDetector : MonoBehaviour
{
  [Header("Goal Settings")]
  public bool isPlayerGoal = false; // false = AI goal, true = Player goal
  public ParticleSystem goalEffect; // Opcional

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Ball") && GameManager.instance != null)
    {
      // Efeito visual opcional
      if (goalEffect != null)
        goalEffect.Play();

      // Adiciona pontuação
      if (isPlayerGoal)
        GameManager.instance.AddScorePlayer();
      else
        GameManager.instance.AddScoreAI();

      // Para a bola temporariamente
      Rigidbody2D ballRb = other.GetComponent<Rigidbody2D>();
      if (ballRb != null)
        ballRb.velocity = Vector2.zero;
    }
  }
}