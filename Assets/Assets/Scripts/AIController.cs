using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Configurações da IA")]
    public float speed = 8.0f;
    public float boundY = 2.25f;
    public float reactionDistance = 3.0f; // Distância para começar a reagir à bola
    public float difficulty = 0.8f; // 0.0 = muito fácil, 1.0 = muito difícil
    public float predictionTime = 0.5f; // Tempo de predição da trajetória da bola
    
    [Header("Configurações Avançadas")]
    public float errorMargin = 0.2f; // Margem de erro para tornar a IA menos perfeita
    public float maxSpeed = 10.0f;
    public bool enablePrediction = true;
    
    private Rigidbody2D rb2d;
    private Transform ball;
    private Rigidbody2D ballRb;
    private Vector2 targetPosition;
    private float lastReactionTime;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
        // Encontrar a bola
        GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
        if (ballObject != null)
        {
            ball = ballObject.transform;
            ballRb = ballObject.GetComponent<Rigidbody2D>();
        }
        
        targetPosition = transform.position;
    }
    
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameOver())
        {
            // Parar movimento quando o jogo acabar
            rb2d.velocity = Vector2.zero;
            return;
        }
        
        if (ball != null)
        {
            MoveAI();
        }
    }
    
    void MoveAI()
    {
        Vector2 ballPosition = ball.position;
        Vector2 paddlePosition = transform.position;
        
        // Verificar se a bola está se movendo em direção à IA
        bool ballComingTowards = (ballRb.velocity.x > 0 && transform.position.x > 0) || 
                                (ballRb.velocity.x < 0 && transform.position.x < 0);
        
        // Calcular distância da bola
        float distanceToBall = Vector2.Distance(ballPosition, paddlePosition);
        
        if (ballComingTowards && distanceToBall <= reactionDistance)
        {
            // Calcular posição alvo
            if (enablePrediction)
            {
                targetPosition = PredictBallPosition();
            }
            else
            {
                targetPosition = new Vector2(paddlePosition.x, ballPosition.y);
            }
            
            // Adicionar margem de erro baseada na dificuldade
            if (difficulty < 1.0f)
            {
                float error = Random.Range(-errorMargin, errorMargin) * (1.0f - difficulty);
                targetPosition.y += error;
            }
        }
        else
        {
            // Voltar para o centro quando a bola não está vindo
            targetPosition = new Vector2(paddlePosition.x, 0);
        }
        
        // Mover em direção à posição alvo
        float currentY = paddlePosition.y;
        float targetY = Mathf.Clamp(targetPosition.y, -boundY, boundY);
        
        Vector2 vel = rb2d.velocity;
        
        if (Mathf.Abs(targetY - currentY) > 0.1f)
        {
            float moveDirection = Mathf.Sign(targetY - currentY);
            float moveSpeed = Mathf.Lerp(speed * 0.5f, maxSpeed, difficulty);
            vel.y = moveDirection * moveSpeed;
        }
        else
        {
            vel.y = 0;
        }
        
        rb2d.velocity = vel;
        
        // Limitar posição
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -boundY, boundY);
        transform.position = pos;
    }
    
    Vector2 PredictBallPosition()
    {
        if (ballRb == null) return ball.position;
        
        Vector2 ballPos = ball.position;
        Vector2 ballVel = ballRb.velocity;
        
        // Calcular onde a bola estará quando chegar à posição da IA
        float timeToReachPaddle = Mathf.Abs((transform.position.x - ballPos.x) / ballVel.x);
        
        // Limitar o tempo de predição
        timeToReachPaddle = Mathf.Min(timeToReachPaddle, predictionTime);
        
        Vector2 predictedPosition = ballPos + ballVel * timeToReachPaddle;
        
        // Considerar rebote nas paredes superior e inferior
        if (predictedPosition.y > boundY || predictedPosition.y < -boundY)
        {
            // Simulação simples de rebote
            float bounceY = predictedPosition.y;
            while (bounceY > boundY || bounceY < -boundY)
            {
                if (bounceY > boundY)
                    bounceY = 2 * boundY - bounceY;
                else if (bounceY < -boundY)
                    bounceY = 2 * (-boundY) - bounceY;
            }
            predictedPosition.y = bounceY;
        }
        
        return predictedPosition;
    }
    
    // Método para ajustar dificuldade dinamicamente
    public void SetDifficulty(float newDifficulty)
    {
        difficulty = Mathf.Clamp01(newDifficulty);
        speed = Mathf.Lerp(5.0f, maxSpeed, difficulty);
    }
}