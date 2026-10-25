using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidad = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 direccion;

    void Start()
    {
        // Busca los componentes en ESTE mismo GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Color inicial: gris, antes de moverse
        spriteRenderer.color = Color.gray;
    }

    // Update: leer el teclado y cambiar el color
    void Update()
    {
        direccion = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            direccion += Vector2.left;
            spriteRenderer.color = Color.orange;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            direccion += Vector2.right;
            spriteRenderer.color = Color.orange;
        }

        // Si el objeto NO tiene Rigidbody2D, se mueve con transform
        if (rb == null)
        {
            transform.Translate(direccion.normalized * velocidad * Time.deltaTime);
        }
    }

    // FixedUpdate: mover con física (va al ritmo del motor de física)
    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + direccion.normalized * velocidad * Time.fixedDeltaTime);
        }
    }
}
