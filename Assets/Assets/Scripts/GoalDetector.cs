using UnityEngine;

public class GoalDetector : MonoBehaviour
{
  public bool isLeftGoal = true;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Ball") && GameManager.instance != null)
    {
      if (isLeftGoal)
        GameManager.instance.AddScoreAI();
      else
        GameManager.instance.AddScorePlayer();
    }
  }
}