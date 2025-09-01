using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 5.0f;
    public float boundY = 4f;
    
    private Transform ball;
    private Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ball = GameObject.FindWithTag("Ball").transform;
    }
    
    void Update()
    {
        if (ball != null)
        {
            MoveTowardsBall();
        }
    }
    
    void MoveTowardsBall()
    {
        float targetY = ball.position.y;
        float currentY = transform.position.y;
        
        // Mover em direção à bola
        if (Mathf.Abs(targetY - currentY) > 0.2f)
        {
            float direction = Mathf.Sign(targetY - currentY);
            rb2d.velocity = new Vector2(0, direction * speed);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
        
        // Limitar posição
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -boundY, boundY);
        transform.position = pos;
    }
}