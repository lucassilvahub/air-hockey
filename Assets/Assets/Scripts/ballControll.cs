using UnityEngine;

public class ballControl : MonoBehaviour
{
  [Header("Config")]
  public float speed = 5f;
  public float maxSpeed = 12f;
  public AudioSource source;

  private Rigidbody2D rb2d;
  private Vector2 startPos;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    source = GetComponent<AudioSource>();
    startPos = transform.position;
    LaunchBall();
  }

  public void LaunchBall()
  {
    transform.position = startPos;
    float x = Random.Range(0, 2) == 0 ? -1 : 1;
    float y = Random.Range(-0.5f, 0.5f);
    rb2d.velocity = new Vector2(x, y).normalized * speed;
  }

  void OnCollisionEnter2D(Collision2D coll)
  {
    if (coll.gameObject.CompareTag("Player"))
    {
      source.Play();

      // Rebote baseado na posição da colisão
      float diff = (transform.position.y - coll.transform.position.y) / 2f;
      float dir = transform.position.x > 0 ? -1 : 1;

      rb2d.velocity = new Vector2(dir * speed, diff * speed);
    }
  }

  void FixedUpdate()
  {
    // Limita velocidade e evita travamento
    if (rb2d.velocity.magnitude > maxSpeed)
      rb2d.velocity = rb2d.velocity.normalized * maxSpeed;

    if (Mathf.Abs(rb2d.velocity.x) < 1f)
      rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * speed, rb2d.velocity.y);

    // Reset nos gols
    if (Mathf.Abs(transform.position.x) > 10f)
      LaunchBall();
  }
}