using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballControl : MonoBehaviour
{
  [Header("Configurações da Bola")]
  public float initialForce = 10f;
  public float maxSpeed = 25f;
  public float minSpeed = 10f;
  public float speedIncreasePerHit = 1.2f;

  [Header("Efeitos de Colisão")]
  public float paddleInfluence = 0.3f; // Influência da velocidade da raquete na bola
  public float maxBounceAngle = 45f; // Ângulo máximo de rebote em graus

  [Header("Configurações dos Gols")]
  public float goalMinY = -1.5f; // Limite inferior da área de gol
  public float goalMaxY = 1.5f;  // Limite superior da área de gol
  public float leftGoalX = -9f;  // Posição X da parede esquerda
  public float rightGoalX = 9f;  // Posição X da parede direita

  private Rigidbody2D rb2d;
  private Vector2 initialPosition;
  private bool ballInPlay = false;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    initialPosition = transform.position;
    Invoke("GoBall", 2);
  }

  // Inicializa a bola randomicamente para esquerda ou direita
  void GoBall()
  {
    ballInPlay = true;
    float rand = Random.Range(0, 2);
    Vector2 force;

    if (rand < 1)
    {
      force = new Vector2(initialForce, Random.Range(-initialForce * 0.5f, initialForce * 0.5f));
    }
    else
    {
      force = new Vector2(-initialForce, Random.Range(-initialForce * 0.5f, initialForce * 0.5f));
    }

    rb2d.AddForce(force);
  }

  // Sistema de colisão melhorado com detecção de gol
  void OnCollisionEnter2D(Collision2D coll)
  {
    if (!ballInPlay) return;

    if (coll.collider.CompareTag("Player"))
    {
      HandlePaddleCollision(coll);
    }
    else if (coll.collider.CompareTag("Wall"))
    {
      HandleWallCollision(coll);
    }
  }

  void HandleWallCollision(Collision2D coll)
  {
    Vector2 hitPoint = coll.contacts[0].point;

    // Verificar se a colisão foi nas paredes laterais (gol)
    if (IsLeftWallHit(hitPoint))
    {
      if (IsInGoalArea(hitPoint.y))
      {
        // GOL! IA marcou
        ScoreGoal("AI");
        return;
      }
    }
    else if (IsRightWallHit(hitPoint))
    {
      if (IsInGoalArea(hitPoint.y))
      {
        // GOL! Jogador marcou
        ScoreGoal("Player");
        return;
      }
    }

    // Se chegou aqui, foi colisão normal com parede (não gol)
    HandleNormalWallBounce();
  }

  bool IsLeftWallHit(Vector2 hitPoint)
  {
    return hitPoint.x <= leftGoalX;
  }

  bool IsRightWallHit(Vector2 hitPoint)
  {
    return hitPoint.x >= rightGoalX;
  }

  bool IsInGoalArea(float yPosition)
  {
    return yPosition >= goalMinY && yPosition <= goalMaxY;
  }

  void ScoreGoal(string scorer)
  {
    ballInPlay = false;

    if (GameManager.instance != null)
    {
      if (scorer == "Player")
      {
        GameManager.instance.AddScorePlayer();
        Debug.Log("GOL DO JOGADOR!");
      }
      else if (scorer == "AI")
      {
        GameManager.instance.AddScoreAI();
        Debug.Log("GOL DA IA!");
      }
    }
  }

  void HandleNormalWallBounce()
  {
    // Manter velocidade consistente após rebote normal na parede
    Vector2 vel = rb2d.velocity;
    vel = vel.normalized * Mathf.Clamp(vel.magnitude, minSpeed, maxSpeed);
    rb2d.velocity = vel;
  }

  void HandlePaddleCollision(Collision2D coll)
  {
    // Calcular ponto de impacto relativo à raquete
    Vector2 hitPoint = coll.contacts[0].point;
    Vector2 paddleCenter = coll.collider.transform.position;
    float paddleHeight = coll.collider.bounds.size.y;

    // Normalizar posição de impacto (-1 a 1, onde 0 é o centro)
    float relativeIntersectY = (hitPoint.y - paddleCenter.y) / (paddleHeight / 2);
    relativeIntersectY = Mathf.Clamp(relativeIntersectY, -1f, 1f);

    // Calcular ângulo de rebote
    float bounceAngle = relativeIntersectY * maxBounceAngle * Mathf.Deg2Rad;

    // Determinar direção (sempre oposta à posição da raquete)
    float direction = coll.collider.transform.position.x > 0 ? -1 : 1;

    // Calcular nova velocidade
    float currentSpeed = rb2d.velocity.magnitude;
    float newSpeed = Mathf.Clamp(currentSpeed + speedIncreasePerHit, minSpeed, maxSpeed);

    Vector2 newVelocity = new Vector2(
        direction * newSpeed * Mathf.Cos(bounceAngle),
        newSpeed * Mathf.Sin(bounceAngle)
    );

    // Adicionar influência da velocidade da raquete
    Rigidbody2D paddleRb = coll.collider.attachedRigidbody;
    if (paddleRb != null)
    {
      newVelocity.y += paddleRb.velocity.y * paddleInfluence;
    }

    // Garantir velocidade mínima horizontal
    if (Mathf.Abs(newVelocity.x) < minSpeed * 0.7f)
    {
      newVelocity.x = direction * minSpeed * 0.7f;
    }

    rb2d.velocity = newVelocity;
  }

  void Update()
  {
    // Manter velocidade dentro dos limites
    if (ballInPlay)
    {
      Vector2 vel = rb2d.velocity;
      float speed = vel.magnitude;

      if (speed < minSpeed)
      {
        rb2d.velocity = vel.normalized * minSpeed;
      }
      else if (speed > maxSpeed)
      {
        rb2d.velocity = vel.normalized * maxSpeed;
      }
    }

    // Verificação extra: se a bola sair muito das bordas (fallback)
    CheckBoundaries();
  }

  void CheckBoundaries()
  {
    Vector2 pos = transform.position;

    // Se a bola sair muito das bordas laterais (fallback de segurança)
    if (pos.x < leftGoalX - 2f)
    {
      ScoreGoal("AI");
    }
    else if (pos.x > rightGoalX + 2f)
    {
      ScoreGoal("Player");
    }
  }

  // Reinicializar a posição e velocidade da bola
  public void ResetBall()
  {
    rb2d.velocity = Vector2.zero;
    transform.position = initialPosition;
    ballInPlay = false;
  }

  // Reinicializar o jogo
  public void RestartGame()
  {
    ResetBall();
    Invoke("GoBall", 1);
  }

  // Parar a bola (para game over)
  public void StopBall()
  {
    rb2d.velocity = Vector2.zero;
    ballInPlay = false;
  }

  // Método para ajustar área dos gols dinamicamente (útil para testes)
  public void SetGoalArea(float minY, float maxY)
  {
    goalMinY = minY;
    goalMaxY = maxY;
  }

  // Método para ajustar posições das paredes dinamicamente
  public void SetWallPositions(float leftX, float rightX)
  {
    leftGoalX = leftX;
    rightGoalX = rightX;
  }
}