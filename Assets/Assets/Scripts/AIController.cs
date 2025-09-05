using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Config")]
    public float speed = 6f;           // Velocidade do goleiro
    public float boundY = 3.5f;        // Limite vertical
    public float reactionDelay = 0.1f; // Delay mínimo de reação
    public float defenseX = 6f;        // Linha de defesa (ajuste conforme lado da IA)
    public float minDistance = 0.6f;   // Distância mínima da bola (evita prensar)

    private Transform ball;
    private Rigidbody2D rb2d;
    private Rigidbody2D ballRb;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GameObject ballObj = GameObject.FindWithTag("Ball");
        if (ballObj != null)
        {
            ball = ballObj.transform;
            ballRb = ballObj.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (ball == null || ballRb == null) return;

        // Só reage se a bola estiver vindo na direção da IA
        bool ballComing = (ballRb.velocity.x > 0 && transform.position.x > 0) ||
                          (ballRb.velocity.x < 0 && transform.position.x < 0);

        if (!ballComing)
        {
            // Volta devagar pro centro quando a bola não vem
            MoveTo(0f);
            return;
        }

        // Previsão: onde a bola vai cruzar o eixo X do goleiro
        float timeToReach = Mathf.Abs((transform.position.x - ball.position.x) / ballRb.velocity.x);

        float predictedY = ball.position.y + ballRb.velocity.y * timeToReach;

        // Se a bola for bater na parede, reflete previsão
        float tableLimit = boundY;
        if (predictedY > tableLimit)
            predictedY = tableLimit - (predictedY - tableLimit);
        else if (predictedY < -tableLimit)
            predictedY = -tableLimit - (predictedY + tableLimit);

        // Evita prensar a bola se estiver muito perto
        if (Vector2.Distance(transform.position, ball.position) < minDistance)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }

        // Move em direção ao Y previsto
        MoveTo(predictedY);
    }

    private void MoveTo(float targetY)
    {
        float diff = targetY - transform.position.y;

        if (Mathf.Abs(diff) > reactionDelay)
        {
            float moveY = Mathf.Sign(diff) * speed;
            rb2d.velocity = new Vector2(0, moveY);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }

        // Limita posição no campo de jogo
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -boundY, boundY);
        pos.x = Mathf.Sign(transform.position.x) * defenseX; // mantém na linha de defesa
        transform.position = pos;
    }
}