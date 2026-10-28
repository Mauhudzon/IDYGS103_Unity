using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jugador_B_Movimiento : MonoBehaviour
{
    public float velocidad = 5f;

    public float anguloArmaInicial = 180f;

    private Rigidbody2D rb;
    private Vector2 direccion;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        direccion = Vector2.zero;


        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccion = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccion = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direccion = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direccion = Vector2.right;
        }
    }

    void FixedUpdate()
    {
      
        rb.linearVelocity = direccion * velocidad;

      
        if (direccion != Vector2.zero)
        {
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            rb.rotation = angulo - anguloArmaInicial;
        }
    }
}
