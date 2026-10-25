using UnityEngine;

// RequireComponent: al agregar este script, Unity agrega automaticamente
// los componentes que necesita (y no deja quitarlos por error)
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CambioColor))]
public class NaveEquilibrio : MonoBehaviour
{
    public float fuerza = 8f;

    private Rigidbody2D rb;
    private CambioColor cambioColor;
    private Vector2 direccion;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cambioColor = GetComponent<CambioColor>();
    }

    // Update: leer el teclado y cambiar el color
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direccion = new Vector2(horizontal, vertical);

        // Mejora A: el color depende de la dirección
        if (vertical > 0)
        {
            cambioColor.CambiarColor(Color.green);
        }
        else if (vertical < 0)
        {
            cambioColor.CambiarColor(Color.yellow);
        }
        else if (horizontal < 0)
        {
            cambioColor.CambiarColor(Color.red);
        }
        else if (horizontal > 0)
        {
            cambioColor.CambiarColor(Color.blue);
        }
    }

    // FixedUpdate: aplicar la fuerza dentro del ciclo de física
    void FixedUpdate()
    {
        rb.AddForce(direccion * fuerza);
    }
}