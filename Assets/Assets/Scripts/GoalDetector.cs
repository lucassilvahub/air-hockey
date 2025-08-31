using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [Header("Configuração do Gol")]
    public bool isLeftGoal = true; // true para gol esquerdo, false para gol direito
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if (GameManager.instance != null)
            {
                if (isLeftGoal)
                {
                    // Bola entrou no gol esquerdo = ponto para a IA (jogador direito)
                    GameManager.instance.AddScoreAI();
                }
                else
                {
                    // Bola entrou no gol direito = ponto para o jogador (jogador esquerdo)
                    GameManager.instance.AddScorePlayer();
                }
            }
        }
    }
}