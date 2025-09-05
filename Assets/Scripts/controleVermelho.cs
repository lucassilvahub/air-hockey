using UnityEngine;

public class controleVermelho : MonoBehaviour
{
    [Header("Config")]
    public float speed = 12f;
    public float boundY = 4f;
    public float boundXMin = -8f;
    public float boundXMax = -1f;
    public float kickPower = 1.5f;

    private Vector3 lastPos;

    void Start() => lastPos = transform.position;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        mousePos.x = Mathf.Clamp(mousePos.x, boundXMin, boundXMax);
        mousePos.y = Mathf.Clamp(mousePos.y, -boundY, boundY);

        transform.position = Vector3.Lerp(transform.position, mousePos, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            Vector3 paddleVel = (transform.position - lastPos) / Time.deltaTime;
            hit.rigidbody.velocity += (Vector2)paddleVel * kickPower;
        }
        lastPos = transform.position;
    }
}