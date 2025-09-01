using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Config")]
    public float speed = 5f;
    public float boundY = 4f;
    public float reactionDelay = 0.1f;
    
    private Transform ball;
    private Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ball = GameObject.FindWithTag("Ball").transform;
    }
    
    void FixedUpdate()
    {
        if (ball == null) return;
        
        float targetY = ball.position.y;
        float diff = targetY - transform.position.y;
        
        // AI só se move se a diferença for maior que o delay
        if (Mathf.Abs(diff) > reactionDelay)
        {
            float moveY = Mathf.Sign(diff) * speed;
            rb2d.velocity = new Vector2(0, moveY);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
        
        // Limita posição
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -boundY, boundY);
        transform.position = pos;
    }
}