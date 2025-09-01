using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballControl : MonoBehaviour
{
  [Header("Configurações")]
  public float speed = 5.0f;
  public AudioSource source;

  private Rigidbody2D rb2d;
  private Vector2 startPosition;

  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    source = GetComponent<AudioSource>();
    startPosition = transform.position;
    LaunchBall();
  }

  void LaunchBall()
  {
    // Posição inicial
    transform.position = startPosition;

    // Direção aleatória
    float x = Random.Range(0, 2) == 0 ? -1 : 1;
    float y = Random.Range(-0.5f, 0.5f);

    rb2d.velocity = new Vector2(x * speed, y * speed);
  }

  void OnCollisionEnter2D(Collision2D coll)
  {
    // Som apenas quando bate na raquete
    if (coll.gameObject.CompareTag("Player"))
    {
      source.Play();
    }
  }

  void Update()
  {
    // Verificar gols e resetar
    if (transform.position.x < -10f || transform.position.x > 10f)
    {
      LaunchBall();
    }
  }
}