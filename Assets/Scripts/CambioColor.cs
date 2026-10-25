using UnityEngine;

public class CambioColor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Guardamos la referencia al Sprite Renderer de ESTE objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Metodo publico: otros scripts pueden llamarlo
    public void CambiarColor(Color nuevoColor)
    {
        spriteRenderer.color = nuevoColor;
    }
}
