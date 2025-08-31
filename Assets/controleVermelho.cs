using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleVermelho : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float mouseSmoothing = 8.0f;     // Suavização do movimento do mouse (mais alto = mais suave)

    [Header("Limitadores do Campo")]
    public float boundY = 10f;            // Limites verticais (superior/inferior)
    public float boundXMin = -8.5f;         // Limite esquerdo (próximo da parede esquerda)
    public float boundXMax = -0.2f;         // Limite direito (meio do campo - impede atravessar)

    [Header("Audio")]
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMouseMovement();
    }

    void HandleMouseMovement()
    {
        // Converter posição do mouse para coordenadas do mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Garantir que Z seja 0 para 2D

        // Aplicar limitadores ANTES de mover
        Vector3 targetPos = ApplyBounds(mousePos);

        // Suavizar movimento para evitar "teleporte" instantâneo
        Vector3 currentPos = transform.position;
        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, mouseSmoothing * Time.deltaTime);

        transform.position = newPos;
    }

    Vector3 ApplyBounds(Vector3 desiredPosition)
    {
        Vector3 clampedPos = desiredPosition;

        // Limitar movimento vertical (superior/inferior)
        clampedPos.y = Mathf.Clamp(clampedPos.y, -boundY, boundY);

        // Limitar movimento horizontal (lado do jogador apenas)
        clampedPos.x = Mathf.Clamp(clampedPos.x, boundXMin, boundXMax);

        return clampedPos;
    }

    // Detectar colisões para efeito sonoro
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Método para ajustar limites dinamicamente (útil para diferentes tamanhos de campo)
    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        boundXMin = minX;
        boundXMax = maxX;
        boundY = Mathf.Max(Mathf.Abs(minY), Mathf.Abs(maxY)); // boundY é sempre positivo
    }

    // Método para verificar se o jogador está tentando sair dos limites
    public bool IsAtBoundary()
    {
        Vector3 pos = transform.position;
        return pos.x <= boundXMin + 0.1f || pos.x >= boundXMax - 0.1f ||
               pos.y <= -boundY + 0.1f || pos.y >= boundY - 0.1f;
    }

    // Feedback visual dos limites no Editor (apenas para desenvolvimento)
    void OnDrawGizmosSelected()
    {
        // Desenhar área permitida para o jogador
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((boundXMin + boundXMax) / 2, 0, 0);
        Vector3 size = new Vector3(boundXMax - boundXMin, boundY * 2, 0);
        Gizmos.DrawWireCube(center, size);

        // Desenhar linha do meio campo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(boundXMax, -boundY, 0), new Vector3(boundXMax, boundY, 0));
    }
}