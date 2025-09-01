using UnityEngine;

public class controleVermelho : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 8f;
    public float boundY = 4f;
    public float boundXMin = -8f;
    public float boundXMax = -1f;
    
    void Update()
    {
        HandleMouse();
    }
    
    void HandleMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        
        // Aplicar limites
        mousePos.x = Mathf.Clamp(mousePos.x, boundXMin, boundXMax);
        mousePos.y = Mathf.Clamp(mousePos.y, -boundY, boundY);
        
        // Mover suavemente
        transform.position = Vector3.Lerp(transform.position, mousePos, speed * Time.deltaTime);
    }
}